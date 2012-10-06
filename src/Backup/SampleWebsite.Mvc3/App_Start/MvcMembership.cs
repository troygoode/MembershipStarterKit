using System.Web.Mvc;
using MvcMembership;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SampleWebsite.Mvc3.App_Start.MvcMembership), "Start")]

namespace SampleWebsite.Mvc3.App_Start
{
	public static class MvcMembership
	{
		public static void Start()
		{
			GlobalFilters.Filters.Add(new TouchUserOnEachVisitFilter());
		}
	}
}