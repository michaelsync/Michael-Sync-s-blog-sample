using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace Excel_Export_DateTime {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Button1_Click(object sender, EventArgs e) {
            var datatable = CreateDataTableWithData();

            System.IO.StringWriter stringWriter = GetHtmlTagsfromDatagrid(datatable);

            WriteResponse(stringWriter);
        }

        private void WriteResponse(System.IO.StringWriter stringWriter) {
            Response.Clear();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.AppendHeader("Content-Disposition", "attachment;filename=myexcel.xls");
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        private static System.IO.StringWriter GetHtmlTagsfromDatagrid(DataTable datatable) {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWriter);
            DataGrid dg = new DataGrid();
            dg.DataSource = datatable;
            dg.DataBind();
            dg.RenderControl(htmlWrite);
            return stringWriter;
        }

        private static DataTable CreateDataTableWithData() {
            var datatable = CreateDataTable();

            datatable.Rows.Add(CreateDataRow(datatable, 1, "Michael Sync",
                "8/31/1982 3:19:40 PM"));
            datatable.Rows.Add(CreateDataRow(datatable, 1, "Shwesin Sync",
                "9/29/1982 3:00:32 PM"));
            datatable.Rows.Add(CreateDataRow(datatable, 1, "Elena Sync",
                "1/15/2011 00:00:01 PM"));
            datatable.Rows.Add(CreateDataRow(datatable, 1, "Tiffany Sync",
                "7/6/2012 00:00:01 PM"));

            datatable.AcceptChanges();
            return datatable;
        }

        private static DataTable CreateDataTable() {
            var datatable = new DataTable();
            datatable.Columns.Add("Id");
            datatable.Columns.Add("Name");
            datatable.Columns.Add("Date of Birth");
            return datatable;
        }

        private static DataRow CreateDataRow(DataTable datatable, int id, string name, string dob) {
            DataRow dr = datatable.NewRow();
            dr[0] = id;
            dr[1] = name;
            dr[2] = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dob); 
            //dr[2] = dob; // Wrong code

            return dr;
        }
    }
}