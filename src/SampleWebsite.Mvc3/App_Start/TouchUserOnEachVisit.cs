using System.Web.Mvc;
using MvcMembership;
using SampleWebsite.Mvc3.App_Start;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (TouchUserOnEachVisit), "Start")]

namespace SampleWebsite.Mvc3.App_Start
{
	public static class TouchUserOnEachVisit
	{
		public static void Start()
		{
			GlobalFilters.Filters.Add(new TouchUserOnEachVisitFilter(()=> new AspNetMembershipProviderWrapper()));
		}
	}
}