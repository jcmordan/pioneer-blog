﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pioneer.Blog.Model.Views;
using Pioneer.Blog.Service;

namespace Pioneer.Blog.Controllers.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactController : Controller
    {
        private readonly ICommunicationService _communicationService;

        public ContactController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        /// <summary>
        /// GET: Contact
        /// </summary>
        public IActionResult Index()
        {
            ViewBag.Description = "Contact Chad Ramos at Pioneer Code. .NET, C#, The Web, Open Source, Programming and more.";
            ViewBag.IsValid = true;
            ViewBag.Selected = "contact";
            return View();
        }


        // POST: /Contact/send
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Send(ContactViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsValid = false;
                return View("~/Views/Contact/Index.cshtml");
            }

            //var response = _communicationService.SendContactEmailNotification(model);
            //ViewBag.IsValid = true;
            //ViewBag.Selected = "contact";

            //switch (response.Status)
            //{
            //    case OperationStatus.Ok:
            //        ViewBag.MessageSent = true;
            //        break;
            //    case OperationStatus.Error:
            //        ViewBag.IsValid = false;
            //        ModelState.AddModelError("", "Sorry, we had an issue with sending your email. Please try again later. ");
            //        break;
            //}

            return View("~/Views/Contact/Index.cshtml");
        }
    }
}