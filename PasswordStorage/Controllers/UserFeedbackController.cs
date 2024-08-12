using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordStorage.Helpers;

namespace PasswordStorage.Controllers
{
    [Authorize]
    public class UserFeedbackController : Controller
    {
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Feedback(string email, string description)
        {
            if (ModelState.IsValid)
            { 
                EmailHelper emailHelper = new EmailHelper();

                bool emailResponse = emailHelper.SendEmailUserFeedback(email, description);

                return RedirectToAction("FeedbackConfirmation");
            }   

            return View();
        }

        public IActionResult FeedbackConfirmation() 
        {
            return View();
        }
    }
}
