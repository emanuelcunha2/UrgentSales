<%@ Page Title="Setups" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Setups.aspx.cs" Inherits="Setups"  MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 

    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>   

    <script type="text/javascript">

        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
         
        $(document).ready(function () {
            console.log("Document ready");

            var isDragging = false;

            // Prevent checkbox clicks from interfering with sortable
            $("#draggableTable tbody, #draggableTable2 tbody").on('click', 'input[type="checkbox"]', function (e) {
                e.stopPropagation();
            });

            // Initialize sortable for the first table
            $("#draggableTable tbody").sortable({
                placeholder: "drag-placeholder",
                helper: "clone",
                update: function (event, ui) {
                    console.log("Updated sort order in table 1");
                    updatePriorityIndices("#draggableTable");
                },
                start: function (event, ui) {
                    isDragging = true;
                    $(ui.item).addClass('dragging');
                },
                stop: function (event, ui) {
                    setTimeout(function () {
                        isDragging = false;
                    }, 200); // Delay to ensure click event is not triggered
                    $(ui.item).removeClass('dragging');
                }
            }).disableSelection();

            // Initialize sortable for the second table
            $("#draggableTable2 tbody").sortable({
                placeholder: "drag-placeholder",
                helper: "clone",
                update: function (event, ui) {
                    console.log("Updated sort order in table 2");
                    updatePriorityIndices("#draggableTable2");
                },
                start: function (event, ui) {
                    isDragging = true;
                    $(ui.item).addClass('dragging');
                },
                stop: function (event, ui) {
                    setTimeout(function () {
                        isDragging = false;
                    }, 200); // Delay to ensure click event is not triggered
                    $(ui.item).removeClass('dragging');
                }
            }).disableSelection();

            // Handle click events separately
            $("#draggableTable tbody, #draggableTable2 tbody").on('click', 'tr', function (e) {
                if (!isDragging) {
                    var designation = $(this).find("td").eq(0).text();
                    var project = $(this).find("td").eq(1).text();
                    var line = $(this).find("td").eq(2).text();
                    triggerPostBack(designation, project, line);
                }
            });

            $("draggableTable3 tbody").sortable({
                placeholder: "drag-placeholder",
                helper: "clone",
                update: function (event, ui) {
                    console.log("Updated sort order in table 3");
                    updatePriorityIndices("draggableTable3");
                },
                start: function (event, ui) {
                    isDragging = true;
                    $(ui.item).addClass('dragging');
                },
                stop: function (event, ui) { 
                    setTimeout(function () {
                        isDragging = false;
                    }, 200);
                    $(ui.item).removeClass('dragging')
                }
            }).disableSelection();

            $("draggableTable4 tbody").sortable({
                placeholder: "drag-placeholder",
                helper: "clone",
                update: function (event, ui) {
                    console.log("Update sort order in table 4");
                    updatePriorityIndices("draggableTable4");
                },
                start: function (event, ui) {
                    isDragging = true;
                    $(ui.item).addClass('dragging')
                },
                stop: function (event, ui) {
                    setTimeout(function () {
                        isDragging = false;
                    }, 200);
                    $(ui.item).removeClass('dragging')
                }
            }).disableSelection();

        });

        function updatePriorityIndices(tableSelector) {
            console.log("Updating priority indices in " + tableSelector);
            $(tableSelector + " tbody tr").each(function (index) {
                $(this).find("td").eq(3).text(index + 1); // The PRIO column is the fourth column (index 3)
            });
              
            // Add event listener for the confirmation button
            $('#confirmYes').off('click').on('click', function () {
                $('#confirmModal').modal('hide'); // Hide the modal
                saveNewOrder(tableSelector); // Execute the save function
            });

            // Add event listener for the cancel button
            $('#confirmNo').off('click').on('click', function () {
                $('#confirmModal').modal('hide'); // Hide the modal
                location.reload();
            });

            // Add event listener for modal hidden event
            $('#confirmModal').on('hidden.bs.modal', function () {
                // Check if the modal was closed without confirmation
                location.reload(); // Reload the page
            });

            $('#confirmModal').modal('show'); // Show the confirmation modal 
        }

        function saveNewOrder(tableSelector) {
            var order = [];
            $(tableSelector + " tbody tr").each(function () {
                var designation = $(this).find("td").eq(0).text();
                var project = $(this).find("td").eq(1).text();
                var line = $(this).find("td").eq(2).text();
                var prio = $(this).find("td").eq(3).text();
                order.push({ designation: designation, project: project, line: line, prio: prio });
            });

            $.ajax({
                type: "POST",
                url: "Setups.aspx/SaveNewOrder",
                data: JSON.stringify({ order: order }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        showAlert("Ordem guardada com sucesso");
                    } else {
                        showAlert("Precisa de permissões para fazer esta operação");
                        location.reload(); 
                    }
                },
                failure: function (response) {
                    showAlert("Error: " + response.d);
                }
            });
        }

        function triggerPostBack(designation, project, line) {
            var rowData = designation + ',' + project + ',' + line;
            var hiddenField = document.getElementById('<%= hiddenField.ClientID %>');
            hiddenField.value = rowData;

            var hiddenButton = document.getElementById('<%= hiddenButton.ClientID %>');
            hiddenButton.click(); 
        }

        // Function to show the modal (called after postback if needed)
        function showSetupListModal() {
            document.getElementById('hiddenButtonSetupList').click();
        }

        function showAlert(message) {
            alert(message);
        }

        function calledFn() {
            alert("code fired");
        }

    </script>

    <div>
    <div>
        <style>
            body{
                background-color:#ffffff; 
            }

            .setupArea-container-row {
                display: flex;
                flex-direction: row;
                align-items:center;
                justify-content:center; 
                width:100%;
            }

            .setupArrow{

                height:50px;
                display: flex;
                flex-direction: row;
                align-items:center;
                justify-content:center; 
                width: 33.333%;
            }
            
            .setupArea-container { 
                min-height: 50px;
                width: 33.333%;
                display:flex;
                flex-direction:column;
                align-items:center;
                justify-content:end;
                justify-items:center;
                padding:20px;
            }

            .tableSetups{
                margin-top:20px;
            }

            .tableSetups thead tr:first-child th:first-child {
                border-top-left-radius: 6px; 
            }

            .tableSetups thead tr:first-child th:last-child {
                border-top-right-radius: 6px; 
            }

            .tableSetups tbody tr:last-child td:first-child {
                border-bottom-left-radius: 6px;
            }

            .tableSetups tbody tr:last-child td:last-child {
                border-bottom-right-radius: 6px;
            }

            .clickable{
                cursor:pointer; 
            }
            .default{
                cursor:default;
            }

            .modal-container{
                width: 69%; 
            }
            .modal-container2{
                width: 74%; 
            }

            .padTopnBottom{
                height:45px;
                padding-top:11px
            }

            .modal-dialog-centered {
               display: flex;
               align-items: center;
               justify-content: center; 
               height: 100vh;
            }

            .w100{ 
                width:100%; 
                display:flex;
                align-items: center;
                flex-direction:column;
                justify-content:center;
            }

            .drag-placeholder {
                background-color:#f0f0f0; 
                border: 2px dashed #b0b0b0; 
                width:100%;
                height: 35px;
            }

           .ui-sortable-helper {
                display: table;
                background-color: #e0e0e0;
                border: 1px solid #ccc;
            }

            @media only screen and (min-width: 50px) {
                .setupArea-container-row {
                   flex-direction: column; 
                }
                .setupArrow{
                    display: none;
                }
                .setupArea-container{
                    width:95%; 
                }
            }

            @media only screen and (min-width: 1100px) {
                .setupArea-container-row {
                    flex-direction: row; 
                }
                .setupArrow{
                    display: flex;
                }
                .setupArea-container{
                    width:33.333%;  
                }
            }

        </style>
         
        <asp:HiddenField ID="hiddenField" runat="server"  /> 
        <asp:Button ID="hiddenButton" runat="server" style="display:none;" OnClick="LinkButton_Click" />   

        <button style="display:none;" id="hiddenButtonSetupList" data-toggle="modal" data-target="#setupListModal"></button>

        <div class="jumbotron">
            <h1>  Gestão de Setups </h1>
        </div> 

        <div style="display:flex;flex-direction:row">    
            <button type="button" style="margin-right:10px;" class="btn btn-primary" data-toggle="modal" data-target="#setupModal">
                INSERIR SETUP
            </button>
            <button type="button" style="margin-right:10px;" class="btn btn-danger" data-toggle="modal" data-target="#setupModalDelete">
                ELIMINAR SETUP
            </button>
            <button type="button" style="margin-right:10px;" class="btn btn-success" data-toggle="modal" data-target="#setupModalImportList">
                IMPORTAR LISTA SETUP
            </button>
        </div> 

        <hr />

        <!-- Confirm Modal -->
        <div class="modal fade" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="confirmModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modal-container">
                    <div class="modal-header">
                        <h4 class="modal-title" >Confirmar</h4> 
                    </div>
                    <div class="modal-body">
                        <label>Pretende alterar esta ordem?</label>  
                    </div>
                    <div class="modal-footer">
                        <div style="display:flex;flex-direction:row">     
                            <button type="button" class="btn btn-secondary" id="confirmNo" data-dismiss="modal">Não</button>
                            <button type="button" class="btn btn-danger" id="confirmYes">Sim</button> 
                        </div> 
                       
                    </div>
                </div>
            </div>
        </div>

        <!-- Insert New Setup -->
        <div class="modal fade" id="setupModal" tabindex="-1" role="dialog" aria-labelledby="setupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modal-container">
                    <div class="modal-header">
                        <h4 class="modal-title" id="setupModalLabel">INSERIR SETUP</h4> 
                    </div>
                    <div class="modal-body">
                        <label>Setup</label>
                        <input id="txtSetup" runat="server" class="form-control" placeholder="Introduzir Setup" EnableViewState="true" />

                        <label style="margin-top:15px;">Projeto</label>
                        <input id="txtProject" runat="server" class="form-control" placeholder="Introduzir Projeto" EnableViewState="true" />

                        <label style="margin-top:15px;">Linha</label>
                        <input id="txtLine" runat="server" class="form-control" placeholder="Introduzir Linha" EnableViewState="true" />

                    </div>
                    <div class="modal-footer">
                        <div style="display:flex;flex-direction:row">    
                            <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">FECHAR</button>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>                                    
                                    <asp:button runat="server"   class="btn btn-primary" OnClick="InsertSetup_Click" Text="INSERIR"   />
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </div> 
                         
                    </div>
                </div>
            </div>
        </div>

        <!-- Delete Setup -->
        <div class="modal fade" id="setupModalDelete" tabindex="-1" role="dialog" aria-labelledby="setupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modal-container">
                    <div class="modal-header">
                        <h4 class="modal-title">ELIMINAR SETUP</h4> 
                    </div>
                    <div class="modal-body">
                        <label>Setup</label>
                        <input id="txtDeleteSetup" runat="server" class="form-control" placeholder="Introduzir Setup" EnableViewState="true" /> 
                    </div>
                    <div class="modal-footer">
                        <div style="display:flex;flex-direction:row">    
                            <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">FECHAR</button>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:button runat="server"   class="btn btn-danger" OnClick="DeleteSetup_Click" Text="ELIMINAR"   />   
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </div>                 
                    </div>
                </div>
            </div>
        </div>
         
        <!-- Import Setup List -->
        <div class="modal fade" id="setupModalImportList" tabindex="-1" role="dialog" aria-labelledby="setupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modal-container">
                    <div class="modal-header">
                        <h4 class="modal-title">IMPORTAR LISTA SETUP</h4>
                    </div>
                    <div class="modal-body">
                        <label>Setup</label>
                        <input id="txtImportSetup" runat="server" class="form-control" placeholder="Introduzir Setup" EnableViewState="false" />

                        <label style="margin-top:15px;">Importar Arquivo Excel</label>
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control padTopnBottom" />
                    </div>
                    <div class="modal-footer">
                        <div style="display:flex;flex-direction:row">
                            <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">FECHAR</button>
                            <asp:button ID="btnUpload" runat="server" class="btn btn-primary" OnClick="ImportSetupList_Click" Text="IMPORTAR" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Setup list Modal -->
        <div class="modal fade" id="setupListModal" tabindex="-1" role="dialog" aria-labelledby="setupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content modal-container2">
                    <div class="modal-header">
                        <h4 class="modal-title">LISTA SETUP &nbsp<b style="color:cornflowerblue"><%: SelectedSetup %></b></h4> 
                    </div>
                    <div class="modal-body">
                        <table class="table" style="width:100%; border-radius:8px;">
                            <thead>
                                <tr style="background-color:#ffffff; border-bottom: 1px solid #b0b0b0;">
             
                                    <th style="border: 0px solid #8a8a8a;">PN</th>
                                    <th style="border: 0px solid #8a8a8a;">DESIGNAÇÃO</th>
                                    <th style="border: 0px solid #8a8a8a;" colspan="2">QUANTIDADE</th> 
                                </tr>
                            </thead>
                            <tbody>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:Repeater ID="SetupListRepeater" runat="server">
                                            <ItemTemplate>
                                                <tr draggable="true" class="clickable" style="background-color:white; border-bottom: 1px solid #dbdbdb;">
                                                    <td style="border: 0px solid #8a8a8a;"><%# Eval("Pn") %></td>
                                                    <td style="border: 0px solid #8a8a8a;"><%# Eval("PnDescription") %></td>
                                                    <td style="border: 0px solid #8a8a8a;"><%# Eval("Qty") %></td> 
                                                </tr>
                                            </ItemTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            </tbody>
                        </table>
                    </div>

                    <div class="modal-footer">
                        <div style="display:flex;flex-direction:row">    
                            <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">FECHAR</button>
                        </div>                 
                    </div>

                </div>
            </div>
        </div>

        <!-- Setups Manager -->
        <div style="align-items:center; justify-content:center; display:flex; flex-direction:column">
            <!-- Top Row -->
            <div class="setupArea-container-row">

                <!-- Arrow Down --> 
                <div class="setupArrow"> 
                </div>


                <!-- Assemble Setups -->
                <div class="setupArea-container"> 
           
                     <asp:UpdatePanel runat="server" style="width:100%;">
                        <ContentTemplate>

                        <div class="w100">                                      
                            <asp:Button class="btn btn-primary" runat="server"  Text="ATIVAR SETUPS" OnClick="SetupAssemble_Click" />
                        </div> 

                        <div class="mt-3 tableSetups" style="border-radius:6px;border: 1px solid #b0b0b0;" >
                            <table id="draggableTable" class="table" style="width:100%; border-radius:8px;">
                                <thead>
                                    <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                        <th style="text-align:center; border: 0px solid #8a8a8a; font-size:1.5em" colspan="5">SETUPS PARA MONTAR</th>
                                    </tr>
                                    <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                        <th style="border: 0px solid #8a8a8a;">SETUP</th>
                                        <th style="border: 0px solid #8a8a8a;">PROJETO</th>
                                        <th style="border: 0px solid #8a8a8a;" colspan="1">LINHA</th> 
                                        <th style="border: 0px solid #8a8a8a;" colspan="1">PRIO</th> 
                                        <th style="border: 0px solid #8a8a8a;" colspan="1"></th> 
                                    </tr>
                                </thead>
                                <tbody>
                                   <asp:Repeater ID="AssembleSetupsRepeater" runat="server" EnableViewState="true">
                                        <ItemTemplate>
                                             <tr class="clickable" style="background-color:white; border-bottom: 1px solid #dbdbdb;" >
                                                <td style="border: 0px solid #8a8a8a;"><%# Eval("Designation") %></td>
                                                <td style="border: 0px solid #8a8a8a;"><%# Eval("Project") %></td>
                                                <td style="border: 0px solid #8a8a8a;"><%# Eval("Line") %></td>
                                                <td style="border: 0px solid #8a8a8a;"><%# Eval("Priority") %></td>

                                                <td style="border: 0px solid #8a8a8a; display:flex; align-items:center; justify-content:center;">
                                                    <asp:CheckBox ID="chk1" runat="server" EnableViewState="true" 
                                                        style="margin:auto; height:20px; transform: scale(1.35);" 
                                                        Checked='<%# Bind("IsChecked") %>'
                                                        OnCheckedChanged="Assemble_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!-- arrow left -->
                <div class="setupArrow"> 
                </div>
                 

            </div>

            <!-- Middle Row -->
            <div class="setupArea-container-row">
                <!-- Active Setups -->
                <div class="setupArea-container" > 

                     <asp:UpdatePanel runat="server" style="width:100%;">
                        <ContentTemplate>
                            <div class="w100">     
                                <asp:Button class="btn btn-primary" runat="server" Text="DESMONTAR SETUPS" OnClick="SetupActive_Click" />
                            </div> 

                            <div class="mt-3 tableSetups" style="border-radius:6px;border: 1px solid #b0b0b0;" >
                            <table class="table" style="width:100%; border-radius:8px;">
                                <thead>
                                    <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                        <th style="text-align:center; border: 0px solid #8a8a8a; font-size:1.5em" colspan="4">SETUPS ATIVOS</th>
                                    </tr>
                                    <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                        <th style="border: 0px solid #8a8a8a;">SETUP</th>
                                        <th style="border: 0px solid #8a8a8a;">PROJETO</th>
                                        <th style="border: 0px solid #8a8a8a;" colspan="2">LINHA</th> 
                                    </tr>
                                </thead>
                                <tbody>
                                   <asp:Repeater ID="ActiveSetupsRepeater" runat="server" EnableViewState="true">
                                        <ItemTemplate>
                                             <tr class="clickable" style="background-color:white; border-bottom: 1px solid #dbdbdb;" >
                                                <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Designation") %></td>
                                                <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Project") %></td>
                                                <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Line") %></td>
                                                <td style="border: 0px solid #8a8a8a; display:flex; align-items:center; justify-content:center;">
                                                <asp:CheckBox ID="chk1" runat="server" EnableViewState="true" 
                                                    style="margin:auto; height:20px; transform: scale(1.35);" 
                                                    Checked='<%# Bind("IsChecked") %>' 

                                                    OnCheckedChanged="Active_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                    </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
        
                <!-- Setup Cicle Title --> 
                <div class="setupArea-container">
                   <div style="background-color:#ebebeb;padding:20px;width:100%; border-radius:6px; border:1px solid #8a8a8a;display:flex;justify-content:center;align-items:center; height:250px;flex-direction:column; font-size:17px">
                        <p style="text-align:center"><b>CICLO DE MONTAGEM</b></p>
                        <p style="text-align:center"><b>&</b></p>
                        <p style="text-align:center"><b>DESMONTAGEM DE SETUPS</b></p>
                   </div>
                </div>

                <!-- Inactive Setups -->
                <div class="setupArea-container">
                 
                     <asp:UpdatePanel runat="server" style="width:100%;">
                        <ContentTemplate>
                       
                                <div class="w100">                                      
                                    <asp:Button class="btn btn-primary" runat="server" Text="MONTAR SETUPS" OnClick="SetupInactive_Click"/>
                                </div>  

                                <div class="mt-3 tableSetups" style="border-radius:6px;border: 1px solid #b0b0b0;" >
                                <table class="table" style="width:100%; border-radius:8px;">
                                    <thead>
                                        <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                            <th style="text-align:center; border: 0px solid #8a8a8a; font-size:1.5em" colspan="4">SETUPS INATIVOS</th>
                                        </tr>
                                        <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                            <th style="border: 0px solid #8a8a8a;">SETUP</th>
                                            <th style="border: 0px solid #8a8a8a;">PROJETO</th>
                                            <th style="border: 0px solid #8a8a8a;" colspan="2">LINHA</th> 
                                        </tr>
                                    </thead>
                                    <tbody>
                                       <asp:Repeater ID="InactiveSetupsRepeater" runat="server" EnableViewState="true">
                                            <ItemTemplate>
                                                 <tr class="clickable" style="background-color:white; border-bottom: 1px solid #dbdbdb;" >
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Designation") %></td>
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Project") %></td>
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Line") %></td>
                                                    <td style="border: 0px solid #8a8a8a; display:flex; align-items:center; justify-content:center;">
                                                    <asp:CheckBox ID="chk1" runat="server" EnableViewState="true" 
                                                        style="margin:auto; height:20px; transform: scale(1.35);" 
                                                        Checked='<%# Bind("IsChecked") %>' 
                                                        OnCheckedChanged="Inactive_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>

            <!-- Bottom Row -->
            <div class="setupArea-container-row">

               <!-- arrow right -->
               <div class="setupArrow">
  
               </div>

                <!-- Disassemble Setups -->
                <div class="setupArea-container">
                     
                     <asp:UpdatePanel runat="server" style="width:100%;">
                        <ContentTemplate>
                        
                            <div class="w100">                                      
                                <asp:Button class="btn btn-primary" runat="server" Text="DESATIVAR SETUPS" OnClick="SetupDisassemble_Click"/>
                            </div> 

                            <div class="mt-3 tableSetups" style="border-radius:6px;border: 1px solid #b0b0b0;" >
                                <table id="draggableTable2" class="table" style="width:100%; border-radius:8px;">
                                    <thead>
                                        <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                            <th style="text-align:center; border: 0px solid #8a8a8a; font-size:1.5em" colspan="5">SETUPS PARA DESMONTAR</th>
                                        </tr>
                                        <tr style="background-color:#d6e4ff; border-bottom: 1px solid #b0b0b0;">
                                             <th style="border: 0px solid #8a8a8a;">SETUP</th>
                                             <th style="border: 0px solid #8a8a8a;">PROJETO</th>
                                             <th style="border: 0px solid #8a8a8a;" colspan="1">LINHA</th> 
                                             <th style="border: 0px solid #8a8a8a;" colspan="1">PRIO</th> 
                                             <th style="border: 0px solid #8a8a8a;" colspan="1"></th> 
                                        </tr>
                                    </thead>
                                    <tbody>
                                       <asp:Repeater ID="DisassembleSetupsRepeater" runat="server" EnableViewState="true">
                                            <ItemTemplate>
                                                 <tr class="clickable" style="background-color:white; border-bottom: 1px solid #dbdbdb;">
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Designation") %></td>
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Project") %></td>
                                                    <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Line") %></td>
                                                     <td style="border: 0px solid #8a8a8a;" onclick='triggerPostBack("<%# Eval("Designation") %>", "<%# Eval("Project") %>", "<%# Eval("Line") %>")'><%# Eval("Priority") %></td>
                                                    <td style="border: 0px solid #8a8a8a; display:flex; align-items:center; justify-content:center;">
                                                    <asp:CheckBox ID="chk1" runat="server" EnableViewState="true" 
                                                        style="margin:auto; height:20px; transform: scale(1.35);" 
                                                        Checked='<%# Bind("IsChecked") %>' 
                                                        OnCheckedChanged="Disassemble_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                        
                </div>

                 <!-- arrow top -->
                <div class="setupArrow">
                  
                </div>
                  
            </div>

        </div>
        
    </div>
    </div>  

</asp:Content>
 

