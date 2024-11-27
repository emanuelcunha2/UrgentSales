<%@ Page Title="Vendas Urgentes e Caixas Incompletas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Urgent.aspx.cs" Inherits="Urgent" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .selectedButtonStyle {
            background-color: white; 
            border-width: 2px; 
            color: #2686c9;
        }

        h2 {
            margin-bottom:15px;
        }

        .textBoxDate{
            max-width:130px;
        }

        .textBoxPicker{
           max-width:150px;
        }

        .modal-dialog-centered {
            display: flex;
            align-items: center;
            justify-content: center; 
            height: 100vh;
        }

        .modal-container{
           width: 69%; 
        }

        th, td{
          border-top: 1px solid #a8a8a8;
          border-bottom: 1px solid #a8a8a8;

        }
        table{
           border: 1px solid #a8a8a8;
        }
    </style>
     
    <asp:UpdatePanel runat="server" id="UpdatePanel3">
        <ContentTemplate>
            <asp:Timer runat="server" id="UpdateTimer" Interval="300000" OnTick="UpdateTimer_Tick"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <div class="jumbotron">
            <h1>  Vendas Urgentes e Caixas Incompletas &#x2022; <%: selectedBuilding %></h1>
            <p>Last SAP Pull: <b style="color:#3470ba"><%: sapDate %></b></p> 
        </div>

        <asp:Button class="btn btn-primary" ID="VS1" runat="server" Text="ALL" OnClick="Button_Click" />
        <asp:Button class="btn btn-primary mx-1" ID="VS2" runat="server" Text="VS1" OnClick="Button_Click" />
        <asp:Button class="btn btn-primary" ID="ALL" runat="server" Text="VS2" OnClick="Button_Click" />
    </ContentTemplate>
    </asp:UpdatePanel>
        
    <hr />
    <h2>Vendas Urgentes</h2> 

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
               <table class="table mt-3"">
                   <thead>
                       <tr style="background-color:#d6e4ff;">
                           <th>Edíficio</th>
                           <th>Part Number</th>
                           <th>Designação</th>
                           
                           <th> <%: selectedProdDeposits %> </th>
                           <th> <%: selectedDeposits %> </th> 

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
                                   
                                   <td><%# Eval("ProdQuantityAvailable") %></td>
                                   <td><%# Eval("QuantityAvailable") %></td>
                                   
                                   <td style="color:red;"><%# Eval("QuantityGap") %></td>
                                   <td><%# Eval("StringRecords") %></td>
                               </tr>
                           </ItemTemplate>
                       </asp:Repeater>
                   </tbody>
               </table>

        </ContentTemplate>
    </asp:UpdatePanel>
     
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label id="noUrgentSalesLbl" runat="server" style="opacity:40%">
                Sem vendas urgentes
            </asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
 

    <hr />
    <h2>Pedidos Caixas Incompletas</h2>


    <!-- Incomplete Boxes Requests Modals --> 
    <!-- Insert New Box Request -->
    <div class="modal fade" id="insertRequestModal" tabindex="-1" role="dialog" aria-labelledby="setupModalLabel" aria-hidden="true"  >
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content modal-container">
                <div class="modal-header">
                    <h4 class="modal-title" id="setupModalLabel">INSERIR PEDIDO</h4> 
                </div>
                <div class="modal-body">

                    <label>Part Number</label>
                    <input id="txtInsertReqPn" runat="server" class="form-control" placeholder="Introduzir Part Number" EnableViewState="true" />

                    <label style="margin-top:15px;">Quantidade da caixa</label>
                    <input id="txtInsertReqQty" type="number" runat="server" class="form-control" placeholder="Introduzir Quantidade" EnableViewState="true" />

                    <label style="margin-top:15px;">Date de envio</label>
                    <input id="txtInsertReqDate" type="date" runat="server" class="form-control" placeholder="Introduzir Data de Envio" EnableViewState="true" />

                    <label style="margin-top:15px;">Comentários</label>
                    <textarea id="txtInsertReqComment" maxlength="90" style="max-height:80px; min-height:35px" rows="2"  runat="server" class="form-control" placeholder="Introduzir Comentário" EnableViewState="true" />

                </div>
                <div class="modal-footer">
                    <div style="display:flex;flex-direction:row">    
                        <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">FECHAR</button>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>                                    
                                <asp:button runat="server" class="btn btn-primary" Text="INSERIR" OnClick="InsertIncBoxRequest_Click"   />
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div> 
                         
                </div>
            </div>
        </div>
    </div>


    <!-- Confirm Box Changes Modal -->
    <div class="modal fade" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="confirmModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content modal-container">
                <div class="modal-header">
                    <h4 class="modal-title" >Confirmar</h4> 
                </div>

                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <label runat="server" id="ConfirmChangesRequestLbl">Confirmar</label>  
                        </ContentTemplate>
                    </asp:UpdatePanel> 

                    <asp:updatepanel  ID="UpdatePanelCommentInsert" runat="server">
                        <ContentTemplate>
                            <textarea id="TextareaComment" maxlength="90" style="max-height:80px; min-height:35px" rows="2"  runat="server" class="form-control" placeholder="Introduzir Comentário" EnableViewState="true" />
                        </ContentTemplate>
                    </asp:updatepanel>
                </div>

                <div class="modal-footer">

                    <div style="display:flex;flex-direction:row">     
                        <button type="button" class="btn btn-secondary" style="margin-right:15px" data-dismiss="modal">CANCELAR</button>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:button runat="server" class="btn btn-primary" Text="CONFIRMAR" OnClick="BoxRequestModalAlterations_Click"   UseSubmitBehavior="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                     </div> 
                     
                </div>
            </div>
        </div>
    </div>


    <!-- Incomplete Boxes Requests Table Filters -->
    <div style="margin-bottom:20px; display:flex; flex-direction:row;justify-content:start;align-items:center;">          
     <button type="button" style="margin-right:10px;" class="btn btn-primary" data-toggle="modal" data-target="#insertRequestModal">
        Inserir novo pedido
     </button>
        <p style="margin-right:20px"></p>
    </div>

    <!-- Incomplete Boxes Requests Table -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <table class="table mt3">
                <thead>
                    <tr style="background-color:#d6e4ff;">
                        <th>Edificio</th> 
                        <th>Part Number</th>
                        <th>Designação</th>
                         
                        <th>2332/3332</th>
                        <th>2333/3333</th> 
                        <th>Quantidade Caixa</th>
                        <th>Localização - Quantidade</th>

                        <th>Data Pedido</th>
                        <th>Data Envio</th>
                        <th>Comentário</th> 
                        <th>Ações</th> <!-- New column for action buttons -->
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="IncompleteBoxRequests" runat="server" EnableViewState="true">
                        <ItemTemplate>
                            <tr>
                                <td style="justify-content:center;align-content:center"><%# Eval("Building") %></td> 
                                <td style="justify-content:center;align-content:center"><%# Eval("PartNumber") %></td>
                                <td style="justify-content:center;align-content:center"><%# Eval("Designation") %></td>
                                                                
                                <td style="justify-content:center;align-content:center"><%# Eval("ProdQuantity") %></td>
                                <td style="justify-content:center;align-content:center"><%# Eval("AvailableQuantity") %></td>
                                <td style="justify-content:center;align-content:center"><%# Eval("Quantity") %></td>
                                <td style="justify-content:center;align-content:center;"><%# Eval("Locations") %></td>

                                <td style="justify-content:center;align-content:center"><%# Eval("RequestDate") %></td>
                                <td style="justify-content:center;align-content:center"><%# Eval("SendDate") %></td>

                                <td style="justify-content:center;align-content:center;text-align: justify; text-justify: inter-word;"><%# Eval("Comment") %></td> 

                                <td style="justify-content:center;align-content:center">
                                    <!-- Update button to trigger the update -->
                                    <asp:linkbutton CssClass="btn btn-primary" ID="btnCompleteIncompleteBxsRq" runat="server" Text="Vender" CommandName="Insert"  OnCommand="btnsBxsRq_Command" CommandArgument='<%# Eval("id") %>'  UseSubmitBehavior="false"/>
                                    <asp:linkbutton CssClass="btn btn-warning" ID="btnUpdateIncompleteBxsRq" runat="server" Text="Comentar" CommandName="Comment"  OnCommand="btnsBxsRq_Command" CommandArgument='<%# Eval("id") %>' UseSubmitBehavior="false"/>
                                    <asp:linkbutton CssClass="btn btn-danger" ID="btndeleteIncompleteBxsRq" runat="server" Text="Apagar" CommandName="Delete"  OnCommand="btnsBxsRq_Command" CommandArgument='<%# Eval("id") %>' UseSubmitBehavior="false"/>
                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
