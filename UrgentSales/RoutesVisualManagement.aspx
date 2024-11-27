 <%@ Page Title="Rotas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="RoutesVisualManagement.aspx.cs" Inherits="RoutesVisualManagement" %>


<asp:Content ID="BodyContent" EnableViewState="true" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        p{
            margin:0;
            padding:0;
        }

        /* START: Display */
        .grid{
            display:grid;
        }

        .grid-C1R1{
            grid-row:1;
            grid-column:1;
        }

        .flex-cols {
            display: flex;
            flex-direction: column;
        }

        .flex-rows {
            display: flex;
            flex-direction: row;
        }

        .flex-rows-center-y {
            align-items: center;
        }

        .flex-rows-center-x{
            justify-content:center;
        }
 
        .flex-100 {
            flex: 1;
        }

        .flex-cols-center {
            justify-content: center;
            align-items: center;
        }

        .flex-cols-center p{
            text-align:center;
        }

        .flex-cols-center-y {
            justify-content: center;
        }

        .fill-width{
            width:100%;
        }

        .fill-height{
            height:100%;
        }

        .fill{
            width:100%;
            height:100%;
        }

        .medium-width{
            min-width:80px;
            width:15%;
        }

        .background-green{
            background-color:green;
        }

        .background-red{
            background-color:red;
        }

        /* END: Display */

        .TestingClass {
            transition: 0.2s; 
        }

        .TestingClass:hover {
            filter: brightness(80%) contrast(120%);
            cursor: pointer;
         }

        .add-justification-icon{
            opacity:0;
        }

        .TestingClass:hover .add-justification-icon {
               opacity:0.5;
        }

        .modal-dialog-centered {
            display: flex;
            align-items: center;
            justify-content: center; 
            margin:auto; 
        }

        .displayTransparent{
            opacity:0;
        }

        .min-height-micro{
            min-height:1px;
        }

        .routesList{
            z-index:1;  
        }

        .BreakTypeHidden{
            display:none;
        }

        .BreakTypeVisible{
            display:block;
        }

        .tiny{
            width:1px;
            height:1px;
            padding:0;
            margin:0;
            position:absolute;
            pointer-events:none;
        }
        
        .no-maxWidth{
            max-width:2000px;
            min-width:0px; 
            height:100%;
            max-height:2000px;
            overflow:hidden;
            padding:0;
            
        }

        .medium-scroll{
            max-height:400px;
            overflow-y:auto;
            overflow-x:hidden;
        }

        .round{ 
        }

        .background-test{
            opacity:0;
        }

        .background-test:hover{
            background-image:url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="%235f6368"><path d="M200-120q-33 0-56.5-23.5T120-200v-560q0-33 23.5-56.5T200-840h360v80H200v560h560v-360h80v360q0 33-23.5 56.5T760-120H200Zm120-160v-80h320v80H320Zm0-120v-80h320v80H320Zm0-120v-80h320v80H320Zm360-80v-80h-80v-80h80v-80h80v80h80v80h-80v80h-80Z"/></svg>');
            background-size: 28px 28px; 
            background-repeat: no-repeat;
            background-position: center;
            opacity:0.25;
        }
    </style>

    <script>
        function JustificationSelected(registerId) {
            console.log("This");
            // Find the hidden button inside the Repeater item
            var hiddenButton = document.getElementById('<%= triggerJustificationSelectChange.ClientID %>');

            // Dynamically set the CommandArgument for the button from JavaScript
            document.getElementById('<%= hiddenField.ClientID %>').value = registerId;

            // Programmatically click the button
            hiddenButton.click();
        }

        function TestIt() {
            console.log("This");
        }
    </script>

    <!-- Buttons to communicate to C# from script -->   
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hiddenField" runat="server" />
            <asp:Button class="displayTransparent tiny" runat="server" ID="triggerJustificationSelectChange" OnClick="justificationButton_Click"  />
        </ContentTemplate>
    </asp:UpdatePanel> 

    <!-- Execute On Timer -->
    <asp:UpdatePanel runat="server" id="UpdatePanel3">
        <ContentTemplate>
            <asp:Timer runat="server" id="UpdateTimer" Interval="45000" OnTick="UpdateTimer_Tick"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="jumbotron">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                    <h1 id ="PageMainTitle" runat="server">  Gestão de Rotas </h1> 
            </ContentTemplate>
        </asp:UpdatePanel> 
    </div>

    <!-- Filters -->
    <div class="flex-rows flex-rows-center-y fill-width" >

        <h4 style="margin:0;margin-right:15px;padding:0">Edifício</h4>

        <select id="selectedBuilding" runat="server" class="form-control" aria-label="Default select example">
           <option value="" selected>ALL</option> 
           <option value="VS1">VS1</option>
           <option value="VS2">VS2</option>
        </select>
         
        <h4 style="margin:0;margin-right:15px;margin-left:30px;padding:0">Modo</h4>

        <select id="selectedMode" runat="server" class="form-control" aria-label="Default select example">
          <option value="none" selected>24h</option>
          <option value="now">Turno Atual</option>
          <option value="1">Turno 1</option>
          <option value="2">Turno 2</option>
          <option value="3">Turno 3</option>
        </select>

        <div style="margin-left:30px">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button class="btn btn-primary flex-cols flex-cols-center-y" runat="server" ID="ConfirmFilterButton" OnClick="ConfirmFilterButton_Click"  text="Confirmar"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- Fill Div -->
        <div class="flex-100"></div>

        <!-- Delete Route Button -->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                       <button  type="button"  style="margin-right:0" class="btn btn-danger flex-rows flex-rows-center-y" data-toggle="modal" data-target="#deleteRouteModal">
                           <p style="padding:0;margin:0;margin-right:10px">Eliminar Rota</p>
                           <svg xmlns="http://www.w3.org/2000/svg" height="20px" viewBox="0 -960 960 960" width="20px" fill="White"><path d="M280-120q-33 0-56.5-23.5T200-200v-520h-40v-80h200v-40h240v40h200v80h-40v520q0 33-23.5 56.5T680-120H280Zm400-600H280v520h400v-520ZM360-280h80v-360h-80v360Zm160 0h80v-360h-80v360ZM280-720v520-520Z"/></svg>
                       </button>
             </ContentTemplate>
        </asp:UpdatePanel>
 
       <button  type="button"  style="margin-left:15px;margin-right:0" class="btn btn-success flex-rows flex-rows-center-y" data-toggle="modal" data-target="#insertRouteModal">
           <p style="padding:0;margin:0;margin-right:10px">Adicionar nova rota</p>
           <svg xmlns="http://www.w3.org/2000/svg" height="20px" viewBox="0 -960 960 960" width="20px" fill="White"><path d="M160-760v560h240v-560H160ZM80-120v-720h720v160h-80v-80H480v560h240v-80h80v160H80Zm400-360Zm-80 0h80-80Zm0 0Zm320 120v-80h-80v-80h80v-80h80v80h80v80h-80v80h-80Z"/></svg>
       </button>

    </div>

    <!-- List of routes -->
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="flex-rows flex-rows-center-x"  style="margin-top:20px">
                <div class="grid" style="width:100%; overflow-y:hidden; border-bottom: 0px solid #a8a8a8">
        
                    <!-- Current Time Div -->
                    <div class="flex-rows grid-C1R1 fill" style="">
                        <div style="width:10vw;"></div>
                        <div style="<%: DayHeightStyleFlex %>;"></div>
                        <div class="flex-rows fill-height" style="margin-left:2px;width:5px; background-color:#ffc524;">
                            <h5 style="padding:0; margin-left:20px; color:#333333"><b><%: CurrentDayDate.ToString("HH:mm tt") %></b></h5>
                        </div>
                    </div>

                    <!-- Current Time line div --> 
                    <div class="grid-C1R1 flex-rows fill" style="background-color:transparent;pointer-events:none; z-index:2;">
                        <div style="width:10vw;"></div>
                        <div style="<%: DayHeightStyleFlex %>; "></div>
                        <div class="fill-height" style="margin-left:2px; margin-top:2px; width:5px; background-color:#ffc524;"></div>
                    </div>
                     
                    <!-- Routes & Breaks List Div -->
                    <div class="flex-cols flex-cols-center-y grid-C1R1 routesList fill" style="" runat="server">
                        <asp:Repeater ID="routesRepeater" runat="server"  OnItemDataBound="routesRepeater_ItemDataBound">
                            <ItemTemplate>      
                                    <div class="grid"> 

                                        <!-- Route Lines -->
                                        <div class="flex-rows fill-width grid-C1R1" style="height:25vh; background-color:transparent; padding:35px 0px 35px 0px; border-radius:6px; border: 0px solid #a8a8a8; overflow:hidden"> 
                                        
                                            <div style="width:10vw;background-color:sandybrown"></div>
                                            <div class="flex-cols fill">
                                                <div class="flex-100" style="border-top:1px solid #b5b5b5" ></div> 
                                                <div class="flex-100" style="border-top:1px solid #b5b5b5; border-bottom:1px solid #b5b5b5;" ></div>
                                                 
                                            </div>
                                        </div>
                                        
                                        <!-- A Route -->
                                        <div class="flex-rows fill-width grid-C1R1" style=" height:25vh; background-color:transparent; padding:35px 0px 35px 0px; margin:0vw; border-radius:6px; border: 0px solid #a8a8a8; overflow:hidden">
                                        
                                            <!-- Route Title & Route Settings Button -->
                                            <div class="fill-width flex-rows TestingClass" style="padding:0px 0px 0px 0px; pointer-events:all; width:10vw;">
                                                <div class="fill grid" style="background-color:#d6e4ff; border: 1px solid #b5b5b5; border-radius:3px; flex:1" >
                                                    <!-- Route Title -->
                                                    <div class="fill flex-cols flex-cols-center grid-C1R1" style="z-index:1;">
                                                        <h4> <%# Eval("Name") %></h4>
                                                        <p style="color:#828282">Rota de <%# Eval("Minutes") %> Minutos</p>
                                                    </div>
                                                    <div class="fill grid-C1R1 grid displayTransparent" style="z-index:2;">
                                                        <asp:button runat="server" CausesValidation="false" class="fill grid-C1R1 no-maxWidth"  OnClick="RouteSettings_Click" CommandArgument='<%# Eval("Id") %>' />
                                                    </div>
                                                </div>

                                                <!-- Minutes Indicator -->
                                                <div class="fill-height flex-cols flex-rows-center-y" style="background-color:white;width:50px;">
                                                    <div style="flex:1"></div>
                                                    <h4 style="padding:0;margin:0"> <%# Eval("Minutes") %></h4>
                                                    <div style="flex:1"></div>
                                                </div>
                                            </div>

                                            <!-- Route Registers Container -->
                                            <div class="fill flex-100 flex-cols" style="background-color: transparent;  ">      

                                                <!-- Breaks and Registers Div --> 
                                                <div class="grid fill">

                                                    <!-- Breaks -->
                                                    <div class="fill flex-100 flex-rows grid-C1R1" style="background-color:transparent;opacity:0.75;padding:0px; z-index:2; pointer-events: none;">
                                                        <asp:Repeater ID="routeBreaksRepeater" runat="server" >
                                                            <ItemTemplate> 
                                                                <div class="grid" style='<%# "flex:" + Eval("BreakStyle") + " border-right: 0px solid #262626;   overflow:hidden;   " %>'>
                                                                    <div class="fill grid"  style="overflow:hidden;">     
                                                                        <%-- <div style="height:1px;background-color:black; margin-top:0;"  class="fill-width grid-C1R1"></div>--%>
                                                                        <%--<p class="grid-C1R1" style="margin:auto;color:white; padding:0"><%# Eval("BreakType")  %></p>--%>
                                                                    </div>   
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>  
                                                    </div>

                                                    <!-- Registers --> 
                                                    <div class="fill flex-100 flex-rows grid-C1R1"> 
                                                        <div class="flex-rows" style="<%: DayHeightStyleFlex %>  background-color:transparent;padding:0px; z-index:5; ">
                                                            <asp:Repeater ID="routeRegistersRepeater" runat="server" >
                                                                <ItemTemplate> 
                                                                    <div class="flex-cols" style='<%# "flex:" + Eval("RouteStyle") + " border-right: 0px solid #262626;overflow:visible; " %>'>
                                                                        <!-- register positioning div -->
                                                                        <div style='<%# Eval("RegisterPostionStyle") %>'></div>

                                                                        <div style="position:relative;right:-10px;top:-10px;background-color:transparent">
                                                                            <div class="TestingClass" style='<%# "" + Eval("RegisterStyle") + " height:20px;width:20px;border-radius:30px; border:1px solid #6b6b6b;right:0;;z-index:10;position:absolute;" %>'></div>
                                                                        </div>
                                                                     </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>

                                                </div>
                   
                                            </div>  
                                        </div>
                                    </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- Modals #1 -  Insert Route -->
    <div class="modal fade"   id="insertRouteModal" tabindex="-1" role="dialog" aria-labelledby="insertRouteModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered fill " style="pointer-events:none"  role="document">
            <div class="modal-content modal-container" style="width:420px;pointer-events:auto">
                <div class="modal-header">
                    <h4 class="modal-title" id="setupModalLabel">Inserir nova rota</h4> 
                </div>
                <div class="modal-body">
                    <label>Edifício</label>
                    <select id="slcInsertNewRouteBuilding" runat="server" class="form-control" aria-label="Default select example">
                        <option value="VS1" selected>VS1</option>
                        <option value="VS2">VS2</option>
                    </select>

                    <label style="margin-top:15px;">Nome da rota</label>
                    <input id="txtInsertNewRouteName" runat="server" class="form-control" placeholder="Introduzir Rota" EnableViewState="true" />


                     <label style="margin-top:15px;">Minutos da rota</label>
                     <input id="txtInsertNewRouteMin" type="number" runat="server" class="form-control" placeholder="Introduzir Minutos" EnableViewState="true" />

                     <!-- Breaks -->
                     <label style="margin-top:15px;">Pausas da Rota</label>
                     <asp:UpdatePanel class="flex-cols medium-scroll" runat="server">
                         <ContentTemplate>
                             
                             <asp:Button ID="NewBreakButton" EnableViewState="true"  OnClick="NewBreakButton_Click"  CssClass="btn btn-success" runat="server" Text="Adicionar nova Pausa"/>

                             <asp:Repeater ID="AddRoutesRepeater"  OnItemDataBound="AddRoutesRepeater_ItemDataBound" runat="server">
                                 <ItemTemplate>
                                     <div style="margin-top:17px; margin-bottom:17px;" class="flex-rows flex-rows-center-y">
                                       <label style=" margin:0; padding:0;margin-right:15px">Nova Pausa <%# (int)Eval("Id") + 1 %></label>
                                       <p style=" margin:0; padding:0; margin-right:10px; ">Paragem de linha?</p>
 
                                       <select id="BreakStoppedLine" runat="server" class="form-control" style="max-width:85px" aria-label="Default select example">
                                           <option value="1" selected>Sim</option>
                                           <option value="0">Não</option>
                                       </select>
                                    </div>
                                      
                                      <div class="flex-rows flex-rows-center-y">
                                        <p style=" margin:0; padding:0; margin-right:10px; ">Inicio</p>
                                        <input class="form-control" runat="server" id="TimeStart" type="time" />

                                        <p style="margin:0;padding:0; margin-right:10px; margin-left:10px;">Fim</p>
                                        <input style="margin-right:10px;" class="form-control" runat="server" id="TimeEnd" type="time" />

                                        <asp:Button ID="RemoveBreakButton" OnClick="RemoveBreakButton_Click" runat="server" CssClass="btn btn-danger" Text="-" CommandArgument='<%# Eval("Id") %>' />
                                      </div> 
                                 </ItemTemplate>
                             </asp:Repeater>

                         </ContentTemplate>
                     </asp:UpdatePanel>

                </div>
                <div class="modal-footer">

                    <div style="display:flex;flex-direction:row">    
                        <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">Cancelar</button>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>                                    
                                <asp:button ID="Btn_InsertRoute" runat="server"   class="btn btn-primary"   Text="Inserir" OnClick="Btn_InsertRoute_Click" OnClientClick=""/>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div> 
                         
                </div>
            </div>
        </div>
    </div>

   <!-- Modals #2 Delete Route -->
   <div class="modal fade" id="deleteRouteModal" tabindex="-1" role="dialog" aria-labelledby="deleteRouteModal" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered fill " style="pointer-events:none"  role="document">
          <div class="modal-content modal-container" style="width:300px;pointer-events:auto">
               <div class="modal-header">
                   <h4 class="modal-title" id="deleteRouteModalLabel">Selecionar rota a eliminar</h4> 
               </div>
               <div class="modal-body">
                   <label>Rota</label>

                   <!-- All Routes Selection -->
                   <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                           <select runat="server" id="SlcRouteToDelete" class="form-control" aria-label="Default select example">
                           </select>
                        </ContentTemplate>
                   </asp:UpdatePanel>
               </div>

               <div class="modal-footer">
                   <div style="display:flex;flex-direction:row">    
                       <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">Cancelar</button>
                       <asp:UpdatePanel runat="server">
                           <ContentTemplate>                                    
                               <asp:button runat="server" CausesValidation="false" class="btn btn-danger" Text="Confirmar" OnClick="DeleteRoute_Click" />
                           </ContentTemplate>
                       </asp:UpdatePanel> 
                   </div> 
               </div>

           </div>
       </div>
   </div>

   <!-- Modals #3 - Insert Justification -->
   <div class="modal fade" id="justificationInsertModal" tabindex="-1" role="dialog" aria-labelledby="justificationInsertModal" aria-hidden="false">
      <div class="modal-dialog modal-dialog-centered fill " style="pointer-events:none"  role="document">
          <div class="modal-content modal-container" style="width:300px;pointer-events:auto">
               <div class="modal-header">
                   <h4 class="modal-title" id="justificationInsertModalLabel">Selecionar justificação</h4> 
               </div>
               <div class="modal-body">
                   <label>Justificação</label>

                   <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                           <select runat="server" id="justificationInsertValue" class="form-control" aria-label="Default select example">
                               <option value="3" selected>Falta de Material</option> 
                           </select>
                        </ContentTemplate>
                   </asp:UpdatePanel>
                    
               </div>

               <div class="modal-footer">
                   <div style="display:flex;flex-direction:row">    
                       <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">Cancelar</button>
                       <asp:UpdatePanel runat="server">
                           <ContentTemplate>                                    
                               <asp:button runat="server" class="btn btn-primary"   Text="Inserir" OnClick="JustificationInsert_Click"  />
                           </ContentTemplate>
                       </asp:UpdatePanel> 
                   </div> 
               </div>

           </div>
       </div>
   </div>

   <!-- Modals #4 -  Update Route -->
   <div class="modal fade" id="updateRouteModal" tabindex="-1" role="dialog" aria-labelledby="updateRouteModal" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered fill " style="pointer-events:none"  role="document">
          <div class="modal-content modal-container" style="width:420px;pointer-events:auto">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                               
                            <h4 runat="server" class="modal-title" id="updateRouteModalLabel">Alterar Rota </h4> 
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <label>Edifício</label>
                          
                            <select id="SelectedRouteBuilding" runat="server" class="form-control" aria-label="Default select example">
                                <option value="VS1" selected>VS1</option>
                                <option value="VS2">VS2</option>
                            </select>

                            <label style="margin-top:15px;">Nome da rota</label>
                            <input id="SelectedRouteName" runat="server" class="form-control" placeholder="Introduzir Rota" EnableViewState="true" />


                            <label style="margin-top:15px;">Minutos da rota</label>
                            <input id="SelectedRouteMinutes" type="number" runat="server" class="form-control" placeholder="Introduzir Minutos" EnableViewState="true" />

                         </ContentTemplate>
                    </asp:UpdatePanel>

                    <!-- Breaks -->
                    <label style="margin-top:15px;">Pausas da Rota</label>
                    <asp:UpdatePanel class="flex-cols medium-scroll" runat="server">
                        <ContentTemplate>
                                    
                            <asp:Button  ID="UpdateNewRoutesButton" CausesValidation="false" OnClick="UpdateNewRoutesButton_Click"  CssClass="btn btn-success" runat="server" Text="Adicionar nova Pausa"/>
 
                            <asp:Repeater ID="updateRoutesRepeater"  OnItemDataBound="updateRoutesRepeater_ItemDataBound" runat="server">
                                <ItemTemplate>

                                     <div style="margin-top:17px; margin-bottom:17px;" class="flex-rows flex-rows-center-y">
                                       <label style=" margin:0; padding:0;margin-right:15px">Nova Pausa <%# (int)Eval("Id") + 1 %></label>
                                       <p style=" margin:0; padding:0; margin-right:10px; ">Paragem de linha?</p>
 
                                       <select id="UpdatingBreakStoppedLine" runat="server" class="form-control" style="max-width:85px" aria-label="Default select example">
                                           <option value="1" selected>Sim</option>
                                           <option value="0">Não</option>
                                       </select>
                                    </div>

                                    <div class="flex-rows flex-rows-center-y">
                                        <p style=" margin:0; padding:0; margin-right:10px; ">Inicio</p>
                                        <input class="form-control" runat="server" id="TimeStart" type="time" />

                                        <p style="margin:0;padding:0; margin-right:10px; margin-left:10px;">Fim</p>
                                        <input style="margin-right:10px;" class="form-control" runat="server" id="TimeEnd" type="time" />

                                        <asp:Button ID="RemoveUpdateBreakButton" OnClick="RemoveUpdateBreakButton_Click" runat="server" CssClass="btn btn-danger" Text="-" CommandArgument='<%# Eval("Id") %>' />
                                    </div> 

                                </ItemTemplate>
                            </asp:Repeater> 
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">

                    <div style="display:flex;flex-direction:row">    
                        <button type="button" style="margin-left:auto;margin-right:10px" class="btn btn-btn" data-dismiss="modal">Cancelar</button>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>                                    
                                <asp:button ID="Btn_UpdateRoute" runat="server"   class="btn btn-primary"   Text="Atualizar" OnClick="Btn_UpdateRoute_Click" OnClientClick=""/>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                    </div> 
                       
                </div>
            </div>
        </div>
   </div>

</asp:Content>

 