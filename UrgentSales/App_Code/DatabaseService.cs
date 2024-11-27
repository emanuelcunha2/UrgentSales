using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Providers.Entities;

/// <summary>
/// Summary description for DatabaseService
/// </summary>
public class DatabaseService
{
    private readonly string connectionString = "Data Source=130.171.191.142;Initial Catalog=FIS;User Id=program_user;Password=praga;Persist Security Info=True;TrustServerCertificate=True;Connection Timeout=10";

    public SqlConnection GetSqlConnection()
    {
        return new SqlConnection(connectionString);
    }

    public string GetLastDateSAP()
    {
        string lastDate = "";
        SqlConnection sqlConnection = GetSqlConnection();
        try
        {
            sqlConnection.Open();

            string queryString = "SELECT TOP(1) * FROM [FIS].[dbo].[tbStocks]";
            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DateTime date = TryGetDateTime(reader["sap_date"]) ?? DateTime.MinValue;
                lastDate = date.ToString();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return lastDate;
    }

    public List<IncompleteBox> GetIncompleteBoxes()
    {
        List<IncompleteBox> incompleteBoxes = new List<IncompleteBox>();
        SqlConnection sqlConnection = GetSqlConnection(); 
        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[SelectIncompleteBoxes]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int actualQty = TryGetInt(reader["actualQty"]) ?? 0;
                int wantedQty = TryGetInt(reader["wantedQty"]) ?? 0;
                string serial = TryGetString(reader["serial"]) ?? "";
                string building = TryGetString(reader["location"]) ?? "";
                string material = TryGetString(reader["material"]) ?? "";
                string designation = TryGetString(reader["designation"]) ?? "";

                if (actualQty < wantedQty)
                {
                    incompleteBoxes.Add(new IncompleteBox()
                    {
                        Serial = serial,
                        QuantityDiference = actualQty + " / " + wantedQty,
                        Building = building,
                        PartNumber = material,
                        Designation = designation
                    });
                }
            }

            incompleteBoxes = incompleteBoxes.OrderBy(x => x.Building).ToList();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }
        return incompleteBoxes;
    }

    public List<GapRecord> GetTodayShipmentGapRecords(string selectedBuilding)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<GapRecord> shipmentRecords = new List<GapRecord>();
        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[SelectTodayShippingGapRecords]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int qty3333 = TryGetInt(reader["qty3333"]) ?? 0;
                int qty2333 = TryGetInt(reader["qty2333"]) ?? 0;
                int qty3332 = TryGetInt(reader["qty3332"]) ?? 0;
                int qty2332 = TryGetInt(reader["qty2332"]) ?? 0;
                int qty0030 = TryGetInt(reader["qty0030"]) ?? 0;
                int qtyNeed = TryGetInt(reader["qtyExpected"]) ?? 0;
                string building = TryGetString(reader["location"]) ?? "";
                building = building == "VS1" ? "VS1" : building == "VS2" ? "VS2" : building;
                GapRecord rc = new GapRecord()
                {
                    PartNumber = TryGetString(reader["apn"]) ?? "",
                    Designation = TryGetString(reader["designation"]) ?? "",
                    Building = building,
                    QuantityAvailable = qty2333 + qty3333,
                    ProdQuantityAvailable = qty2332 + qty3332,
                    QuantityGap = qty0030 - qtyNeed,
                    StringRecords = TryGetString(reader["GroupedTimes"]) ?? "",
                };

                if (rc.QuantityGap < 0)
                {
                    if (selectedBuilding != "ALL")
                    {
                        if (selectedBuilding == building) { shipmentRecords.Add(rc); }
                    }
                    else
                    {
                        shipmentRecords.Add(rc);
                    }
                }
            }
            shipmentRecords = shipmentRecords.OrderBy(x => x.Building).ToList();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error: " + ex.ToString());                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return shipmentRecords;
    }


    public List<Setup> GetSetups(string status)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<Setup> setups = new List<Setup>();
        try
        {

            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[SelectSetupsByStatus]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            // Add the varchar(40) parameter
            SqlParameter statusParameter = new SqlParameter("@Status", SqlDbType.VarChar, 40);
            statusParameter.Value = status;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Setup setup = new Setup();

                setup.Id = Int32.Parse(reader["id"].ToString());
                setup.Designation = reader["setup"].ToString();
                setup.Project = reader["project"].ToString();
                setup.Line = reader["line"].ToString();
                setup.IsChecked = false;
                
                if(reader["priority"] == DBNull.Value)
                {
                    setup.Priority = 0;
                }
                else { setup.Priority = Int32.Parse(reader["priority"].ToString()); }

                setups.Add(setup);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return setups;
    }



    public int GetRouteJustification(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<Setup> setups = new List<Setup>();
        try
        {

            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_SelectRegisterJustification]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            // Add the varchar(40) parameter
            SqlParameter statusParameter = new SqlParameter("@registerId", SqlDbType.Int);
            statusParameter.Value = id;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["justification_id"] == DBNull.Value) { return 0; }
                else { return (int)reader["justification_id"]; }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return 0;
    }


    public List<IncompleteBoxesRequest> GetIncompleteBoxesRequests()
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<IncompleteBoxesRequest> incReqs = new List<IncompleteBoxesRequest>();
        try
        {

            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[CIS_SelectOpenIncompleteBoxRequests]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
             
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                IncompleteBoxesRequest req = new IncompleteBoxesRequest();

                req.Id = Int32.Parse(reader["id"].ToString());
                req.PartNumber = reader["partnumber"].ToString();
                req.SoldDate = DateTime.Parse(reader["soldDate"].ToString());
                req.RequestDate = DateTime.Parse(reader["reqDate"].ToString());
                req.SendDate = DateTime.Parse(reader["sendDate"].ToString());
                req.Comment = reader["comments"].ToString();
                req.Quantity = Int32.Parse(reader["quantity"].ToString());
                req.Designation = reader["description"].ToString();
                req.AvailableQuantity = TryGetInt(reader["qty3"]) ?? 0;
                req.ProdQuantity = TryGetInt(reader["qty2"]) ?? 0;
                req.Building = reader["location"].ToString();
                req.Locations = reader["QuantityPerLocation"].ToString();

                // Replace comment format to show in html
                req.Comment = FormatTextToHtml(req.Comment);
                req.Locations = FormatTextToHtml(req.Locations);

                incReqs.Add(req); 
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return incReqs;
    }

    public string FormatTextToHtml(string text)
    {

        string htmlBreakElement = "<br />";
        text = text.Replace("\n",htmlBreakElement);
        return text;
    }
    public List<Setup> GetSetupsList(string setup)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<Setup> setups = new List<Setup>();
        try
        {

            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[SelectSetupList]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            // Add the varchar(40) parameter
            SqlParameter statusParameter = new SqlParameter("@setup", SqlDbType.VarChar, 40);
            statusParameter.Value = setup;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Setup set = new Setup();
                
                set.Pn = reader["pn"].ToString();
                set.PnDescription = reader["designation"].ToString();
                set.Qty = Int32.Parse(reader["qty"].ToString());  
                setups.Add(set);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return setups;
    }



    public List<Setup> UpdateSetup(string status, int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<Setup> setups = new List<Setup>();
        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[UpdateSetupsStatus]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            // Add the varchar(40) parameter
            SqlParameter statusParameter = new SqlParameter("@Status", SqlDbType.VarChar, 40);
            SqlParameter statusParameter2 = new SqlParameter("@id", SqlDbType.Int);
            statusParameter.Value = status;
            statusParameter2.Value = id;

            command.Parameters.Add(statusParameter);
            command.Parameters.Add(statusParameter2);

            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return setups;
    }

    public bool InsertSetup(string designation, string project, string line)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[InsertNewSetup]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar, 40) { Value = designation });
            command.Parameters.Add(new SqlParameter("@project", SqlDbType.VarChar, 40) { Value = project });
            command.Parameters.Add(new SqlParameter("@line", SqlDbType.VarChar, 40) { Value = line });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    public bool InsertIncompleteBoxRequest(string partnum, int quantity, string comments,DateTime date, string user)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[CIS_InsertNewRequest]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@partNumber", SqlDbType.VarChar, 13) { Value = partnum });
            command.Parameters.Add(new SqlParameter("@quantity", SqlDbType.Int) { Value = quantity });
            command.Parameters.Add(new SqlParameter("@comments", SqlDbType.VarChar, 100) { Value = comments });
            command.Parameters.Add(new SqlParameter("@sendDate", SqlDbType.DateTime) { Value = date });
            command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 10) { Value = user });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result != 0)
            { 
                return false; // Indicate failure  
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool SetupUpdate(string designation, string project, string line, int priority)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[UpdateSetup]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar, 40) { Value = designation });
            command.Parameters.Add(new SqlParameter("@project", SqlDbType.VarChar, 40) { Value = project });
            command.Parameters.Add(new SqlParameter("@line", SqlDbType.VarChar, 40) { Value = line });
            command.Parameters.Add(new SqlParameter("@priority", SqlDbType.Int) { Value = priority });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool IncompleBoxUpdateCompleted(int id, string user)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[CIS_UpdateIncBoxRequestCompleted]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
             
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar,10) { Value = user });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result != 0)
            {
                Console.WriteLine("id doesnt exist.");
                return false; // Indicate failure
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool IncompleBoxReqDelete(int id, string user)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[CIS_DeleteIncBoxRequest]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar, 10) { Value = user });


            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result != 0)
            {
                Console.WriteLine("id doesnt exist.");
                return false; // Indicate failure
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool IncompleBoxReqComment(int id, string comment)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[CIS_CommentIncBoxRequest]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@comment", SqlDbType.VarChar, 100) { Value = comment });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("id doesnt exist.");
                return false; // Indicate failure
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    public bool DeleteRoute(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_DeleteRoute]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("id doesnt exist.");
                return false; // Indicate failure
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool UpdateRegisterJustification(int id, int justificationId)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_UpdateRouteJustification]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@registerId", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@justification", SqlDbType.Int) { Value = justificationId });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("id doesnt exist.");
                return false; // Indicate failure
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }






    public bool InsertSetupList(string setup, string pn, string designation, int qty)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[InsertNewSetup_List]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@setup", SqlDbType.VarChar, 40) { Value = setup });
            command.Parameters.Add(new SqlParameter("@pn", SqlDbType.VarChar, 40) { Value = pn });
            command.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar, 40) { Value = designation });
            command.Parameters.Add(new SqlParameter("@qty", SqlDbType.Int, 40) { Value = qty });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the designation doesn't exist.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }





    public bool DeleteSetup(string designation)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[DeleteSetup]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar, 40) { Value = designation });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the designation doesn't exist.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }


    public bool DeleteSetupList(string designation)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[DeleteListSetup]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@designation", SqlDbType.VarChar, 40) { Value = designation });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the designation doesn't exist.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }







    public List<Route> GetRoutes(string building)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<Route> routes = new List<Route>();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_SelectRoutes]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
             
            SqlParameter statusParameter = new SqlParameter("@building", SqlDbType.VarChar, 10);
            statusParameter.Value = building;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            { 
                Route route = new Route();
                route.Id = (int)reader["id"];
                route.Date = DateTime.Now;
                route.Name = reader["name"].ToString();
                route.Building = reader["building"].ToString();
                route.Minutes = (int)reader["minutes"];

                routes.Add(route);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return routes;
    }

    public List<RouteRegister> GetRouteRegisters(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<RouteRegister> routeRegisters = new List<RouteRegister>();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_SelectRouteRegisters]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter statusParameter = new SqlParameter("@id", SqlDbType.Int);
            statusParameter.Value = id;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();
            int count = 0;

            bool lastRecordBreak = false;
            int lastRecordMin = 0;
            int accomulatedBreakMin = 0;

            while (reader.Read())
            {
                RouteRegister reg = new RouteRegister();
                reg.RegisterId = (int)reader["id"];
                reg.RouteId = (int)reader["route_id"];
                reg.TimeDate = (DateTime)reader["date"];
                reg.Justification = reader["justification"].ToString();
                reg.TimePassed = (int)reader["timePassed"];

                // Check if last record was break to calculate time between breaks
                if(reader["isBreakRecord"] != DBNull.Value) 
                {
                    var x = reader["isBreakRecord"];
                    reg.IsBreak = (bool)x;

                    if(reg.IsBreak && lastRecordBreak)
                    {
                        accomulatedBreakMin += (reg.TimePassed - lastRecordMin);
                        lastRecordMin = reg.TimePassed;
                    }
                    else
                    {
                        lastRecordMin = reg.TimePassed;
                        lastRecordBreak = true;
                    }
                    count++;
                }
                else
                {
                    if(lastRecordBreak) 
                    {
                        reg.TimePassed -= accomulatedBreakMin;
                        lastRecordMin = 0;
                        accomulatedBreakMin = 0;
                        lastRecordBreak = false;
                    } 
                }

                routeRegisters.Add(reg);

            }
            Console.WriteLine("");
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return routeRegisters;
    }


    public Route GetRoute(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        Route route = new Route();
        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_SelectRoute]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter statusParameter = new SqlParameter("@id", SqlDbType.Int);
            statusParameter.Value = id;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                route.Id = (int)reader["id"];
                route.Date = DateTime.Now;
                route.Name = reader["name"].ToString();
                route.Building = reader["building"].ToString();
                route.Minutes = (int)reader["minutes"];

            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return route;
    }




    public List<RouteBreak> GetRouteBreaks(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();
        List<RouteBreak> routeBreaks = new List<RouteBreak>();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_SelectRouteBreaks]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter statusParameter = new SqlParameter("@id", SqlDbType.Int);
            statusParameter.Value = id;
            command.Parameters.Add(statusParameter);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                RouteBreak reg = new RouteBreak();
                RouteBreak fill = new RouteBreak();

                reg.RouteId = (int)reader["route_id"];
                reg.TimeStart = TimeSpan.Parse(reader["timeStart"].ToString());
                reg.TimeEnd = TimeSpan.Parse(reader["timeEnd"].ToString());

                var isLineStopped = (bool)reader["isLineStopped"];
                reg.IsStoppedLineBreak = isLineStopped;

                fill.IsBreakFill = true;
                fill.TimeEnd = reg.TimeStart;

                routeBreaks.Add(fill);
                routeBreaks.Add(reg); 
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        return routeBreaks;
    }



    public int InsertRoute(string building, string name, int minutes)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_InsertNewRoute]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50) { Value = name });
            command.Parameters.Add(new SqlParameter("@building", SqlDbType.VarChar, 10) { Value = building });
            command.Parameters.Add(new SqlParameter("@minutes", SqlDbType.Int) { Value = minutes });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter("@insertedId", SqlDbType.Int);

            returnValue.Direction = ParameterDirection.Output;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return -1; // Indicate failure due to duplicate setup
            }

            return result; // Indicate success
        }   
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return -1; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return -1; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }



    public int UpdateRoute(string building, string name, int minutes, int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_UpdateRoute]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50) { Value = name });
            command.Parameters.Add(new SqlParameter("@building", SqlDbType.VarChar, 10) { Value = building });
            command.Parameters.Add(new SqlParameter("@minutes", SqlDbType.Int) { Value = minutes });
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter("@res", SqlDbType.Int);

            returnValue.Direction = ParameterDirection.Output;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return -1; // Indicate failure due to duplicate setup
            }

            return result; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return -1; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return -1; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }

    }


    public int DeleteRouteAllBreaks(int id)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_DeleteExistingRouteBreaks]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter("@res", SqlDbType.Int);

            returnValue.Direction = ParameterDirection.Output;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return -1; // Indicate failure due to duplicate setup
            }

            return result; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return -1; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return -1; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }

    }




    public bool InsertRouteBreak(TimeSpan start, TimeSpan end, int route_id, bool isLineStopped)
    {
        SqlConnection sqlConnection = GetSqlConnection();

        try
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("[PCL].[dbo].[VMR_InsertNewRouteBreak]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@start", SqlDbType.Time) { Value = start });
            command.Parameters.Add(new SqlParameter("@end", SqlDbType.Time) { Value = end });
            command.Parameters.Add(new SqlParameter("@routeId", SqlDbType.Int) { Value = route_id });
            command.Parameters.Add(new SqlParameter("@isLineStop", SqlDbType.Bit) { Value = isLineStopped });

            // Add a parameter to capture the return value of the stored procedure
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Check the return value from the stored procedure
            int result = (int)returnValue.Value;

            if (result == -1)
            {
                Console.WriteLine("A setup with the same designation already exists.");
                return false; // Indicate failure due to duplicate setup
            }

            return true; // Indicate success
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Error: " + ex.ToString());
            return false; // Indicate failure due to SQL error
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.ToString());
            return false; // Indicate failure due to general error
        }
        finally
        {
            sqlConnection.Close();
        }
    }



    private int? TryGetInt(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return null;
        }
    int intValue;
    if (int.TryParse(value.ToString(), out intValue))
        {
            return intValue;
        }

        return null;
    }
     

    private string TryGetString(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return "";
        }

        return value.ToString();
    }

    private DateTime? TryGetDateTime(object value)
    {
        if (value == null || value == DBNull.Value)
        {
            return null;
        }


        DateTime dateTimeValue;
        if (DateTime.TryParse(value.ToString(), out dateTimeValue))
        {
            return dateTimeValue;
        }

        return null;
    }

}