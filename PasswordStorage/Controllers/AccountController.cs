﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordStorage.Data;
using PasswordStorage.Helpers;
using PasswordStorage.Models;

namespace PasswordStorage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDAL _dal;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IDAL dal,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //[Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        //[Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _dal.UserSignUpAsync(signUpModel);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(signUpModel.LoginName, signUpModel.Password, false, false);
                    return RedirectToAction("Index", "Containers"); 
                }

                foreach (var errorMessage in result.Errors)
                {
                    
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }

            return View(signUpModel);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _dal.UserLoginAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Containers");
                }
                else
                {
                    ModelState.AddModelError("", "Некорретный логин или пароль");
                }
            }

            return View(signInModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _dal.GetUserByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                if(user != null)
                { 
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var domainName = Environment.GetEnvironmentVariable("DOMAIN_NAME");
                    var url = $"{domainName}{Url.Action("ResetPassword", "Account", new { code = token, email = user.Email })}";
                    var currentUserEmail = user.Email ?? throw new ArgumentException("Invalid email");

                    EmailHelper emailHelper = new EmailHelper();
                    bool emailResponse = emailHelper.SendEmailPasswordReset(currentUserEmail, url);
                    if (emailResponse)
                    {
                        return RedirectToAction("ForgotPasswordConfirmation", "Account");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Почта не найдена");
                }
            }

            return View(model);
        }

        public IActionResult ResetPassword(string code, string email)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var model = new ResetPasswordModel { Code = code, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dal.GetUserByEmailAsync(model.Email);

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

                foreach (var errorMessage in result.Errors)
                {

                    ModelState.AddModelError("", errorMessage.Description);
                }

                return RedirectToAction("ResetPasswordConfirmation");
            }

            return View(model); 
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AboutApplication()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}
