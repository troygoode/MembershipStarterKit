<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Error: Duplicate Role Found
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Error: Duplicate Role Found</h2>
    <p>Duplicate role found by <em><% =Model.ControllerName %>.<% =Model.ActionName %></em>.</p>
    <p>This exception is thrown when the role provider returns a list of roles containing duplicates. If you are using the default Asp.Net Role Provider and database, check your <strong>ASPNETDB</strong> database's <strong>aspnet_Roles</strong> table for duplicates in the <strong>RoleName</strong> column.</p>

</asp:Content>