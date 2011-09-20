using System.Collections.Generic;
using System.Web.Security;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration
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
      public string StatusLabel 
      {
         get
         {
            switch (this.Status)
            {
               case (StatusEnum.Offline):
                  {
                     return Resources.Resources.Offline;
                  }
               case (StatusEnum.Online):
                  {
                     return Resources.Resources.Online;
                  }
               case (StatusEnum.LockedOut):
                  {
                     return Resources.Resources.LockedOut;
                  }
               case (StatusEnum.Unapproved):
                  {
                     return Resources.Resources.Unapproved;
                  }
               default:
                  {
                     return Resources.Resources.UnknownStatus;
                  }
            }
         }
      }
		public MembershipUser User { get; set; }
		public bool CanResetPassword { get; set; }
		public bool RequirePasswordQuestionAnswerToResetPassword { get; set; }
		public IDictionary<string, bool> Roles { get; set; }
		public bool IsRolesEnabled { get; set; }
	}
}