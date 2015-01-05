﻿using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class RegisterEmployeeController : ApiController
    {
        public List<RegisterEmployee> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DARegisters.Load_RegisterEmployee(principal.Claims);
        }
        public RegisterEmployee Get(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DARegisters.Load_RegisterEmployee(id, principal.Claims);
        }
    }
}