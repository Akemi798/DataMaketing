using Google.Authenticator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Authentication;
using static QRCoder.PayloadGenerator.WiFi;

namespace OkVip.ManagementDataMarketing.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<TaipeiUser> _userManager;
        private readonly SignInManager<TaipeiUser> _signInManager;

        public AuthenticationController(
            SignInManager<TaipeiUser> signInManager,
            UserManager<TaipeiUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user != null && user.IsActivated)
                //{
                //    var validateTfa = ValidateTwoFactorPIN(model.Pin, user.GoogleAuthenticatorSecretCode);
                //    if (!validateTfa)
                //    {
                //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //        return View(model);
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return View(model);
                //}


                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "DashBoard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            var setupInfo = twoFactor.GenerateSetupCode(
                TfaConstants.ISSUER,
                user.Email,
                Encoding.ASCII.GetBytes(user.GoogleAuthenticatorSecretCode)
            );

            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                QrCode = setupInfo.QrCodeSetupImageUrl,
                ManualEntryKey = setupInfo.ManualEntryKey
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
                var setupInfo = twoFactor.GenerateSetupCode(
                    TfaConstants.ISSUER,
                    user.Email,
                    Encoding.ASCII.GetBytes(user.GoogleAuthenticatorSecretCode)
                );

                model = new ChangePasswordViewModel()
                {
                    QrCode = setupInfo.QrCodeSetupImageUrl,
                    ManualEntryKey = setupInfo.ManualEntryKey
                };


                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData[OkVip.ManagementDataMarketing.Commons.Constants.TempDataConstants.TEMP_DATA_INFO_MESSAGE] = "ĐỔI PASSWORD THÀNH CÔNG";
            return View(model);
        }

        #region helpers

        private bool ValidateTwoFactorPIN(string pin, string code)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(code, pin);
        }

        #endregion
    }
}
