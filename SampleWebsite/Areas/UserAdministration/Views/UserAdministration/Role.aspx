<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SampleWebsite.Areas.UserAdministration.Models.UserAdministration.RoleViewModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	Role: <% =Html.Encode(Model.Role) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

	<h2 class="mvcMembership">Role: <% =Html.Encode(Model.Role) %></h2>
	<div class="mvcMembership-roleUsers">
		<% if(Model.Users.Count() > 0){ %>
			<ul class="mvcMembership">
				<% foreach(var user in Model.Users){ %>
				<li>
					<% =Html.ActionLink(user.UserName, "Details", new{id=user.ProviderUserKey}) %>
					<% using(Html.BeginForm("RemoveFromRole", "UserAdministration", new{id = user.ProviderUserKey, role = Model.Role})){ %>
						<input type="submit" value="Remove From" />
					<% } %>
				</li>
				<% } %>
			</ul>
		<% }else{ %>
		<p>No users are in this role.</p>
		<% } %>
	</div>

</asp:Content>