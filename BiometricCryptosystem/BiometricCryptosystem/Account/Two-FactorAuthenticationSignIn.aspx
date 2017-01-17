<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Two-FactorAuthenticationSignIn.aspx.cs" Inherits="BiometricCryptosystem.Account.Two_FactorAuthenticationSignIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Authentication Failed! Try again." Visible="False"></asp:Label>
    <br />
    Select File To Upload:<br />
    <br />
    <asp:FileUpload ID="FileUpload1" runat="server" />



    <asp:Button ID="UploadBtn" Text="Upload Me!" OnClick="UploadBtn_Click" runat="server"></asp:Button>

    <br />
    <br />

    <br />
    <asp:TextBox ID="log" runat="server" Height="219px" Width="473px" TextMode="MultiLine"></asp:TextBox>
    <asp:TextBox ID="matlabLog" runat="server" Height="224px" Width="504px" TextMode="MultiLine"></asp:TextBox>
    <br />
    <asp:Button ID="matlabBtn" Text="Run Matlab!" OnClick="MatlabBtn_Click" runat="server"></asp:Button>
    <br />
    <br />
    <br />
    <asp:Image ID="Image1" runat="server" Height="480px" Width="463px" />
</asp:Content>
