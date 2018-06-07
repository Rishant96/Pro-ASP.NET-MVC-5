using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_EssentialTools.Models
{
    public class FlexibleDiscountHelper : IDiscountHelper
    {
        public decimal ApplyDiscount(decimal totalparam)
        {
            decimal discount = totalparam > 100 ? 70 : 25;
            return (totalparam - (discount / 100m * totalparam));
        }
    }
}