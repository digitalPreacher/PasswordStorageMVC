using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PasswordStorage.Data;
using PasswordStorage.Models;

namespace PasswordStorage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDAL _dal;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IDAL dal, SignInManager<ApplicationUser> signInManager)
        {

            _dal = dal; 
            _signInManager = signInManager;
        
        }

        [Route("SignUp")]
        public IActionResult SignUp()
        {

           return View();
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _dal.UserSignUpAsync(signUpModel);

            foreach (var errorMessage in result.Errors)
            {
                ModelState.AddModelError("", errorMessage.Description);
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
            var result =  await _dal.UserLoginAsync(signInModel);

            return RedirectToAction("Index", controllerName: "Containers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Containers");
        }
    }
}
