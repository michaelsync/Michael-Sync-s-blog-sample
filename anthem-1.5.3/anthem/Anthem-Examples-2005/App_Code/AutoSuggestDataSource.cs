using System.Data;
using System.Web;

public class AutoSuggestDataSource
{
    public System.Data.DataSet Select()
    {
        System.Data.DataSet ds = HttpContext.Current.Session["autods"] as System.Data.DataSet;
        if (ds == null)
        {
            ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Columns.Add("name", typeof(string));
            dt.Rows.Add(new object[] { "Allan & Ampersand" });
            dt.Rows.Add(new object[] { "Allison" });
            dt.Rows.Add(new object[] { "Aniston" });
            dt.Rows.Add(new object[] { "Arjona" });
            dt.Rows.Add(new object[] { "Arkenstone" });
            dt.Rows.Add(new object[] { "Arkin" });
            dt.Rows.Add(new object[] { "Arnold" });
            dt.Rows.Add(new object[] { "Arthur" });
            dt.Rows.Add(new object[] { "Astin" });
            dt.Rows.Add(new object[] { "Bert" });
            dt.Rows.Add(new object[] { "Bjorn" });
            dt.Rows.Add(new object[] { "Bon" });
            dt.Rows.Add(new object[] { "Burt" });

            HttpContext.Current.Session["autods"] = ds;
        }
        return ds;
    }

    // START added code by prs - 13nov06
    // create a two column dataset
    public System.Data.DataSet Select2()
    {
        System.Data.DataSet ds = HttpContext.Current.Session["autods"] as System.Data.DataSet;
        if (ds == null)
        {
            ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Rows.Add(new object[] { 1, "Ampersand" });
            dt.Rows.Add(new object[] { 2, "Allison" });
            dt.Rows.Add(new object[] { 3, "Aniston" });
            dt.Rows.Add(new object[] { 4, "Arjona" });
            dt.Rows.Add(new object[] { 5, "Arkenstone" });
            dt.Rows.Add(new object[] { 6, "Arkin" });
            dt.Rows.Add(new object[] { 7, "Arnold" });
            dt.Rows.Add(new object[] { 8, "Arthur" });
            dt.Rows.Add(new object[] { 9, "Astin" });
            dt.Rows.Add(new object[] { 10, "Bert" });
            dt.Rows.Add(new object[] { 11, "Bjorn" });
            dt.Rows.Add(new object[] { 12, "Bon" });
            dt.Rows.Add(new object[] { 13, "Burt" });

            HttpContext.Current.Session["autods"] = ds;
        }
        return ds;
    }
    // END added code by prs - 13nov06
}