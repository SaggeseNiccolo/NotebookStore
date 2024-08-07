﻿using System.Security.Claims;
using System.Security.Principal;

namespace NotebookStore.Business.Context
{
    public class ConsoleUserContext : IUserContext
    {
        public ConsoleUserContext()
        {
        }

        ClaimsPrincipal? IUserContext.GetCurrentUser()
        {
            GenericIdentity genericIdentity = new GenericIdentity("Admin");
            genericIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            ClaimsPrincipal c = new GenericPrincipal(genericIdentity, new string[] { "Admin" });
            return c;
        }
    }
}
