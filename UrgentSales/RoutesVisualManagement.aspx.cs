using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoutesVisualManagement : System.Web.UI.Page
{ 

    public List<RouteBreak> SelectedRouteBreaks
    {
        get
        {
            if(ViewState["SelectedRouteBreaks"] == null) 
            {
                ViewState["SelectedRouteBreaks"] = new List<RouteBreak>(); 
            }

            return (List<RouteBreak>)ViewState["SelectedRouteBreaks"];
        }
        set
        {
            // Calculate Ids
            int id = 0;
            foreach(RouteBreak routeBreak in value)
            {
                routeBreak.Id = id;
                id++;
            }

            ViewState["SelectedRouteBreaks"] = value;
        }
    }


    public List<RouteBreak> SelectedUpdatingRouteBreaks
    {
        get
        {
            if (ViewState["SelectedUpdatingRouteBreaks"] == null)
            {
                ViewState["SelectedUpdatingRouteBreaks"] = new List<RouteBreak>();
            }

            return (List<RouteBreak>)ViewState["SelectedUpdatingRouteBreaks"];
        }
        set
        {
            // Calculate Ids
            int id = 0;
            foreach (RouteBreak routeBreak in value)
            {
                routeBreak.Id = id;
                id++;
            }

            ViewState["SelectedUpdatingRouteBreaks"] = value;
        }

    }

    public DateTime StartDayDate
    {
        get
        { 
            return (DateTime)ViewState["StartDayDate"];
        }
        set
        {
            ViewState["StartDayDate"] = value;
        }
    }

    public DateTime EndDayDate
    {
        get
        {
            return (DateTime)ViewState["EndDayDate"];
        }
        set
        {
            ViewState["EndDayDate"] = value;
        }
    }

    public DateTime CurrentDayDate
    {
        get
        {
            return (DateTime)ViewState["CurrentDayDate"];
        }
        set
        {
            ViewState["CurrentDayDate"] = value;
        }
    }

    public double CurrentDayTimePercentage
    {
        get
        {
            if(ViewState["CurrentDayTimePercentage"] == null) { ViewState["CurrentDayTimePercentage"] = (double)0; }
            return (double)ViewState["CurrentDayTimePercentage"];
        }
        set
        {
            ViewState["CurrentDayTimePercentage"] = value;
        }
    }
    public string SelectedBuildingValue
    {
        get
        {
            if (Session["SelectedBuildingValue"] == null) { Session["SelectedBuildingValue"] = ""; }
            return (string)Session["SelectedBuildingValue"];
        }
        set
        {
            Session["SelectedBuildingValue"] = value;
        }
    }

    public string SelectedShiftValue
    {
        get
        {
            if (Session["SelectedShiftValue"] == null) { Session["SelectedShiftValue"] = "none"; }
            return (string)Session["SelectedShiftValue"];
        }
        set
        {
            Session["SelectedShiftValue"] = value;
        }
    }

    public string DayHeightStyleFlex 
    {
        get
        {
            return (string)ViewState["DayHeightStyleFlex"];
        }
        set
        {
            ViewState["DayHeightStyleFlex"] = value;
        }
    }

    public int SelectedRouteRegisterId
    {
        get
        {
            return (int)ViewState["SelectedRouteRegisterId"];
        }
        set
        {
            ViewState["SelectedRouteRegisterId"] = value;
        }
    }

    public int SelectedRouteId
    {
        get
        {
            return (int)ViewState["SelectedRouteId"];
        }
        set
        {
            ViewState["SelectedRouteId"] = value;
        }
    }

    public Route SelectedRoute
    {
        get
        {
            if (ViewState["SelectedRoute"] == null) { ViewState["SelectedRoute"] = new Route(); }
            return (Route)ViewState["SelectedRoute"];
        }
        set
        {
            ViewState["SelectedRouteId"] = value;
        }
    }



    private DatabaseService DbService = new DatabaseService();


    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        { 
            selectedBuilding.Value = SelectedBuildingValue;
            selectedMode.Value = SelectedShiftValue;

            RefreshData();
            SelectedRouteBreaks = new List<RouteBreak>();
            SelectedUpdatingRouteBreaks = new List<RouteBreak>();
        } 
    }

    private void RefreshData()
    {
        // Get todays dates
        StartDayDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
        EndDayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        CurrentDayDate = DateTime.Now;

        // Get routes
        var routes = DbService.GetRoutes(selectedBuilding.Value);
        var allRoutes = DbService.GetRoutes("");

        // Bind routes to delete
        SlcRouteToDelete.Items.Clear();
        ListItem empty = new ListItem("Nenhuma rota selecionada", "0");
        SlcRouteToDelete.Items.Add(empty);

        foreach (Route route in allRoutes)
        {
            // Create a ListItem and set its value and text
            ListItem listItem = new ListItem(route.Name + " > " + route.Building, route.Id.ToString());
            SlcRouteToDelete.Items.Add(listItem);
        }

        // Get registers and breaks
        foreach (var route in routes)
        {
            route.Registers = DbService.GetRouteRegisters(route.Id);
            route.Breaks = DbService.GetRouteBreaks(route.Id);
        }

        // Reset start and end day dates
        StartDayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        EndDayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        CurrentDayDate = DateTime.Now;

        // Change end and start dates based on shift mode
        if (selectedMode.Value == "1" || selectedMode.Value == "2" || selectedMode.Value == "3")
        {
           ChangeHoursBasedOnShift((Int32.Parse(selectedMode.Value)));
           PageMainTitle.InnerText = "Gestão de Rotas - Turno " + selectedMode.Value;
        }
        else if (selectedMode.Value == "now")
        {
            int shift = 0;
            DateTime td = DateTime.Now;
            TimeSpan now = DateTime.Now.TimeOfDay;

            // Get shift based on time now
            if (now >= new TimeSpan(6, 0, 0) && now <= new TimeSpan(14, 29, 59)) { shift = 1; }
            else if (now >= new TimeSpan(14, 30, 0) && now <= new TimeSpan(22, 59, 59)) { shift = 2; }
            else { shift = 3; }

            ChangeHoursBasedOnShift(shift);
            PageMainTitle.InnerText = "Gestão de Rotas - Turno Atual" + " - " + shift.ToString();
        }
        else
        {
            PageMainTitle.InnerText = "Gestão de Rotas - 24h";
        }

        // Calculate colors and heights for registers and breaks
        CalculateRouteRegistersHeight(routes);
        CalculateRouteBreaksHeight(routes);

        // Bind routes
        routesRepeater.DataSource = routes;
        routesRepeater.DataBind();

        var startMinutes = CurrentDayDate.Subtract(StartDayDate).TotalMinutes;
        var endMinutes = EndDayDate.Subtract(StartDayDate).TotalMinutes;

        // Get Current time percentage
        CurrentDayTimePercentage = Math.Round(startMinutes / endMinutes, 10);
        DayHeightStyleFlex = "flex:" + CurrentDayTimePercentage.ToString().Replace(",",".") + ";";
    }

    
    protected void routesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs args)
    {
        if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Bind Breaks
            Repeater routeBreaksRepeater = (Repeater)args.Item.FindControl("routeBreaksRepeater");
            routeBreaksRepeater.DataSource = ((Route)args.Item.DataItem).Breaks;
            routeBreaksRepeater.DataBind();

            // Bind registers
            Repeater routeRegistersRepeater = (Repeater)args.Item.FindControl("routeRegistersRepeater");
            routeRegistersRepeater.DataSource = ((Route)args.Item.DataItem).Registers;
            routeRegistersRepeater.DataBind();
        }
    }

    private int ChangeHoursBasedOnShift(int shift)
    {
        DateTime td = DateTime.Now;
        TimeSpan now = DateTime.Now.TimeOfDay;
 
        // Calculate the start and end times of the current shift.
        switch (shift)
        {
            case 1:
                StartDayDate = new DateTime(td.Year, td.Month, td.Day, 6, 0, 0);
                EndDayDate = new DateTime(td.Year, td.Month, td.Day, 14, 29, 59);
                break;
            case 2:
                StartDayDate = new DateTime(td.Year, td.Month, td.Day, 14, 30, 0);
                EndDayDate = new DateTime(td.Year, td.Month, td.Day, 22, 59, 59);
                break;
            case 3:
                // For the third shift, check if it's before or after midnight to adjust the date accordingly.
                if (now > new TimeSpan(22, 59, 59))
                {
                    StartDayDate = new DateTime(td.Year, td.Month, td.Day, 23, 0, 0);
                    EndDayDate = new DateTime(td.Year, td.Month, td.Day + 1, 05, 59, 59);
                }
                else
                {
                    StartDayDate = new DateTime(td.Year, td.Month, td.Day - 1, 23, 0, 0);
                    EndDayDate = new DateTime(td.Year, td.Month, td.Day, 05, 59, 59);
                }
                break;
        }

        return shift;
    }

    private void CalculateRouteBreaksHeight(List<Route> routes)
    {
        foreach(Route route in routes)
        {
            DateTime lastBreakDate = StartDayDate;

            foreach (RouteBreak routeBreak in route.Breaks)
            { 
                DateTime breakStart, breakEnd;
                string styleChanges = "";

                // Check if its for filling space
                if (routeBreak.IsBreakFill)
                {
                    styleChanges = "opacity:0;";
                    breakStart = lastBreakDate;
                    breakEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, routeBreak.TimeEnd.Hours, routeBreak.TimeEnd.Minutes, 0);
                }
                else
                {
                    styleChanges = "pointer-events: auto;";
                    breakStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, routeBreak.TimeStart.Hours, routeBreak.TimeStart.Minutes, 0);
                    breakEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, routeBreak.TimeEnd.Hours, routeBreak.TimeEnd.Minutes, 0);
                }

                // Check if break inside the time
                if (breakStart < StartDayDate || breakEnd > EndDayDate || breakEnd < StartDayDate)
                {
                    routeBreak.BreakStyle = "0; display:none;";
                    continue;
                }

                var minutesStartToBreak = breakStart.Subtract(StartDayDate).TotalMinutes;
                var minutesStartToBreakEnd = breakEnd.Subtract(StartDayDate).TotalMinutes;
                var minutesStartToEnd = EndDayDate.Subtract(StartDayDate).TotalMinutes;

                // Check the percentage till start and end 
                double percentageBreakStart = Math.Round(minutesStartToBreak / minutesStartToEnd, 10);
                double percentageBreakEnd = Math.Round(minutesStartToBreakEnd / minutesStartToEnd, 10);

                // Calculate the total percentage of break height
                double actualBreakPercentage = Math.Round((minutesStartToBreakEnd - minutesStartToBreak) / minutesStartToEnd,10);

                // Get Style Changes based On type of break
                if (!routeBreak.IsStoppedLineBreak)
                {
                    styleChanges += "background-color: #348ceb;";
                    routeBreak.BreakType = "Pausa";
                }
                else 
                { 
                    styleChanges += "background-color: #ff9e59;";
                    routeBreak.BreakType = "Paragem de Linha";
                }
                // Add height in proportion and color based on time
                routeBreak.BreakStyle = actualBreakPercentage.ToString().Replace(",", ".") + ";" + styleChanges;

                lastBreakDate = breakEnd;
            }
        }
    }
    private void CalculateRouteRegistersHeight(List<Route> routes)
    {         
        foreach (Route route in routes)
        {
            double lastRegisterPercentage = 0;
            DateTime lastRegisterDate = StartDayDate;

            foreach (RouteRegister register in route.Registers)
            {  
                if (StartDayDate > register.TimeDate || EndDayDate < register.TimeDate)
                {
                    register.RouteStyle = "0; display:none;";
                    continue;
                }

                var minutesStartToRegister = register.TimeDate.Subtract(StartDayDate).TotalMinutes;
                var minutesStartToNow = CurrentDayDate.Subtract(StartDayDate).TotalMinutes;
                var minutesStartToEnd = EndDayDate.Subtract(StartDayDate).TotalMinutes;

                // If current time is larger than the end date  set end date as max
                if(minutesStartToNow > minutesStartToEnd)
                {
                    minutesStartToNow = minutesStartToEnd;
                }

                double percentageRegister = Math.Round(minutesStartToRegister / minutesStartToEnd, 10);
                double percentageCurrentTime = Math.Round(minutesStartToNow/ minutesStartToEnd, 10);

                // Calculate percentage of the height inside the current time div because its always >= than register time
                double heightInsideCurrentTime = (percentageRegister - lastRegisterPercentage) / percentageCurrentTime;
                 
                lastRegisterPercentage = (percentageRegister - lastRegisterPercentage) + lastRegisterPercentage;

                // Default red  styling
                string background = "background-color:Transparent;";
                register.RegisterStyle = "background-color:#f05151;";
         
                // Green styling if met standards
                var minutesLastToRegister = register.TimeDate.Subtract(lastRegisterDate).TotalMinutes;
                
                // Check if met Green standard of time
                if (register.TimePassed <= route.Minutes)
                { 
                    background = "background-color:Transparent;pointer-events:none;";
                    register.RegisterStyle = "background-color:#30d179;";
                }

                // If is break 
                if (register.IsBreak)
                {
                    background = "background-color:Transparent;pointer-events:none;";
                    register.RegisterStyle = "background-color:#8c8c8c;opacity:0;";
                }

                // Calculate Registers postion relative to average time
                int routeMinutes = route.Minutes * 2;
                int minutesPassed = register.TimePassed;
                double percentagePositioning = Math.Round((double)minutesPassed / routeMinutes, 10);

                // Reverse 
                percentagePositioning = 1 - percentagePositioning;
                register.RegisterPostionStyle = "flex:" + percentagePositioning.ToString().Replace(",", ".") + ";";

                lastRegisterDate = register.TimeDate;
                // Add height in proportion and color based on time
                register.RouteStyle = heightInsideCurrentTime.ToString().Replace(",", ".") + ";" + background;
               
            }
        }
    }

   

    protected void Btn_InsertRoute_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }
        UpdateValuesFromSelectedRouteBreaks();

        // Check for Invalid Route Breaks
        foreach (RouteBreak brk in SelectedRouteBreaks)
        {
            if (brk.TimeStart.TotalMinutes > brk.TimeEnd.TotalMinutes)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro! Tempos de pausa inválidos');", true);

                return;
            }
        }


        int res = 0;
        if(slcInsertNewRouteBuilding.Value.Trim() == "" || slcInsertNewRouteBuilding.Value.Trim() == ""  || !Int32.TryParse(txtInsertNewRouteMin.Value, out res))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, verifique se tem permissões, ou erros técnicos');", true);
        }
        else
        {
            int id = DbService.InsertRoute(slcInsertNewRouteBuilding.Value, txtInsertNewRouteName.Value, Int32.Parse(txtInsertNewRouteMin.Value));

            UpdateValuesFromSelectedRouteBreaks();
            foreach(RouteBreak brk in SelectedRouteBreaks)
            {
                DbService.InsertRouteBreak(brk.TimeStart, brk.TimeEnd, id, brk.IsStoppedLineBreak);
            }
        }
        RefreshPage();
         

    }

    private void RefreshPage()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "refreshPage", "location.reload();", true);  
    }

    protected void JustificationInsert_Click(object sender, EventArgs e)
    {
        if(Int32.Parse(justificationInsertValue.Value) == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, Selecione uma justificação');", true);

            RefreshPage();
            return;
        }

        var res = DbService.UpdateRegisterJustification(SelectedRouteRegisterId, Int32.Parse(justificationInsertValue.Value));
        RefreshPage();
    }

    protected void DeleteRoute_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }

        if (Int32.Parse(SlcRouteToDelete.Value) == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro!, Selecione uma rota para eliminar');", true);

            RefreshPage();
            return;
        }

        var res = DbService.DeleteRoute(Int32.Parse(SlcRouteToDelete.Value));
        RefreshPage();
    }

    protected void UpdateTimer_Tick(object sender, EventArgs e)
    {
        RefreshData(); 
    }

    protected void triggerJustificationSelectChange_Click(object sender, EventArgs e)
    {

    }


  
    protected void ConfirmFilterButton_Click(object sender, EventArgs e)
    {
        SelectedBuildingValue = selectedBuilding.Value;
        SelectedShiftValue = selectedMode.Value;

        RefreshData();
    }

    protected void justificationButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender; 
        SelectedRouteRegisterId = Int32.Parse(hiddenField.Value);
        int routeRegisterId = Int32.Parse(hiddenField.Value);
        string justification = DbService.GetRouteJustification(routeRegisterId).ToString();
        justificationInsertValue.Value = justification;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "showJustificationInsertModal", " $('#justificationInsertModal').modal('show'); ", true);
    }

    protected void RouteSettings_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;

        SelectedRouteId = Int32.Parse(button.CommandArgument);

        Route selectedRoute = DbService.GetRoute(SelectedRouteId);

        updateRouteModalLabel.InnerText = "Alterar Rota " + selectedRoute.Name;
        SelectedRouteBuilding.Value = selectedRoute.Building;
        SelectedRouteName.Value = selectedRoute.Name;
        SelectedRouteMinutes.Value = selectedRoute.Minutes.ToString();

        List<RouteBreak> breaks = DbService.GetRouteBreaks(SelectedRouteId);
        SelectedUpdatingRouteBreaks = new List<RouteBreak>();

        foreach (RouteBreak brk in breaks)
        {
            if (!brk.IsBreakFill)
            {
                SelectedUpdatingRouteBreaks.Add(brk);
            }
        }
        SelectedUpdatingRouteBreaks = SelectedUpdatingRouteBreaks;
        DataBindSelectedRouteBreaksUpdating();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "updateRouteModalShow", " $('#updateRouteModal').modal('show'); ", true);
    }

    protected void NewBreakButton_Click(object sender, EventArgs e)
    {
        List<RouteBreak> breaks = new List<RouteBreak>();

        foreach(RouteBreak brk in SelectedRouteBreaks)
        {
            breaks.Add(brk);
        }
        breaks.Add(new RouteBreak() { TimeStart = TimeSpan.Zero, TimeEnd = TimeSpan.Zero, IsStoppedLineBreak = false });

        SelectedRouteBreaks = breaks;
        UpdateValuesFromSelectedRouteBreaks();
        DataBindSelectedRouteBreaks();
    }


    protected void AddRoutesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
     // This method is called when the Repeater is binding
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var routeBreak = (RouteBreak)e.Item.DataItem;

            var timeStartInput = (HtmlInputGenericControl)e.Item.FindControl("TimeStart");
            var timeEndInput = (HtmlInputGenericControl)e.Item.FindControl("TimeEnd");
            var isLineStopped = (HtmlSelect)e.Item.FindControl("BreakStoppedLine");

            // Set the values of the input fields based on the RouteBreak object
            if (routeBreak != null)
            {
                timeStartInput.Value = routeBreak.TimeStart.ToString(@"hh\:mm");
                timeEndInput.Value = routeBreak.TimeEnd.ToString(@"hh\:mm");

                int value = 0;
                if (routeBreak.IsStoppedLineBreak) { value = 1; }

                isLineStopped.Value = value.ToString();
            }
        }
    }

    private void DataBindSelectedRouteBreaks()
    {
        AddRoutesRepeater.DataSource = SelectedRouteBreaks;
        AddRoutesRepeater.DataBind();
    }

    private void DataBindSelectedRouteBreaksUpdating()
    {
        updateRoutesRepeater.DataSource = SelectedUpdatingRouteBreaks;
        updateRoutesRepeater.DataBind();
    }

    private void UpdateValuesFromSelectedRouteBreaks()
    {
        // Loop through each item in the Repeater and capture the updated values
        foreach (RepeaterItem item in AddRoutesRepeater.Items)
        {
            var timeStartInput = (HtmlInputGenericControl)item.FindControl("TimeStart");
            var timeEndInput = (HtmlInputGenericControl)item.FindControl("TimeEnd"); 
            var isLineStopped = (HtmlSelect)item.FindControl("BreakStoppedLine");

            if (timeStartInput != null && timeEndInput != null)
            {
                // Find the corresponding RouteBreak object from the SelectedRouteBreaks list
                var routeBreak = SelectedRouteBreaks[item.ItemIndex];

                // Update the RouteBreak object with the new values
                routeBreak.TimeStart = TimeSpan.Parse(timeStartInput.Value);
                routeBreak.TimeEnd = TimeSpan.Parse(timeEndInput.Value);
                routeBreak.IsStoppedLineBreak = (isLineStopped.Value == "1"); 
            }
        }
    }


    private void UpdateValuesFromSelectedRouteBreaksUpdating()
    {
        // Loop through each item in the Repeater and capture the updated values
        foreach (RepeaterItem item in updateRoutesRepeater.Items)
        {
            var timeStartInput = (HtmlInputGenericControl)item.FindControl("TimeStart");
            var timeEndInput = (HtmlInputGenericControl)item.FindControl("TimeEnd");
            var isLineStopped = (HtmlSelect)item.FindControl("UpdatingBreakStoppedLine");

            if (timeStartInput != null && timeEndInput != null)
            {
                // Find the corresponding RouteBreak object from the SelectedRouteBreaks list
                var routeBreak = SelectedUpdatingRouteBreaks[item.ItemIndex];

                // Update the RouteBreak object with the new values
                routeBreak.TimeStart = TimeSpan.Parse(timeStartInput.Value);
                routeBreak.TimeEnd = TimeSpan.Parse(timeEndInput.Value);
                routeBreak.IsStoppedLineBreak = (isLineStopped.Value == "1");
            }
        }
    }


    protected void RemoveBreakButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int id = Int32.Parse(button.CommandArgument);
        UpdateValuesFromSelectedRouteBreaks();

        SelectedRouteBreaks.Remove(SelectedRouteBreaks.Where(x => x.Id == id).First());

        SelectedRouteBreaks = SelectedRouteBreaks;
        DataBindSelectedRouteBreaks();
    }

    protected void updateRoutesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // This method is called when the Repeater is binding
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var routeBreak = (RouteBreak)e.Item.DataItem;

            var timeStartInput = (HtmlInputGenericControl)e.Item.FindControl("TimeStart");
            var timeEndInput = (HtmlInputGenericControl)e.Item.FindControl("TimeEnd");
            var isLineStopped = (HtmlSelect)e.Item.FindControl("UpdatingBreakStoppedLine");

            // Set the values of the input fields based on the RouteBreak object
            if (routeBreak != null)
            {
                timeStartInput.Value = routeBreak.TimeStart.ToString(@"hh\:mm");
                timeEndInput.Value = routeBreak.TimeEnd.ToString(@"hh\:mm");

                int value = 0;
                if (routeBreak.IsStoppedLineBreak) { value = 1; }

                isLineStopped.Value = value.ToString();
            }
        }
    }

    protected void RemoveUpdateBreakButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int id = Int32.Parse(button.CommandArgument);
        UpdateValuesFromSelectedRouteBreaksUpdating();

        SelectedUpdatingRouteBreaks.Remove(SelectedUpdatingRouteBreaks.Where(x => x.Id == id).First());

        SelectedUpdatingRouteBreaks = SelectedUpdatingRouteBreaks;
        DataBindSelectedRouteBreaksUpdating();
    }

    protected void UpdateNewRoutesButton_Click(object sender, EventArgs e)
    {
        List<RouteBreak> breaks = new List<RouteBreak>();

        foreach (RouteBreak brk in SelectedUpdatingRouteBreaks)
        {
            breaks.Add(brk);
        }

        var time = Math.Round(DateTime.Now.TimeOfDay.TotalHours,0);
        breaks.Add(new RouteBreak() { 
            TimeStart = new TimeSpan((int)time, 0,0),
            TimeEnd = new TimeSpan((int)time, 0, 0)
        });

        SelectedUpdatingRouteBreaks = breaks;
        UpdateValuesFromSelectedRouteBreaksUpdating();
        DataBindSelectedRouteBreaksUpdating();
    }

    protected void Btn_UpdateRoute_Click(object sender, EventArgs e)
    {
        if (!AuthUtils.IsUserLoggedIn())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Precisa de permissões para fazer esta operação');", true);

            RefreshPage();
            return;
        }

        UpdateValuesFromSelectedRouteBreaksUpdating();

        // Check for Invalid Route Breaks
        foreach (RouteBreak brk in SelectedUpdatingRouteBreaks)
        {
            if(brk.TimeStart.TotalMinutes > brk.TimeEnd.TotalMinutes)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Erro! Tempos de pausa inválidos');", true);
                 
                return;
            } 
        } 

        DbService.UpdateRoute(SelectedRouteBuilding.Value, SelectedRouteName.Value, Int32.Parse(SelectedRouteMinutes.Value), SelectedRouteId);

        DbService.DeleteRouteAllBreaks(SelectedRouteId);

        UpdateValuesFromSelectedRouteBreaksUpdating();

        foreach (RouteBreak brk in SelectedUpdatingRouteBreaks)
        {
            DbService.InsertRouteBreak(brk.TimeStart, brk.TimeEnd, SelectedRouteId, brk.IsStoppedLineBreak);
        }

        RefreshPage();
    }

}