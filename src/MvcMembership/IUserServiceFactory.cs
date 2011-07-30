using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcMembership
{
	public interface IUserServiceFactory
	{
		IUserService Make();
	}
}
