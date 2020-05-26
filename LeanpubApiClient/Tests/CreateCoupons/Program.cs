using System;
using System.IO;
using System.Threading.Tasks;
using LeanpubApi.Client;
using LeanpubApi.Core.Model;

namespace CreateCoupons
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine("CreateCoupons [bookID]");
                return;
            }
            var bookId = args[0];

            if (!File.Exists(".apikey"))
            {
                Console.WriteLine(".apikey file not found!");
                return;
            }

            var apiKey = (await File.ReadAllTextAsync(".apikey")).Trim();

            var client = new LeanpubApiClient(apiKey);

            var coupon = new CouponForCreation
            {
                coupon_code = "test1234",
                start_date = DateTime.Now.Date,
                end_date = DateTime.Now.AddYears(1).Date,
                max_uses = 1,
                discounted_price = 0.0,
                suspended = false,
                note = string.Empty
            };

            if (await client.CreateCouponAsync(bookId, coupon))
            {
                Console.WriteLine($"Coupon created: {coupon.coupon_code}");
            }
        }
    }
}
