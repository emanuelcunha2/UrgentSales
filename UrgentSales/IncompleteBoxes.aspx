<%@ Page Title="Caixas Incompletas" MasterPageFile="~/Site.Master"  Language="C#" AutoEventWireup="true" CodeFile="IncompleteBoxes.aspx.cs" Inherits="IncompleteBoxes" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <div class="jumbotron">
            <h1>  Caixas Incompletas </h1>
            <%--<p>Last SAP Pull: <b style="color:#3470ba"></b></p> --%>
        </div>
        
        <table class="table table-bordered mt-3" style="border-color:#cccccc">
            <thead>
                <tr style="background-color:#f5f5f5;">
                    <th>Edificio</th>
                    <th>Serials</th> 
                    <th>Part Number</th>
                    <th>Designação</th>
                    <th>Quantity</th>
                    <th>Locations</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="incompleteBoxesList" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Building") %></td>
                            <td><%# Eval("Serial") %></td>
                            <td><%# Eval("PartNumber") %></td>
                            <td><%# Eval("Designation") %></td>
                            <td><%# Eval("QuantityDiference") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

      


    </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
