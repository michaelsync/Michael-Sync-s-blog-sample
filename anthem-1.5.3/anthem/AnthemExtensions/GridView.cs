using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;
using Anthem;

namespace AnthemExtensions {
#if V2    
    public class GridViewRowClickEventArgs : EventArgs {
        private ASP.GridViewRow row;        
        private string commandName;
        private string commandArgument;

        #region ctor
        public GridViewRowClickEventArgs(ASP.GridViewRow row, string commandName, string commandArgument) {
            this.commandName = commandName;
            this.commandArgument = commandArgument;
            this.row = row;
        }

        #endregion

        #region Properties
        public int NewSelectedIndex {
            get { return this.row.RowIndex; }
        }

        public string CommandName {
            get { return this.commandName; }
        }

        public string CommandArgument {
            get { return this.commandArgument; }
        }

        public ASP.GridViewRow Row {
            get { return this.row; }
        }

        #endregion
    }

    [DefaultEvent("ItemDataBound")]
    public class GridView : Anthem.GridView {
        private static readonly object EventRowClick;
        private int virtualItemCount;
        private int sortColumnIndex = -1;
        private bool useCoolPager = false;
        private bool hidePagerOnOnePage = false;
        private string totalRecordString = "";

        private RowClickEvent rowClickEvent;
        private ASP.TableItemStyle mouseOverRowStyle;
        private ASP.TableItemStyle sortedColumnStyle;
        private ASP.TableItemStyle sortedColumnHeaderRowStyle;
        private readonly Type[] NORENDER_CLICK_TYPES = new Type[] {
            typeof(ASP.HyperLink),
            typeof(ASP.LinkButton)
        };

        #region cctor
        static GridView() {
            GridView.EventRowClick = new object();
        }

        #endregion

        #region External Events & Methods
        public event EventHandler<GridViewRowClickEventArgs> RowClick {
            add { base.Events.AddHandler(GridView.EventRowClick, value); }
            remove { base.Events.RemoveHandler(GridView.EventRowClick, value); }
        }

        public virtual void OnRowClick(GridViewRowClickEventArgs e) {
            this.SelectedIndex = e.NewSelectedIndex;
            EventHandler<GridViewRowClickEventArgs> rowClickHandler = (EventHandler<GridViewRowClickEventArgs>)base.Events[GridView.EventRowClick];
            if (rowClickHandler != null)
                rowClickHandler(this, e);
        }

        #endregion

        #region Internals
        private void SetSortHeaderAttributes(ASP.GridViewRowEventArgs e) {
            bool images = (this.AscImage != String.Empty && this.DescImage != String.Empty);
            for (int i = 0; i < e.Row.Cells.Count; i++) {
                ASP.TableCell td = e.Row.Cells[i];
                if (td.HasControls()) {
                    ASP.LinkButton button = td.Controls[0] as ASP.LinkButton;
                    if (button != null) {                        
                        if (this.SortExpression == button.CommandArgument) {
                            td.ApplyStyle(this.SortedColumnHeaderRowStyle);
                            this.sortColumnIndex = i;
                            if (images) {
                                ImageButton btn = new ImageButton();
                                btn.CommandName = button.CommandName;
                                btn.CommandArgument = button.CommandArgument;                                
                                btn.ImageUrl = (this.SortDirection == ASP.SortDirection.Ascending ? this.AscImage : this.DescImage);
                                td.Controls.Add(new LiteralControl("&nbsp;"));
                                td.Controls.Add(btn);
                                td.Controls.Add(new LiteralControl("&nbsp;"));
                            }
                        }
                    }
                }
            }
        }

        private void DetermineRowClickAction(ASP.GridViewRow row) {
            string commandName = null;
            string commandArg = null;
            switch (this.RowClickEvent) {
                case RowClickEvent.Click  :
                    commandName = "click";
                    commandArg = row.RowIndex.ToString();
                    break;
                    
                case RowClickEvent.Edit   :
                    commandName = "edit";
                    commandArg = row.RowIndex.ToString();
                    break;

                case RowClickEvent.Select :
                    commandName = "select";
                    commandArg = row.RowIndex.ToString();
                    break;
            }
            if (!Utils.ContainsControlType(row, NORENDER_CLICK_TYPES))
                this.SetClickEvent(row, commandName, commandArg); // set on tr
            else {
                foreach (ASP.TableCell cell in row.Cells)
                    if (!Utils.ContainsControlType(cell, NORENDER_CLICK_TYPES))
                        this.SetClickEvent(cell, commandName, commandArg);
            }
        }

