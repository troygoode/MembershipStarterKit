# What is the Asp.Net MVC Membership Starter Kit?

The starter kit currently consists of two things:

1. A sample website containing the controllers, models, and views needed to administer users & roles.
2. A library that provides testable interfaces for administering users & roles and concrete implementations of those interfaces that wrap the built-in Asp.Net Membership & Roles providers.

Out of the box, the starter kit gives you the following features:

* List of Users
* List of Roles
* User Account Info
* Change Email Address
* Change a User's Roles

Some screenshots of the UI are [available on my blog](http://www.squaredroot.com/2009/08/07/mvcmembership-release-1-0/). While that blog post announced an older version of the project, the screenshots still accurately reflect the current version.

# How do I use it?

*Note:* If you have an ASP.Net MVC 1.0 project, you can convert it to ASP.NET MVC 2.0 following these
instructions:
[http://www.asp.net/learn/whitepapers/what-is-new-in-aspnet-mvc/#_TOC2](http://www.asp.net/learn/whitepapers/what-is-new-in-aspnet-mvc/#_TOC2)

## Add References to MvcMembership.dll
  
1. After getting the source code build it using your preferred IDE or using the included `Build.Debug.bat` or `Build.Release.bat` batch files.
2. Add a reference from the target site to `MvcMembership.dll`.

## Dependencies

1. The MvcMembership.dll depends upon the PagedList.dll assembly, which you can find packaged with the MvcMembership source code, in the bin of the SampleWebsite, or [downloaded from GitHub](http://github.com/TroyGoode/PagedList).

## Add the Provided MVC Area (Controller, Views, etc)
 
1. Copy the directory `SampleWebsite\Areas\UserAdministration` to `{targetSite}\Areas`. (If no "Areas" folder exists in your target site, you can just add one.)
2. Ensure your application registers areas on startup: `Application_Start` shold call `AreaRegistration.RegisterAllAreas()`.
3. Copy the file `SampleWebsite\Content\MvcMembership.css` to `{targetSite}\Content\`.

## Configure Membership & Roles Providers

1. Make sure you've configured your `web.config` properly for Membership and Roles. If you aren't sure of how to do this, take a look at the first two articles in [this series by Scott Mitchell at 4GuysFromRolla](http://www.4guysfromrolla.com/articles/120705-1.aspx). *Hint:* Use C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regsql.exe, and then grant your web site's application pool user access to the database.
2. Change the "Administrator" value of the `Authorize` attribute in the `UserAdministrationController.cs` file (line 11) to whatever role you want to require a user to have in order to view/use the User Administration area. If a user tries to navigate to the User Administration area without this role, they will be redirected to the login page (even if they're already logged in).
3. Configure the `system.net/mailSettings/smtp` node in your web.config file with the appropriate SMTP settings so that emails can be sent for password change/reset events.
4. Make sure the user identity of your application pool has sufficient permissions to the aspnet database.
5. Add the following code to your `global.asax` to keep the membership system updated with each user's last activity date:

```csharp
protected void Application_AuthenticateRequest()
{
    if(HttpContext.Current.User != null)
        Membership.GetUser(true);
}
```

## Integrate the Views

1. The starter kit relies on your site having a site master page. A default ASP.Net MVC site is generated with a `Site.Master` in the `\Views\Shared` folder. If you want to isolate something to the starter kit you could put it in `\Areas\UserAdministration\Views\Shared`.
2. That master page and any contained views will need to specify their Area when generating links, even views not in an area (so the default master page would requires fixes). If the link is not to a page in an area (typical), then an Area of "" (empty string) should be specified. For instance, a call to generate a link to the homepage should look like so:
    `Html.ActionLink("Home", "Index", "Home", new {Area = ""}, new {})`
3. Add a User Administration link to your master page (change "Administrator" to whatever role you want to use):

```erb
<% if (Roles.IsUserInRole("Administrator")){ %>
    <li><%= Html.ActionLink("User Administration", "Index", "UserAdministration", new { Area = "UserAdministration" }, new { }) %></li>
<% } %>
```