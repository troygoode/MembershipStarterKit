using System;
using System.Web.Mvc;

namespace MvcMembership
{
	public class TouchUserOnEachVisitFilter : ActionFilterAttribute
	{
		private readonly Func<IUserService> _userServiceFactory;
		private IUserService _userService;

		private IUserService UserService
		{
			get { return _userService ?? (_userService = _userServiceFactory()); }
		}

		public TouchUserOnEachVisitFilter(Func<IUserService> userServiceFactory)
		{
			_userServiceFactory = userServiceFactory;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var user = filterContext.RequestContext.HttpContext.User;
			if (user.Identity.IsAuthenticated)
				UserService.Touch(user.Identity.Name);
		}
	}
}