        //ASP.TableCell cell or ASP.GridViewRow row
        private void SetClickEvent(ASP.WebControl row, string commandName, string commandArgument) {
            if (!this.AddCallBacks) {
                string eventText = commandName + "$" + commandArgument;
                row.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(this, eventText));
            }
            else {
                string command = (String.IsNullOrEmpty(commandName) && String.IsNullOrEmpty(commandArgument) ? "" : commandName + "$" + commandArgument);
                string script = String.Format("Anthem_FireCallBackEvent(this,event,'{0}','{1}',{2},'{3}','','{4}',{5},{6},{7},{8},true,true);return false;",
                                              this.UniqueID,
                                              command,
                                              "false",
                                              String.Empty,
                                              base.TextDuringCallBack,
                                              base.EnabledDuringCallBack ? "true" : "false",
                                              (String.IsNullOrEmpty(base.PreCallBackFunction)  ? "null" : base.PreCallBackFunction),
                                              (String.IsNullOrEmpty(base.PostCallBackFunction) ? "null" : base.PostCallBackFunction),
                                              (String.IsNullOrEmpty(base.CallBackCancelledFunction) ? "null" : base.CallBackCancelledFunction));
                Manager.AddScriptAttribute(row, "onclick", script);
            }
        }

        private void CreateCoolPager(ASP.TableRow row, int columnSpan, ASP.PagedDataSource pagedDataSource) {
            int pageIndex = pagedDataSource.CurrentPageIndex;
            int pageCount = pagedDataSource.PageCount;
            int pageSize = pagedDataSource.PageSize;            
            int total = pagedDataSource.DataSourceCount;

            ASP.TableCell td = new ASP.TableCell();            
            DropDownList ddlPageSelector = new DropDownList();
            Button btnFirst = new Button();
            Button btnLast = new Button();
            Button btnNext = new Button();
            Button btnPrev = new Button();
            Label lblTotal = new Label();

            td.ColumnSpan = columnSpan;
            row.Cells.Add(td);
            td.Controls.Add(new LiteralControl("&nbsp;Page : "));
            td.Controls.Add(ddlPageSelector);
            td.Controls.Add(btnFirst);
            td.Controls.Add(btnPrev);
            td.Controls.Add(btnNext);
            td.Controls.Add(btnLast);
            td.Controls.Add(lblTotal);
            
            btnNext.Text = ">";
            btnNext.CommandArgument = "Next";
            btnNext.CommandName = "Page";
            
            btnLast.Text = ">>";
            btnLast.CommandArgument = "Last";
            btnLast.CommandName = "Page";

            btnFirst.Text = "<<";
            btnFirst.CommandArgument = "First";
            btnFirst.CommandName = "Page";
            
            btnPrev.Text = "<";
            btnPrev.CommandArgument = "Prev";
            btnPrev.CommandName = "Page";

            lblTotal.Text = this.TotalRecordString + "&nbsp;" + total.ToString();
            btnFirst.Enabled = btnPrev.Enabled = (pageIndex != 0);
            btnNext.Enabled = btnLast.Enabled = (pageIndex < (pageCount - 1));
            ddlPageSelector.Items.Clear();

            if (this.AddCallBacks) 
                ddlPageSelector.AutoCallBack = true;
            else
                ddlPageSelector.AutoPostBack = true;
            for (int i = 1; i <= pageCount; i++) 
                ddlPageSelector.Items.Add(i.ToString());
            ddlPageSelector.SelectedIndex = pageIndex;
            ddlPageSelector.SelectedIndexChanged += delegate {
                this.PageIndex = ddlPageSelector.SelectedIndex;
                this.DataBind();
            };
        }

        #endregion

        #region Override Methods
        protected override void OnLoad(EventArgs e) {
            if (!this.EnableViewState) {
                base.UpdateAfterCallBack = true;
                base.RequiresDataBinding = true;
            }
            base.OnLoad(e);
        }

        protected override void OnRowCreated(ASP.GridViewRowEventArgs e) {
            base.OnRowCreated(e);
            if (e.Row != null) {
                switch (e.Row.RowType) {
                    case ASP.DataControlRowType.Header  : this.SetSortHeaderAttributes(e); break;
                    case ASP.DataControlRowType.DataRow :
                        if (this.sortColumnIndex > -1) {
                            ASP.TableCell td = e.Row.Cells[this.sortColumnIndex];
                            td.ApplyStyle(this.SortedColumnRowStyle);
                        }
                        break;
                }
            }
        }

        protected override void RaisePostBackEvent(string eventArgument) {
            int index = eventArgument.IndexOf('$');
            if (index >= 0) {
                string commandName = eventArgument.Substring(0, index);
                string commandArg = eventArgument.Substring(index + 1);
                switch (commandName) {
                    case "rc":
                        int rowIndex = Int32.Parse(commandArg);
                        ASP.GridViewRow row = base.Rows[rowIndex];
                        GridViewRowClickEventArgs args = new GridViewRowClickEventArgs(row, commandName, commandArg);
                        this.OnRowClick(args);
                        break;

                    default:
                        base.RaisePostBackEvent(eventArgument);
                        break;
                }
            }
        }

        protected override void LoadControlState(object savedState) {
            object[] array = (object[])savedState;
            base.LoadControlState(array[0]);
            this.rowClickEvent = (RowClickEvent)array[1];
            this.useCoolPager = (bool)array[2];
            this.hidePagerOnOnePage = (bool)array[3];
        }

        protected override object SaveControlState() {
            return new object[] { 
                base.SaveControlState(),
                this.rowClickEvent,
                this.useCoolPager,
                this.hidePagerOnOnePage
            };
        }

