using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    public string selectedDeposits
    {
        get { return ViewState["SelectedDeposits"] as string; }
        set { ViewState["SelectedDeposits"] = value; }
    }

    public string selectedProdDeposits
    {
        get { return ViewState["SelectedProdDeposits"] as string; }
        set { ViewState["SelectedProdDeposits"] = value; }
    }

    public string sapDate
    {
        get { return ViewState["SapDate"] as string; }
        set { ViewState["SapDate"] = value; }
    }

    public string selectedBuilding
    {
        get { return ViewState["SelectedBuilding"] as string; }
        set { ViewState["SelectedBuilding"] = value; }
    }

    private DatabaseService DbService = new DatabaseService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            selectedBuilding = "ALL";
            selectedDeposits = "2333/3333";
            selectedProdDeposits = "2332/3332";
            BindData();
        }
    }

    private void BindData()
    {
        var records = DbService.GetTodayShipmentGapRecords(selectedBuilding);
        rptRecords.DataSource = records;
        rptRecords.DataBind(); 
    }

    private void ResetButtonStyles()
    {
        ALL.CssClass = "";
        VS1.CssClass = "";
        VS2.CssClass = "";
    }

    protected void Button_Click(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            selectedBuilding = button.Text;

            switch (selectedBuilding)
            {
                case "ALL":
                    selectedDeposits = "2333/3333";
                    selectedProdDeposits = "2332/3332";
                    break;
                case "VS1":
                    selectedDeposits = "3333";
                    selectedProdDeposits = "3332";
                    break;
                case "VS2":
                    selectedDeposits = "2333";
                    selectedProdDeposits = "2332";
                    break;
                default: 
                    break; 
            }
             
            ResetButtonStyles();
            button.CssClass = "btn btn-primary mx-1 selectedButtonStyle"; 

            LoadData();
        }
    }

    protected void UpdateTimer_Tick(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void LoadData()
    {
        sapDate = DbService.GetLastDateSAP();
        var records = DbService.GetTodayShipmentGapRecords(selectedBuilding);
        rptRecords.DataSource = records;
        rptRecords.DataBind();
    }

}