<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" rel="stylesheet" />

    <style>
        .image1 {
            margin-top: 50px;
            margin-bottom: 25px;
        }

        .password-wrapper {
            position: relative;
            width: 100%;
        }

        .password-toggle {
            position: absolute;
            top: 50%;
            right: 10px;
            transform: translateY(-50%);
            cursor: pointer;
        }

        #loginForm {
            margin-top: 25px;
            padding: 20px;
            width: 100%; /* Ensures the form takes up the full width */
            max-width: 500px; /* Optional: limits the maximum width */
        }

        .form-horizontal {
            width: 100%;
        }

        .form-group {
            margin-left: 0;
            width: 100%;
        }

        .checkbox {
            margin-left: 0;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('#togglePassword').on('click', function () {
                var passwordInput = $('#<%= Password.ClientID %>');
                var passwordFieldType = passwordInput.attr('type');

                if (passwordFieldType === 'password') {
                    passwordInput.attr('type', 'text');
                    $(this).html('<i class="fa fa-eye-slash"></i>');
                } else {
                    passwordInput.attr('type', 'password');
                    $(this).html('<i class="fa fa-eye"></i>');
                }
            });
        });
    </script>

    <asp:Image Width="500px" CssClass="image1" runat="server" ImageUrl="~/Images/Aptiv_logo.svg.png" />
    <h4>Use an APTIV account to log in.</h4>
    <hr />

    <section id="loginForm">
        <div class="form-horizontal">
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="UserName" CssClass="control-label">User name</asp:Label>
                <div style="Width:55%;margin-top:10px">
                    <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="text-danger" ErrorMessage="The user name field is required." />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
                <div style="Width:55%;margin-top:10px">
                    <div class="password-wrapper">
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        <span class="password-toggle" id="togglePassword">
                            <i class="fa fa-eye"></i>
                        </span>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                </div>
            </div>
            <div class="form-group">
                <div class="checkbox" style="margin-left:22px;">
                    <asp:CheckBox runat="server" ID="RememberMe" />
                    <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div>
                    <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
