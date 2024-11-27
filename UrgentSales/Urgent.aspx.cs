using Microsoft.Owin.Security.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Urgent : Page
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

    public List<GapRecord> gapRecords
    {
        get { return ViewState["GapRecords"] as List<GapRecord>; }
        set { ViewState["GapRecords"] = value; }
    }

    public List<IncompleteBoxesRequest> incompleteBoxesRequests
    {
        get { return ViewState["IncompleteBoxesRequests"] as List<IncompleteBoxesRequest>; }
        set { ViewState["IncompleteBoxesRequests"] = value; }
    }

    public List<string> filterDatabaseCompletionModes
    {
        get { return ViewState["FilterDatabaseCompletionModes"] as List<string>; }
        set { ViewState["FilterDatabaseCompletionModes"] = value; }
    }
    
    private DatabaseService DbService = new DatabaseService();


    public string actionBoxRequest
    {
        get { return ViewState["ActionBoxRequest"] as string; }
        set { ViewState["ActionBoxRequest"] = value; }
    }

    public string idBoxRequest
    {
        get { return ViewState["IdBoxRequest"] as string; }
        set { ViewState["IdBoxRequest"] = value; }
    }
      

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            selectedBuilding = "ALL";
            selectedDeposits = "2333/3333";
            selectedProdDeposits = "2332/3332";
            sapDate = DbService.GetLastDateSAP();

            BindData();

            List<IncompleteBoxesRequest> reqs = DbService.GetIncompleteBoxesRequests();
            IncompleteBoxRequests.DataSource = reqs;
            IncompleteBoxRequests.DataBind(); 
        }
    }

    private void BindData()
    {
        var records = DbService.GetTodayShipmentGapRecords(selectedBuilding);
        rptRecords.DataSource = records;
        gapRecords = records;

        if (records.Count > 0)
        {
            noUrgentSalesLbl.Visible = false;
        }
        else
        {
            noUrgentSalesLbl.Visible = true;
        }

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


    protected void InsertIncBoxRequest_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);
            LoadData();
            return;
        }

        // Process the form data
        string pn = txtInsertReqPn.Value;

        // Remove Whitespaces
        pn = Regex.Replace(pn, @"\s+", "");

        int qty = 0;
        DateTime dt;
        bool verifiedQty = Int32.TryParse(txtInsertReqQty.Value, out qty) ;
        bool verifiedDate = DateTime.TryParse(txtInsertReqDate.Value, out dt);
        string comment = txtInsertReqComment.Value;

        // Check if all necessary data Filled
        if (pn.Length == 0 || qty <= 0 || !verifiedDate)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Informações em falta');", true);
            RefreshPage();
            return;
        }

        string user = "none";

        HttpCookie authCookie = Request.Cookies[".ASPXAUTH"];
        if (authCookie != null)
        {
            var value = FormsAuthentication.Decrypt(authCookie.Value);
            user = value.Name;
        }

        var res = DbService.InsertIncompleteBoxRequest(pn, qty, comment, dt, user);

        LoadData();

        if (!res)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro ao criar Pedido, verifique se tem permissões, ou erros técnicos');", true);
        }
        RefreshPage();
    }


    private void RefreshPage()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "refreshPage", "location.reload();", true);
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
        gapRecords = records;
        rptRecords.DataBind();


        if(records.Count > 0)
        {
            noUrgentSalesLbl.Visible = false;
        }
        else
        {
            noUrgentSalesLbl.Visible = true;
        }

        List<IncompleteBoxesRequest> reqs = DbService.GetIncompleteBoxesRequests();
        IncompleteBoxRequests.DataSource = reqs;
        IncompleteBoxRequests.DataBind();
    }

    protected void btnsBxsRq_Command(object sender, CommandEventArgs e)
    {
        TextareaComment.Visible = false;

        if (e.CommandName == "Insert")
        {
            actionBoxRequest = "Insert";
            ConfirmChangesRequestLbl.InnerText = "Confirmar Venda";
            idBoxRequest = Convert.ToString(e.CommandArgument);
        } 

        if(e.CommandName == "Delete")
        {
            actionBoxRequest = "Delete";
            ConfirmChangesRequestLbl.InnerText = "Confirmar Remoção";
            idBoxRequest = Convert.ToString(e.CommandArgument);
        }

        if (e.CommandName == "Comment")
        {
            TextareaComment.Visible = true;
            actionBoxRequest = "Comment";
            ConfirmChangesRequestLbl.InnerText = "Confirmar comentário";

            idBoxRequest = Convert.ToString(e.CommandArgument);
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "showSetupList", " $('#confirmModal').modal('show'); ", true);
        LoadData();
    }

    protected void BoxRequestModalAlterations_Click(object sender, EventArgs e)
    {
        UpdatePanelCommentInsert.Visible = false;
        string user = "none";

        HttpCookie authCookie = Request.Cookies[".ASPXAUTH"];
        if(authCookie != null)
        {
            var value = FormsAuthentication.Decrypt(authCookie.Value);
            user = value.Name;
        }

        if (actionBoxRequest == "Insert")
        { 
            var res = DbService.IncompleBoxUpdateCompleted(Int32.Parse(idBoxRequest),user);
            
            if (!res)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, verifique se tem permissões, ou erros técnicos');", true);
                RefreshPage();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Vendido com sucesso!');", true);
                RefreshPage();
            }

            return;
        }

        if (actionBoxRequest == "Delete")
        {

            var res =  DbService.IncompleBoxReqDelete(Int32.Parse(idBoxRequest), user); 
            if (!res)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, verifique se tem permissões, ou erros técnicos');", true);
                RefreshPage();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Eliminado com sucesso');", true);
                RefreshPage();
            }

            return;
        }

        if (actionBoxRequest == "Comment")
        {
            var res = DbService.IncompleBoxReqComment(Int32.Parse(idBoxRequest), TextareaComment.Value);

            if (!res)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, verifique se tem permissões, ou erros técnicos');", true);
                RefreshPage();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Comentado com sucesso');", true);
                RefreshPage();
            }

            return;
        }

    }
}