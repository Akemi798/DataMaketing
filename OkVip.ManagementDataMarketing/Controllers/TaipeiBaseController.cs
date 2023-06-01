using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Account;

namespace OkVip.ManagementDataMarketing.Controllers
{
    [Authorize]
    public class TaipeiBaseController : Controller
    {
        // implement CurrentUser if needed (using singletone)
    }
}
