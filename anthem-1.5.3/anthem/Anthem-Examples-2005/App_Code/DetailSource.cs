using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class DetailSource
{
    private DataTable GetDataTable(int id)
    {
        DataTable details = new DataTable();
        details.Columns.Add("id", typeof(int));
        details.Columns.Add("a", typeof(string));
        details.Columns.Add("b", typeof(string));
        details.Columns.Add("c", typeof(bool));
        details.Columns.Add("d", typeof(int));
        details.Columns.Add("e", typeof(float));
        details.Columns.Add("f", typeof(decimal));
        details.PrimaryKey = new DataColumn[] { details.Columns["id"] };

        DataTable dt = HttpContext.Current.Session["dt"] as DataTable;
        if (dt != null)
        {
            DataRow dr = dt.Rows.Find(id);
            if (dr != null)
                details.Rows.Add(dt.Rows.Find(id).ItemArray);
        }
        return details;
    }

    public DataTable Select(int id)
    {
        return GetDataTable(id);
    }

    public void Update(int id, string a, string b, bool c, int d, float e, decimal f)
    {
        DataTable dt = HttpContext.Current.Session["dt"] as DataTable;
        if (dt != null)
        {
            DataRow dr = dt.Rows.Find(id);
            dr["a"] = a;
            dr["b"] = b;
            dr["c"] = c;
            dr["d"] = d;
            dr["e"] = e;
            dr["f"] = f;
        }
    }

    public void Delete(int id)
    {
        DataTable dt = HttpContext.Current.Session["dt"] as DataTable;
        if (dt != null)
        {
            DataRow dr = dt.Rows.Find(id);
            dr.Delete();
        }
    }
}