        protected override void InitializePager(ASP.GridViewRow row, int columnSpan, ASP.PagedDataSource pagedDataSource) {
            this.virtualItemCount = pagedDataSource.DataSourceCount;
            bool hide = (this.hidePagerOnOnePage && pagedDataSource.PageCount <= 1);
            if (!hide) {
                if (this.UseCoolPager) 
                    this.CreateCoolPager(row, columnSpan, pagedDataSource);
                else
                    base.InitializePager(row, columnSpan, pagedDataSource);
            }
        }

        protected override void PrepareControlHierarchy() {
            base.PrepareControlHierarchy();
            if (this.RowClickEvent != RowClickEvent.None || !this.MouseOverRowStyle.IsEmpty) {
                foreach (ASP.GridViewRow row in base.Rows) {
                    if (row.RowState == ASP.DataControlRowState.Normal || row.RowState == ASP.DataControlRowState.Alternate) {
                        if (this.RowClickEvent != RowClickEvent.None)
                            this.DetermineRowClickAction(row);

                        if (!this.MouseOverRowStyle.IsEmpty) {
                            row.Attributes.Add("onmouseover", Utils.GetJavaScriptStyleString(this.MouseOverRowStyle));
                            row.Attributes.Add("onmouseout", Utils.GetJavaScriptStyleString(row.ControlStyle));
                        }
                    }
                }
            }
        }

        #endregion

        #region Style Properties
        // use ref for some stupid reason even though a class passes by ref otherwise ASP compiler freaks! :P
        private void SetTableItemStyle(ref ASP.TableItemStyle style) {
            style = new ASP.TableItemStyle();
            if (base.IsTrackingViewState)
                ((IStateManager)style).TrackViewState();
        }

        [Themeable(true)]
        [Bindable(true)]
        [Category("Styles")]
        [Description("Sorted column row header styling on grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ASP.TableItemStyle SortedColumnHeaderRowStyle {
            get {
                if (this.sortedColumnHeaderRowStyle == null) 
                    this.SetTableItemStyle(ref this.sortedColumnHeaderRowStyle);
                return this.sortedColumnHeaderRowStyle;
            }
        }

        [Themeable(true)]
        [Bindable(true)]
        [Category("Styles")]
        [Description("Sorted column row styling on grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ASP.TableItemStyle SortedColumnRowStyle {
            get {
                if (this.sortedColumnStyle == null) 
                    this.SetTableItemStyle(ref this.sortedColumnStyle);
                return this.sortedColumnStyle;
            }
        }

        [Themeable(true)]
        [Bindable(true)]
        [Category("Styles")]
        [Description("Mouse over styling on grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ASP.TableItemStyle MouseOverRowStyle {
            get {
                if (this.mouseOverRowStyle == null)
                    this.SetTableItemStyle(ref this.mouseOverRowStyle);
                return this.mouseOverRowStyle;
            }
        }

        #endregion

        #region Properties
        [Browsable(false)]
        public int VirtualItemCount {
            get { return this.virtualItemCount; }
        }

        [Browsable(false)]
        public int SortColumnIndex {
            get { return this.sortColumnIndex; }
        }

        [Themeable(true)]
        [Category("Extended")]
        [Description("Use a better pager without having to create a complex template")]
        public bool UseCoolPager {
            get { return this.useCoolPager; }
            set { this.useCoolPager = value; }
        }

        [Themeable(true)]
        [Category("Extended")]
        [Description("Use a better pager without having to create a complex template")]
        public bool HidePagerOnOnePage {
            get { return this.hidePagerOnOnePage; }
            set { this.hidePagerOnOnePage = value; }
        }

        [Themeable(true)]
        [Category("Extended")]
        [DefaultValue("Total Records : ")]
        [Description("Total record string")]
        public string TotalRecordString {
            get { return this.totalRecordString; }
            set { this.totalRecordString = value; }
        }

        [Themeable(true)]
        [Bindable(true)]
        [DefaultValue(RowClickEvent.None)]
        [Category("Extended")]
        [Description("Enable row click event")]
        public RowClickEvent RowClickEvent {
            get { return this.rowClickEvent; }
            set { this.rowClickEvent = value; }
        }

        [UrlProperty]
        [Themeable(true)]
        [Category("Extended")]        
        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        [Description("Specifies path for the image used as an Ascending Sorting image")]
        public string AscImage {
            get {
                object obj = this.ViewState["AscImage"];
                return (obj == null ? String.Empty : (string)obj);
            }
            set {
                this.ViewState["AscImage"] = value;
            }
        }

        [UrlProperty]
        [Themeable(true)]
        [Category("Extended")]
        [Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]        
        [Description("Specifies path for the image used as a Descending Sorting image")]
        public string DescImage {
            get {
                object obj = this.ViewState["DescImage"];
                return (obj == null ? String.Empty : (string)obj);
            }
            set {
                this.ViewState["DescImage"] = value;
            }
        }

        #endregion
    }
#endif
}
