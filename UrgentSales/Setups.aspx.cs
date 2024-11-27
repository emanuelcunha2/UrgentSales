using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Setups : System.Web.UI.Page
{
    [WebMethod]
    public static bool SaveNewOrder(List<OrderItem> order)
    {
        DatabaseService dbService = new DatabaseService();

        if(!AuthUtils.IsUserLoggedIn())
        {  
            return false;
        }
        foreach(OrderItem i in order)
        {
            dbService.SetupUpdate(i.Designation, i.Project, i.Line, i.Prio);
        }
        return true;    
    }

    public List<Setup> SetupList
    {
        get
        {
            return (List<Setup>)ViewState["SetupList"];
        }
        set
        {
            ViewState["SetupList"] = value;
        }
    }

    public List<Setup> InactiveSetups
    {
        get
        {
            if (ViewState["InactiveSetups"] == null)
            {
                ViewState["InactiveSetups"] = DbService.GetSetups("Inactive");
            }
            return (List<Setup>)ViewState["InactiveSetups"];
        }
        set
        {
            ViewState["InactiveSetups"] = value;
        }
    }
     

    public List<Setup> ActiveSetups
    {
        get
        {
            if (ViewState["ActiveSetups"] == null)
            {
                ViewState["ActiveSetups"] = DbService.GetSetups("Active");
            }
            return (List<Setup>)ViewState["ActiveSetups"];
        }
        set
        {
            ViewState["ActiveSetups"] = value;
        }
    }

    public List<Setup> AssembleSetups
    {
        get
        {
            if (ViewState["AssembleSetups"] == null)
            {

                ViewState["AssembleSetups"] = DbService.GetSetups("Assemble");
            }
            return (List<Setup>)ViewState["AssembleSetups"];
        }
        set
        {
            ViewState["AssembleSetups"] = value;
        }
    }

    public List<Setup> DisassembleSetups
    {
        get
        {
            if (ViewState["DisassembleSetups"] == null)
            {
                ViewState["DisassembleSetups"] = DbService.GetSetups("Disassemble");
            }
            return (List<Setup>)ViewState["DisassembleSetups"];
        }
        set
        {
            ViewState["DisassembleSetups"] = value;
        }
    }

    public string SelectedSetup
    {
        get { return ViewState["SelectedSetup"] as string; }
        set { ViewState["SelectedSetup"] = value; }

    }
    private DatabaseService DbService = new DatabaseService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData(); 

            // Check if there is a message in the session
            if (Session["AlertMessage"] != null)
            {
                string message = Session["AlertMessage"].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + message + "');", true);
                Session["AlertMessage"] = null; // Clear the message after displaying it
            }
        }
    }

    private void RefreshData()
    {
        InactiveSetups = DbService.GetSetups("Inactive");
        ActiveSetups = DbService.GetSetups("Active");
        AssembleSetups = DbService.GetSetups("Assemble");
        DisassembleSetups = DbService.GetSetups("Disassemble");
    }

    private void BindData()
    { 
        InactiveSetupsRepeater.DataSource = InactiveSetups;
        InactiveSetupsRepeater.DataBind();

        ActiveSetupsRepeater.DataSource = ActiveSetups;
        ActiveSetupsRepeater.DataBind();

        AssembleSetupsRepeater.DataSource = AssembleSetups;
        AssembleSetupsRepeater.DataBind();

        DisassembleSetupsRepeater.DataSource = DisassembleSetups;
        DisassembleSetupsRepeater.DataBind();
    }

    protected void SetupInactive_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }
        foreach (Setup setup in InactiveSetups)
        {
            if(setup.IsChecked == true)
            {
                DbService.UpdateSetup("Assemble",setup.Id);
                RefreshData();
                BindData();
                RefreshPage();
            }
        }
    }

    protected void SetupActive_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }
        foreach (Setup setup in ActiveSetups)
        {
            if (setup.IsChecked == true)
            {
                DbService.UpdateSetup("Disassemble", setup.Id);
                RefreshData();
                BindData();
                RefreshPage();
            }
        }
    }

    protected void SetupAssemble_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }
        foreach (Setup setup in AssembleSetups)
        {
            if (setup.IsChecked == true)
            {
                DbService.UpdateSetup("Active", setup.Id);
                RefreshData();
                BindData();
                RefreshPage();
            }
        }
    }

    protected void SetupDisassemble_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }
        foreach (Setup setup in DisassembleSetups)
        {
            if (setup.IsChecked == true)
            {
                DbService.UpdateSetup("Inactive", setup.Id);
                RefreshData();
                BindData();
                RefreshPage();
            }
        }
    } 

    private void RefreshPage()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "refreshPage", "location.reload();", true);
    }

    protected void Inactive_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        RepeaterItem item = (RepeaterItem)chk.NamingContainer;

        // Get the current item index
        int itemIndex = item.ItemIndex;

        // Update the data source with the new checkbox value
        InactiveSetups[itemIndex].IsChecked = chk.Checked;
    }

    protected void Active_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        RepeaterItem item = (RepeaterItem)chk.NamingContainer;

        // Get the current item index
        int itemIndex = item.ItemIndex;

        // Update the data source with the new checkbox value
        ActiveSetups[itemIndex].IsChecked = chk.Checked;
    }

    protected void Assemble_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        RepeaterItem item = (RepeaterItem)chk.NamingContainer;

        // Get the current item index
        int itemIndex = item.ItemIndex;

        // Update the data source with the new checkbox value
        AssembleSetups[itemIndex].IsChecked = chk.Checked;
    }

    protected void Disassemble_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        RepeaterItem item = (RepeaterItem)chk.NamingContainer;

        // Get the current item index
        int itemIndex = item.ItemIndex;

        // Update the data source with the new checkbox value
        DisassembleSetups[itemIndex].IsChecked = chk.Checked;
    }

    protected void InsertSetup_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);
            RefreshPage();
            return;
        }

        // Process the form data
        string setup = txtSetup.Value;
        string project = txtProject.Value; ;
        string line = txtLine.Value;

        // Check if all data Filled
        if(setup.Length == 0 || project.Length == 0 || line.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Informações em falta');", true);
            RefreshPage();
            return;
        }

        var res = DbService.InsertSetup(setup, project, line);
         
        RefreshData();
        BindData();
        RefreshPage();

        if (!res)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro ao inserir Setup, verifique se o Setup já não existe');", true);
        }
    }

    protected void DeleteSetup_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);
            RefreshPage();
            return;

        }
        // Process the form data
        string setup = txtDeleteSetup.Value;
        var res = DbService.DeleteSetup(setup);

        RefreshData();
        BindData();
        RefreshPage();

        if (!res)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro ao eliminar Setup, verifique se o Setup existe');", true);
        }
    } 
    protected void ImportSetupList_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            Session["AlertMessage"] = "Precisa de permissões para fazer esta operação";
            Response.Redirect(Request.Url.AbsoluteUri, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            return;
        }

        if (FileUpload1.HasFile)
        {
            try
            {
                string setup = txtImportSetup.Value;
                var res1 = DbService.DeleteSetupList(setup);

                if (!res1)
                { 
                    Session["AlertMessage"] = "Erro ao encontrar Setup, verifique se o Setup existe";
                    Response.Redirect(Request.Url.AbsoluteUri, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }

                using (var stream = FileUpload1.PostedFile.InputStream)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial depending on your usage

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                        DataTable dt = new DataTable();

                        // Add columns to DataTable (only first three columns)
                        for (int col = 1; col <= 3; col++)
                        {
                            dt.Columns.Add(worksheet.Cells[1, col].Text);
                        }

                        // Add rows to DataTable
                        for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            bool rowHasData = false;
                            DataRow row = dt.NewRow();

                            for (int col = 1; col <= 3; col++)
                            {
                                var cell = worksheet.Cells[rowNum, col];
                                if (string.IsNullOrWhiteSpace(cell.Text))
                                {
                                    break;
                                }
                                row[col - 1] = cell.Text;
                                rowHasData = true;
                            }

                            if (rowHasData)
                            {
                                dt.Rows.Add(row);
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Now you have your data in the DataTable (dt)
                        // Saving to database
                        foreach (DataRow row in dt.Rows)
                        {
                            var pn = row[0];
                            var designation = row[1];
                            var qty = row[2];

                            var res2 = DbService.InsertSetupList(setup, pn.ToString(), designation.ToString(), Int32.Parse(qty.ToString()));
                        }
                    }
                }

                // Notify user of success
                Session["AlertMessage"] = "Ficheiro Importado com Sucesso"; 
                FileUpload1.PostedFile.InputStream.Dispose();
            }
            catch (Exception ex)
            {
                // Handle the error
                Session["AlertMessage"] = "Erro a importar o ficheiro: " + ex.Message;
            }
        }
        else
        {
            // Notify user to select a file
            Session["AlertMessage"] = "Por favor selecione um ficheiro para importar";
        }

        // Redirect to the same page to clear the POST data
        Response.Redirect(Request.Url.AbsoluteUri, false);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
     
    protected void LinkButton_Click(object sender, EventArgs e)
    { 
        RefreshData();
        BindData();

        string[] args = hiddenField.Value.Split(',');

        string designation = args[0];
        string project = args[1];
        string line = args[2];
         
        SetupList = DbService.GetSetupsList(designation);
        SelectedSetup = designation;

        SetupListRepeater.DataSource = SetupList;
        SetupListRepeater.DataBind();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "showSetupList", " $('#setupListModal').modal('show'); ", true);
    } 
}