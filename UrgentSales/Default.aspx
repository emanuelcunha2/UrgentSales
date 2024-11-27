<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .selectedButtonStyle {
            background-color: white; 
            border-width: 2px; 
            color: #2686c9;
        }
    </style>
     
    <asp:UpdatePanel runat="server" id="UpdatePanel3">
        <ContentTemplate>
            <asp:Timer runat="server" id="UpdateTimer" Interval="20000" OnTick="UpdateTimer_Tick"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <div class="jumbotron">
            <h1>  Vendas Urgentes &#x2022; <%: selectedBuilding %></h1>
            <p>Last SAP Pull: <b style="color:#3470ba"><%: sapDate %></b></p> 
        </div>

        <asp:Button class="btn btn-primary" ID="VS1" runat="server" Text="ALL" OnClick="Button_Click" />
        <asp:Button class="btn btn-primary mx-1" ID="VS2" runat="server" Text="VS1" OnClick="Button_Click" />
        <asp:Button class="btn btn-primary" ID="ALL" runat="server" Text="VS2" OnClick="Button_Click" />
    </ContentTemplate>
    </asp:UpdatePanel>
        
    <hr />

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
               <table class="table table-bordered mt-3" style="border-color:#cccccc">
                   <thead>
                       <tr style="background-color:#f5f5f5;">
                           <th>Edíficio</th>
                           <th>Part Number</th>
                           <th>Designação</th>
                           <th> <%: selectedDeposits %> </th>
                           <th> <%: selectedProdDeposits %> </th>
                           <th>Gap</th>
                           <th>Shipments</th>
                       </tr>
                   </thead>
                   <tbody>

                       <asp:Repeater ID="rptRecords" runat="server" EnableViewState="false">
                           <ItemTemplate>
                               <tr>
                                   <td><%# Eval("Building") %></td>
                                   <td><%# Eval("PartNumber") %></td>
                                   <td><%# Eval("Designation") %></td>
                                   <td><%# Eval("QuantityAvailable") %></td> 
                                   <td><%# Eval("ProdQuantityAvailable") %></td>
                                   <td style="color:red;"><%# Eval("QuantityGap") %></td>
                                   <td><%# Eval("StringRecords") %></td>
                               </tr>
                           </ItemTemplate>
                       </asp:Repeater>
                   </tbody>
               </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
