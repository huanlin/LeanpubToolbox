using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanpubApi.Core.Model
{
    public class Coupon
    {
        public int id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string coupon_code { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int num_uses { get; set; }
        public int? max_uses { get; set; }
        public bool suspended { get; set; }
        public string note { get; set; }
        public bool is_url { get; set; }
        public int? couponable_id { get; set; }
        public string couponable_type { get; set; }
        public int? newsletter_id { get; set; }
        public string discounted_price { get; set; }
        public bool featured_in_newsletter { get; set; }
        public bool sponsored_in_newsletter { get; set; }

        public PackageDiscount[] package_discounts { get; set; }
    }

    public class PackageDiscount
    {
        public string package_slug { get; set; }
        public string discounted_price { get; set; }
    }
}
