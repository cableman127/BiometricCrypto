<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnrollTwo-FactorAuthentication.aspx.cs" Inherits="BiometricCryptosystem.Account.EnrollTwo_FactorAuthentication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Enrollment in Two-Factor Authentication has failed! Please try again." Visible="False"></asp:Label>
        <br />
        Enter A Friendly Name
        <input id="txtImgName" type="text" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtImgName"></asp:RequiredFieldValidator>
        <br>
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
        <asp:Image ID="Image1" runat="server" Height="480px" Width="463px" /><asp:Image ID="Image2" runat="server" Height="480px" Width="463px" />
</asp:Content>
