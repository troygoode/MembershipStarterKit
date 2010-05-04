<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DetailsViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="SampleWebsite.Models.UserAdministration"%>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	User Details: <% =Html.Encode(Model.DisplayName) %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

    <h2>User Details: <% =Html.Encode(Model.DisplayName) %> [<% =Model.Status %>]</h2>

	<h3>Account</h3>
	<div class="mvcMembership-account">
		<dl>
			<dt>User Name:</dt>
				<dd><% =Html.Encode(Model.User.UserName) %></dd>
			<% if(Model.User.LastActivityDate == Model.User.CreationDate){ %>
			<dt>Last Active:</dt>
				<dd><em>Never</em></dd>
			<dt>Last Login:</dt>
				<dd><em>Never</em></dd>
			<% }else{ %>
			<dt>Last Active:</dt>
				<dd><% =Model.User.LastActivityDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
			<dt>Last Login:</dt>
				<dd><% =Model.User.LastLoginDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
			<% } %>
			<dt>Created:</dt>
				<dd><% =Model.User.CreationDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
		</dl>

		<% using(Html.BeginForm("ChangeApproval", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<% =Html.Hidden("isApproved", !Model.User.IsApproved) %>
			<input type="submit" value='<% =(Model.User.IsApproved ? "Unapprove" : "Approve") %> Account' />
		<% } %>
		<% using(Html.BeginForm("DeleteUser", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<input type="submit" value="Delete Account" />
		<% } %>
	</div>

	<h3>Email Address & Comments</h3>
	<div class="mvcMembership-emailAndComments">
		<% using(Html.BeginForm("Details", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
		<fieldset>
			<p>
				<label for="User_Email">Email Address:</label>
				<% =Html.TextBox("User.Email") %>
			</p>
			<p>
				<label for="User_Comment">Comments:</label>
				<% =Html.TextArea("User.Comment") %>
			</p>
			<input type="submit" value="Save Email Address and Comment" />
		</fieldset>
		<% } %>
	</div>

	<h3>Password</h3>
	<div class="mvcMembership-password">
		<% if(Model.User.IsLockedOut){ %>
			<p>Locked out since <% =Model.User.LastLockoutDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></p>
			<% using(Html.BeginForm("Unlock", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<input type="submit" value="Unlock Account" />
			<% } %>
		<% }else{ %>

			<% if(Model.User.LastPasswordChangedDate == Model.User.CreationDate){ %>
			<dl>
				<dt>Last Changed:</dt>
				<dd><em>Never</em></dd>
			</dl>
			<% }else{ %>
			<dl>
				<dt>Last Changed:</dt>
				<dd><% =Model.User.LastPasswordChangedDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
			</dl>
			<% } %>

			<% using(Html.BeginForm("ResetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<fieldset>
				<p>
					<dl>
						<dt>Password Question:</dt>
						<% if(string.IsNullOrEmpty(Model.User.PasswordQuestion) || string.IsNullOrEmpty(Model.User.PasswordQuestion.Trim())){ %>
						<dd><em>No password question defined.</em></dd>
						<% }else{ %>
						<dd><% =Html.Encode(Model.User.PasswordQuestion) %></dd>
						<% } %>
					</dl>
				</p>
				<p>
					<label for="answer">Password Answer:</label>
					<% =Html.TextBox("answer") %>
				</p>
				<input type="submit" value="Reset Password" />
			</fieldset>
			<% } %>

		<% } %>
	</div>

	<h3>Roles</h3>
	<div class="mvcMembership-userRoles">
		<ul>
			<% foreach(var role in Model.Roles){ %>
			<li>
				<% =Html.ActionLink(role.Key, "Role", new{id = role.Key}) %>
				<% if(role.Value){ %>
					<% using(Html.BeginForm("RemoveFromRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){ %>
					<input type="submit" value="Remove From" />
					<% } %>
				<% }else{ %>
					<% using(Html.BeginForm("AddToRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){ %>
					<input type="submit" value="Add To" />
					<% } %>
				<% } %>
			</li>
			<% } %>
		</ul>
		</div>

</asp:Content>