using AutoMapper;
using Google.Authenticator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Commons;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Data;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Account;

namespace OkVip.ManagementDataMarketing.Services
{
    public interface IAccountService
    {
        public List<AccountViewModel> GetAll();
        public AccountViewModel GetById(string id);
        public void Create(CreateAccountViewModel model);
        public void Update(UpdateAccountViewModel model);
    }

    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<TaipeiUser> _userManager;
        public AccountService(ApplicationDbContext db, IMapper mapper, UserManager<TaipeiUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
        }

        public List<AccountViewModel> GetAll()
        {
            List<TaipeiUser> users = _db.User.AsNoTracking().ToList();
            List<AccountViewModel> result = new List<AccountViewModel>();
            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            
            for (int i = 0; i < users.Count; i++)
            {
                AccountViewModel resultItem = _mapper.Map<AccountViewModel>(users[i]);
                var setupInfo = twoFactor.GenerateSetupCode(
                    TfaConstants.ISSUER,
                    users[i].Email,
                    Encoding.ASCII.GetBytes(users[i].GoogleAuthenticatorSecretCode)
                );

                var isAdmin = _userManager.IsInRoleAsync(users[i], RoleConstants.ADMIN_ROLE_NAME);
                isAdmin.Wait();

                resultItem.IsAdmin = isAdmin.Result;
                resultItem.QrCode = setupInfo.QrCodeSetupImageUrl;
                result.Add(resultItem);
            }

            return result;
        }

        public void Create(CreateAccountViewModel model)
        {
            TaipeiUser user = _mapper.Map<TaipeiUser>(model);
            user.GoogleAuthenticatorSecretCode = Guid.NewGuid().ToString().Split("-")[4];

            var createTask = _userManager.CreateAsync(user, model.Password);
            createTask.Wait();
            if (!createTask.Result.Succeeded)
            {
                throw new InvalidOperationException(createTask.Result.Errors.FirstOrDefault().Description);
            }

            if (model.IsAdmin)
            {
                var updateRoleTask = _userManager.AddToRoleAsync(user, RoleConstants.ADMIN_ROLE_NAME);
                updateRoleTask.Wait();
                if (!updateRoleTask.Result.Succeeded)
                {
                    throw new InvalidOperationException(updateRoleTask.Result.Errors.FirstOrDefault().Description);
                }
            }

        }

        public AccountViewModel GetById(string id)
        {
            TaipeiUser user = _db.User.AsNoTracking().FirstOrDefault(item => item.Id == id);
            AccountViewModel result = _mapper.Map<AccountViewModel>(user);
            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();

            
            var setupInfo = twoFactor.GenerateSetupCode(
                TfaConstants.ISSUER,
                user.Email,
                Encoding.ASCII.GetBytes(user.GoogleAuthenticatorSecretCode)
            );

            var isAdmin = _userManager.IsInRoleAsync(user, RoleConstants.ADMIN_ROLE_NAME);
            isAdmin.Wait();

            result.IsAdmin = isAdmin.Result;
            result.QrCode = setupInfo.QrCodeSetupImageUrl;
            result.ManualEntryKey = setupInfo.ManualEntryKey;
            return result;
        }


        public void Update(UpdateAccountViewModel model)
        {
            TaipeiUser dbModel = _db.User.FirstOrDefault(item => item.Id == model.Id);
            dbModel.UpdateDate = DateTime.Now;
            dbModel.IsActivated = model.IsActivated;
            if (model.Password != null
                && model.Password.Length > 0)
            {
                dbModel.PasswordHash = Functions.PassGenerate(dbModel, model.Password);
            }

            if (model.IsAdmin)
            {
                _userManager.AddToRoleAsync(dbModel, RoleConstants.ADMIN_ROLE_NAME).Wait();
            }
            else
            {
                _userManager.RemoveFromRoleAsync(dbModel, RoleConstants.ADMIN_ROLE_NAME).Wait();
            }

            var updateResult = _userManager.UpdateAsync(dbModel);
            updateResult.Wait();

            if (!updateResult.Result.Succeeded)
            {
                throw new InvalidOperationException(updateResult.Result.Errors.FirstOrDefault().Description);
            }
        }

    }
}
