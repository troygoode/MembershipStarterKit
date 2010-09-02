<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SampleWebsite.Areas.UserAdministration.Models.UserAdministration.DetailsViewModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	User Details: <%: Model.DisplayName %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

	<h2 class="mvcMembership">User Details: <%: Model.DisplayName %> [<% =Model.Status %>]</h2>

	<h3 class="mvcMembership">Account</h3>
	<div class="mvcMembership-account">
		<dl class="mvcMembership">
			<dt>User Name:</dt>
				<dd><%: Model.User.UserName %></dd>
			<dt>Email Address:</dt>
				<dd><a href="mailto:<%: Model.User.Email %>"><%: Model.User.Email %></a></dd>
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

	<h3 class="mvcMembership">Email Address & Comments</h3>
	<div class="mvcMembership-emailAndComments">
		<% using(Html.BeginForm("Details", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
		<fieldset>
			<p>
				<label for="email">Email Address:</label>
				<% =Html.TextBox("email", Model.User.Email) %>
			</p>
			<p>
				<label for="comments">Comments:</label>
				<% =Html.TextArea("comments", Model.User.Comment) %>
			</p>
			<input type="submit" value="Save Email Address and Comments" />
		</fieldset>
		<% } %>
	</div>

	<h3 class="mvcMembership">Password</h3>
	<div class="mvcMembership-password">
		<% if(Model.User.IsLockedOut){ %>
			<p>Locked out since <% =Model.User.LastLockoutDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></p>
			<% using(Html.BeginForm("Unlock", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<input type="submit" value="Unlock Account" />
			<% } %>
		<% }else{ %>

			<% if(Model.User.LastPasswordChangedDate == Model.User.CreationDate){ %>
			<dl class="mvcMembership">
				<dt>Last Changed:</dt>
				<dd><em>Never</em></dd>
			</dl>
			<% }else{ %>
			<dl class="mvcMembership">
				<dt>Last Changed:</dt>
				<dd><% =Model.User.LastPasswordChangedDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
			</dl>
			<% } %>

			<% if(Model.CanResetPassword && Model.RequirePasswordQuestionAnswerToResetPassword){ %>
				<% using(Html.BeginForm("ResetPasswordWithAnswer", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
				<fieldset>
					<p>
						<dl class="mvcMembership">
							<dt>Password Question:</dt>
							<% if(string.IsNullOrEmpty(Model.User.PasswordQuestion) || string.IsNullOrEmpty(Model.User.PasswordQuestion.Trim())){ %>
							<dd><em>No password question defined.</em></dd>
							<% }else{ %>
							<dd><%: Model.User.PasswordQuestion %></dd>
							<% } %>
						</dl>
					</p>
					<p>
						<label for="answer">Password Answer:</label>
						<% =Html.TextBox("answer") %>
					</p>
					<input type="submit" value="Reset to Random Password and Email User" />
				</fieldset>
				<% } %>
			<% }else if(Model.CanResetPassword){ %>
				<% using(Html.BeginForm("ChangePassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
				<fieldset>
					<p>
						<label for="password">New Password:</label>
						<% =Html.TextBox("password") %>
					</p>
					<input type="submit" value="Change Password" />
				</fieldset>
				<% } %>
				<% using(Html.BeginForm("ResetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
				<fieldset>
					<input type="submit" value="Reset to Random Password and Email User" />
				</fieldset>
				<% } %>
			<% } %>

		<% } %>
	</div>

	<h3 class="mvcMembership">Roles</h3>
	<div class="mvcMembership-userRoles">
		<ul class="mvcMembership">
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