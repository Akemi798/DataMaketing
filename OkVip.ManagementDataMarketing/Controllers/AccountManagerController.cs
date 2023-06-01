using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Account;
using OkVip.ManagementDataMarketing.Services;
using OkVip.ManagementDataMarketing.Controllers;

namespace OkVip.ManagementDataMarketing.Controllers
{
    [Authorize(Roles = RoleConstants.ADMIN_ROLE_NAME)]
    public class AccountManagerController : TaipeiBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountManagerController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            try
            {
                return View(_accountService.GetAll());
            }
            catch (Exception ex)
            {
                TempData[TempDataConstants.TEMP_DATA_ERROR_MESSAGE] = ex.Message;
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.Create(viewModel);
                    TempData[TempDataConstants.TEMP_DATA_INFO_MESSAGE] = "Tạo mới user \"" + viewModel.UserName + "\" thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            
            return View(viewModel);
        }

        public IActionResult Update(string id)
        {
            if (id != null)
            {
                try
                {
                    var user = _accountService.GetById(id);
                    return View(_mapper.Map<UpdateAccountViewModel>(user));
                }
                catch (Exception ex)
                {
                    TempData[TempDataConstants.TEMP_DATA_ERROR_MESSAGE] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(UpdateAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.Update(viewModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(viewModel);
                }
                TempData[TempDataConstants.TEMP_DATA_INFO_MESSAGE] = "Cập nhật user \"" + viewModel.UserName + "\" thành công";
                return RedirectToAction("Index");
            }
            return View(viewModel);

        }
    }
}
