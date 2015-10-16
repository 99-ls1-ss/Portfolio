using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Portfolio.Controllers {
    [RequireHttps]
    public class ContactController : Controller {
        // GET: Contact
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string contactName, string contactEmail, string contactSubject, string contactMessage) {
            
            EmailService emailService = new EmailService();
            IdentityMessage identityMessage = new IdentityMessage();
            identityMessage.Subject = contactSubject;
            identityMessage.Body = "From : " + contactName + "<br />Email: " + contactEmail + "<br /><br />Email Message: " + contactMessage;
            identityMessage.Destination = ConfigurationManager.AppSettings["FromEmail"];

            await emailService.SendAsync(identityMessage);
            return RedirectToAction("Index");

        }
    }
}