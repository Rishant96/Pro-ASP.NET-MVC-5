using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_EssentialTools.Models
{
    public interface IDiscountHelper
    {
        decimal ApplyDiscount(decimal totalparam);
    }

    public class DefaultDiscountHelper : IDiscountHelper
    {
        public decimal discountSize;

        public DefaultDiscountHelper(decimal discountParam)
        {
            discountSize = discountParam;
        }

        public decimal ApplyDiscount(decimal totalparam)
        {
            return (totalparam - (discountSize / 100m * totalparam));
        }
    }
}