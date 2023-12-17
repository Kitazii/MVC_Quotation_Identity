using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Quotation_Identity.Models
{
    public class Quotation
    {
        [Key]
        public int QuotationId { get; set; }
        [Display(Name = "Sale Price")]
        public double SalePrice { get; set; }
        public double Discount { get; set; }

        [Display(Name = "Discount Amount")]
        public double DiscountAmount { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        //Navigational properties - ONE TO MANY
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        //this method will calculate the discount amount
        public void CalculateDiscountAmount()
        {
            DiscountAmount = Discount * SalePrice / 100;
        }

        //this method will calculate the total price after discount
        public void CalculateTotalPrice()
        {
            TotalPrice = SalePrice - DiscountAmount;
        }
    }
}