  This project is based closely on the ASP.Net MVC Membership Starter Kit, which was published supporting ASP.Net MVC 1.0.  This project adds support for ASP.Net MVC 2.  The homepage for the original project is at http://mvcmembership.codeplex.com/.

  I verified these steps running on Windows Server 2008 R2.

  If you have an ASP.Net MVC 1.0 project, you can convert it to ASP.NET MVC 2.0 following these
instructions: http://www.asp.net/learn/whitepapers/what-is-new-in-aspnet-mvc/#_TOC2

  Follow these instructions to use the MVC Membership starter kit on an ASP.NET MVC 2 site:
  
   1. After getting the source code build it using your preferred IDE or using the included Build.Debug.bat or Build.Release.bat  batch files.
   2. Add a reference from the target site to MvcMembership.dll 
   3. Copy directories in MvcMembership\SampleWebSite\Areas\ to <targetSite>\Areas\.
   3.b.  If no "Areas" folder exists in your target site, you can just add one.
   4. Ensure your application registers areas on startup  
      Application_Start shold call AreaRegistration.RegisterAllAreas() 
      The routes should be registered before less specific routes.
      The routes registered match a single rule "Membership/{controller}/{action}/{id}"
   5. Copy path MvcMembership\SampleWebSite\Content\MvcMembership to <targetSite>\Content\MvcMembership.
   6. Make sure you’ve configured your web.config properly for Membership and Roles. If you aren’t sure of how to do this, take a look at the first two articles in this series by Scott Mitchell at 4GuysFromRolla.
      Hint: use C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regsql.exe, and then grant your web site's application pool user access to the database.
      Make sure the user identity of your application pool has sufficient permissions to the aspnet database (
   7. Add the following code to your global.asax to keep the membership system updated with each user’s last activity date:
   
        protected void Application_AuthenticateRequest()
        {
            if(User != null)
                Membership.GetUser(true);
        }
   8. The starter kit relies on your site having a site master page.
      A default ASP.Net MVC site is generated with a Site.Master
      Typically you'd have a Site.Master at \Views\Shared\, put if you want to isolate something to the starter kit you could put it at \Areas\MvcMembership\Views\Shared\
      That master page and any contained views will need to specify their Area when generating links, even views not in an area (so the default master page would requires fixes).
      If the link is not to a page in an area (typical), then an Area of "" (empty string) should be specified.
      For instance, a call to generate an action link:
        Html.ActionLink("Home", "Index", "Home")
      should instead specify a RouteDictionary with "Area" like so:
        Html.ActionLink("Home", "Index", "Home", new {Area = ""}, new { })
      so the link generation code knows not to make a link within that area.
        
   9. Add a Membership Administration link to your master page
   
      Html.ActionLink("User Administration", "Index", "UserAdministration", new { Area = "MvcMembershipImport" }, new { })
        