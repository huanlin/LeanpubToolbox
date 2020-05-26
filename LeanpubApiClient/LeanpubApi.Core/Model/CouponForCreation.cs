using System;
using System.Collections.Generic;
using System.Text;

namespace LeanpubApi.Core.Model
{
    public class CouponForCreation
    {
        public string coupon_code { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int? max_uses { get; set; }
        public bool suspended { get; set; }
        public double discounted_price { get; set; }
        public string note { get; set; }
    }
}
