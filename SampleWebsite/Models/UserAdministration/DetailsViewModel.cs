using System.Collections.Generic;
using System.Web.Security;

namespace SampleWebsite.Models.UserAdministration
{
	public class DetailsViewModel
	{
		#region StatusEnum enum

		public enum StatusEnum
		{
			Offline,
			Online,
			LockedOut,
			Unapproved
		}

		#endregion

		public string DisplayName { get; set; }
		public StatusEnum Status { get; set; }
		public MembershipUser User { get; set; }
		public IDictionary<string, bool> Roles { get; set; }
	}
}