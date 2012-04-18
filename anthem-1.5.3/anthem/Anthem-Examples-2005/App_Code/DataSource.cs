using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class DataSource
{
    private DataTable GetDataTable()
    {
        DataTable dt = HttpContext.Current.Session["dt"] as DataTable;
        if (dt == null)
        {
            dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("a", typeof(string));
            dt.Columns.Add("b", typeof(string));
            dt.Columns.Add("c", typeof(bool));
            dt.Columns.Add("d", typeof(int));
            dt.Columns.Add("e", typeof(float));
            dt.Columns.Add("f", typeof(decimal));
            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

            for (int i = 1; i < 10; ++i)
            {
                dt.Rows.Add(new object[] { i, "a" + i, "b" + (10 - i), i % 2 == 0, i*2, i*1.14159, i*3.14});
            }

            HttpContext.Current.Session["dt"] = dt;
        }
        return dt;
    }

    public static void RemoveDataTable()
    {
        HttpContext.Current.Session.Remove("dt");
    }

    public DataTable Select()
    {
        return GetDataTable();
    }

    public void Update(int id, string a, string b, bool c)
    {
        DataTable dt = GetDataTable();
        DataRow dr = dt.Rows.Find(id);
        dr["a"] = a;
        dr["b"] = b;
        dr["c"] = c;
    }

    public void Delete(int id)
    {
        DataTable dt = GetDataTable();
        DataRow dr = dt.Rows.Find(id);
        dr.Delete();
    }
}
