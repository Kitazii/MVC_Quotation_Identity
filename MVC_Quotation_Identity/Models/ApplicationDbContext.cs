using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace MVC_Quotation_Identity.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Quotation> Quotations { get; set; }
        public ApplicationDbContext()
            : base("QuotationIdentityConnection", throwIfV1Schema: false)
        {

            Database.SetInitializer(new DatabaseInitialiser());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public class DatabaseInitialiser : DropCreateDatabaseAlways<ApplicationDbContext> 
        {
            protected override void Seed(ApplicationDbContext context)
            {
                if (!context.Users.Any())
                {
                    //Populating the Roles Table

                    //I need a rolemanager and a rolestore
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    //first check if the role doesn't exist
                    if (!roleManager.RoleExists("Admin"))
                    {
                        var admin = new IdentityRole("Admin"); //Create the role
                        var roleResult = roleManager.Create(admin); //storing the role in the database
                    }

                    if(!roleManager.RoleExists("Member"))
                    {
                        var member = new IdentityRole("Member");
                        var roleResult = roleManager.Create(member);
                    }

                    //Populating the Users Table
                    //creating the userManager
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    //Creating an admin and assign the admin to role

                    //before I create an admin I need to check if the user exists
                    string username = "admin@quotation.com";
                    string password = "12345678";

                    var user = userManager.FindByName(username);

                    if (user == null)
                    {
                        //create the applicationuser
                        var adminUser = new ApplicationUser
                        {
                            Forename = "Jim",
                            Surname = "Johnson",
                            UserName = username,
                            Email = username,
                            EmailConfirmed = true,
                            RegisteredAt = DateTime.Now
                        };

                        //store the user in the database
                        var userResult = userManager.Create(adminUser, password);

                        //assign user to role
                        if (userResult.Succeeded)
                        {
                            userManager.AddToRole(adminUser.Id, "Admin");
                        }

                    } //end of if for admin

                    Quotation quo1 = new Quotation
                    {
                        SalePrice = 100,
                        Discount = 10,
                        DiscountAmount = 10,
                        TotalPrice = 90
                    };

                    Quotation quo2 = new Quotation
                    {
                        SalePrice = 200,
                        Discount = 10,
                        DiscountAmount = 20,
                        TotalPrice = 80
                    };

                    List<Quotation> quotations = new List<Quotation>();
                    quotations.Add(quo1);
                    quotations.Add(quo2);

                    string username1 = "member@gmail.com";
                    string password1 = "123AndyStevenston";

                    //checking if the username does not exist in the database
                    var member1 = userManager.FindByName(username1);

                    if (member1 == null)
                    {
                        var memberUser = new ApplicationUser
                        {
                            Forename = "Andy",
                            Surname = "Stevenson",
                            UserName = username1,
                            Email = username1,
                            EmailConfirmed = true,
                            RegisteredAt = DateTime.Now,
                            Quotations = quotations
                        };

                        var memberResult = userManager.Create(memberUser, password1);

                        if (memberResult.Succeeded)
                        {
                            userManager.AddToRole(memberUser.Id, "Member");
                        }

                    } //end of if for member
                }

                base.Seed(context);
                context.SaveChanges();
            }
        }
    }
}