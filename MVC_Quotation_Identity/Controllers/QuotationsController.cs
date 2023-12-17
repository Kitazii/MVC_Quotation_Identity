using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Quotation_Identity.Models;
using System.Data.Entity;

namespace MVC_Quotation_Identity.Controllers
{
    [Authorize(Roles = "Admin, Member")]
    public class QuotationsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Quotations
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var quotations = context.Quotations.Include(q=>q.User);

            return View(quotations.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Quotation model)
        {
            if (ModelState.IsValid)
            {
                //upadate the model
                model.CalculateDiscountAmount();
                model.CalculateTotalPrice();
                model.UserId = User.Identity.GetUserId(); //gets the id of the current logged in user
                context.Quotations.Add(model);
                context.SaveChanges();
                return View(model);
            }

            return View();
        }

        //this action will list all the quotations that belong to a user
        [Authorize(Roles = "Member")]
        public ActionResult ListOwnQuotations()
        {
            //get user id
            string userId = User.Identity.GetUserId();

            List<Quotation> quotations = context.Quotations.Where(q => q.UserId.Equals(userId)).ToList(); //select the quotations that belong to the logged in user
            List<Quotation> quotationsVM = new List<Quotation>();

            foreach(Quotation q in quotations)
            {
                ApplicationUser user = context.Users.Find(q.UserId);
                q.User = user;
                quotationsVM.Add(q);
            }

            return View(quotationsVM);
        }
    }
}