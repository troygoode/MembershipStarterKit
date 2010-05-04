<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IndexViewModel>" %>
<%@ Import Namespace="System.Globalization"%>
<%@ Import Namespace="SampleWebsite.Models.UserAdministration"%>
<%@ Import Namespace="PagedList"%>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	User Administration
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

    <h2>User Administration</h2>
    
    <h3>Users</h3>
    <div class="mvcMembership-allUsers">
    <% if(Model.Users.Count > 0){ %>
		<ul class="users">
			<% foreach(var user in Model.Users){ %>
			<li>
				<span class="username"><% =Html.ActionLink(user.UserName, "Details", new{ id = user.ProviderUserKey}) %></span>
				<span class="email"><a href="mailto:<% =Html.Encode(user.Email) %>"><% =Html.Encode(user.Email) %></a></span>
				<% if(user.IsOnline){ %>
					<span class="isOnline">Online</span>
				<% }else{ %>
					<span class="isOffline">Offline for
						<%
							var offlineSince = (DateTime.Now - user.LastActivityDate);
							if (offlineSince.TotalSeconds <= 60) Response.Write("1 minute.");
							else if(offlineSince.TotalMinutes < 60) Response.Write(Math.Floor(offlineSince.TotalMinutes) + " minutes.");
							else if (offlineSince.TotalMinutes < 120) Response.Write("1 hour.");
							else if (offlineSince.TotalHours < 24) Response.Write(Math.Floor(offlineSince.TotalHours) + " hours.");
							else if (offlineSince.TotalHours < 48) Response.Write("1 day.");
							else Response.Write(Math.Floor(offlineSince.TotalDays) + " days.");
						%>
					</span>
				<% } %>
				<% if(!string.IsNullOrEmpty(user.Comment)){ %>
					<span class="comment"><% =Html.Encode(user.Comment) %></span>
				<% } %>
			</li>
			<% } %>
		</ul>
		<ul class="paging">

			<% if (Model.Users.IsFirstPage){ %>
			<li>First</li>
			<li>Previous</li>
			<% }else{ %>
			<li><% =Html.ActionLink("First", "Index") %></li>
			<li><% =Html.ActionLink("Previous", "Index", new { index = Model.Users.PageIndex - 1 })%></li>
			<% } %>

			<li>Page <% =Model.Users.PageNumber%> of <% =Model.Users.PageCount%></li>

			<% if (Model.Users.IsLastPage){ %>
			<li>Next</li>
			<li>Last</li>
			<% }else{ %>
			<li><% =Html.ActionLink("Next", "Index", new { index = Model.Users.PageIndex + 1 })%></li>
			<li><% =Html.ActionLink("Last", "Index", new { index = Model.Users.PageCount - 1 })%></li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No users have registered.</p>
	<% } %>
	</div>

	<h3>Roles</h3>
	<div class="mvcMembership-allRoles">
	<% if(Model.Roles.Count() > 0 ){ %>
		<ul>
			<% foreach(var role in Model.Roles){ %>
			<li>
				<% =Html.ActionLink(role, "Role", new{id = role}) %>
				<% using(Html.BeginForm("DeleteRole", "UserAdministration", new{id=role})){ %>
				<input type="submit" value="Delete" />
				<% } %>
			</li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No roles have been created.</p>
	<% } %>
	<% using(Html.BeginForm("CreateRole", "UserAdministration")){ %>
		<fieldset>
			<label for="id">Role:</label>
			<% =Html.TextBox("id") %>
			<input type="submit" value="Create Role" />
		</fieldset>
	<% } %>
	</div>

</asp:Content>