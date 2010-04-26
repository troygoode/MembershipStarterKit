  This project is based closely on the ASP.Net MVC Membership Starter Kit, which was published
supporting ASP.Net MVC 1.0.  This project adds support for ASP.Net MVC 2.  The homepage for the
original project is at http://mvcmembership.codeplex.com/.

  I verified these steps running on Windows Server 2008 R2.

  If you have an ASP.Net MVC 1.0 project, you can convert it to ASP.NET MVC 2.0 following these
instructions: http://www.asp.net/learn/whitepapers/what-is-new-in-aspnet-mvc/#_TOC2

  Follow these instructions to 
   1. After getting the source code build it using your preferred IDE or using the included Build.Debug.bat or Build.Release.bat  batch files.
   2. Add a reference from the target site to MvcMembership.dll 
   3. Copy the contents of the "Areas" fold from SampleWebsite to your site's "Areas" folder.
   3.b.  If no "Areas" folder exists in your target site, add an area then delete it.
         The "Areas" folder can be added by right-clicking on the project and selecting "Add Area", this will also create another area in the areas folder that you could delete.
         The "Areas" folder can be created in your project directly.
   3.c.  Ensure your application registers areas on startup
         The sample site calls AreaRegistration.RegisterAllAreas().  You can use the same call in the target site's Application_Start.
         The call should happen before less specific routing rules, so it should probably happen before the calls to register the other routes in your web application.
   4. Copy the MvcMembership folder from the SampleWebsite's Content folder to your app's Content folder.
   5. Make sure you’ve configured your web.config properly for Membership and Roles. If you aren’t sure of how to do this, take a look at the first two articles in this series by Scott Mitchell at 4GuysFromRolla.
      Hint: use C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regsql.exe, and then grant your web site's application pool user access to the database.
      Make sure the user identity of your application pool has sufficient permissions to the aspnet database (
   6. Add the following code to your global.asax to keep the membership system updated with each user’s last activity date:
   
        protected void Application_AuthenticateRequest()
        {
            if(User != null)
                Membership.GetUser(true);
        }
   7. Make sure you have master page Site.Master.  
      This code will need to be adapted so it works well when used from an Area.
      As far as I can tell, that means changing code that generates links to specify an Area explicitly.
      If the link is not to a page in an area (typical), then an Area of null should be specified.
      For instance, a call to generate an action link:
        Html.ActionLink("Home", "Index", "Home")
      would specify a RouteDictionary with "Area" like so:
        Html.ActionLink("Home", "Index", "Home", new {Area = null}, new { })
      because the Home controller is not in any area.
        
   8. Add a Membership Administration link to your master page
   
      Html.ActionLink("User Administration", "Index", "UserAdministration", new { Area = "MvcMembershipImport" }, new { })
      
      
  I had to do some additional steps as my old MVC project was set up to find Controller through an IoC container.  So changes were required to register the MvcMembership controllers with the container.  Expect some bumps I imagine other legacy projects will have other things to account for.
  