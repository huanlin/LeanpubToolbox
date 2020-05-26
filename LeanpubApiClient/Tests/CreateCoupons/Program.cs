using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LeanpubApi.Client;
using LeanpubApi.Core.Model;

namespace CreateCoupons
{
    class Program
    {
        static string apiKey;
        static LeanpubApiClient apiClient;

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

            apiKey = (await File.ReadAllTextAsync(".apikey")).Trim();

            apiClient = new LeanpubApiClient(apiKey);

            await BatchCreateCoupons(bookId, "dinet-miniasp-jun", 11, 40);
        }

        public static async Task CreateCoupon(string bookId)
        {
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

            if (await apiClient.CreateCouponAsync(bookId, coupon))
            {
                Console.WriteLine($"Coupon created: {coupon.coupon_code}");
            }
        }

        public static async Task BatchCreateCoupons(string bookId, string couponPrefix, int startNumber, int count)
        {
            const string Characters = "23456789abcdefghjkmnpqrstuvwxyz";

            var r = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < count; i++)
            {
                int number = startNumber + i;
                var coupon = new CouponForCreation
                {
                    coupon_code = $"{couponPrefix}{number:00}-{GetRandomCode()}",
                    start_date = DateTime.Now.Date.AddDays(-1), // 確保立刻生效
                    end_date = DateTime.Now.AddYears(1).Date, // 一年內有效
                    max_uses = 1, // 只能使用一次
                    discounted_price = 0.0, // 零元結帳
                    suspended = false, // 立刻生效
                    note = string.Empty
                };

                if (await apiClient.CreateCouponAsync(bookId, coupon))
                {
                    Console.WriteLine($"Coupon created: {coupon.coupon_code}");
                }
            }

            string GetRandomCode()
            {
                var sb = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    int index = r.Next(0, Characters.Length - 1);
                    sb.Append(Characters[index]);
                }
                return sb.ToString();
            }
        }
    }
}
