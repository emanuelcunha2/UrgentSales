using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IncompleteBoxes : System.Web.UI.Page
{
    private DatabaseService DbService = new DatabaseService();

    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        var boxes = DbService.GetIncompleteBoxes();
        incompleteBoxesList.DataSource = boxes;
        incompleteBoxesList.DataBind();
    }

}