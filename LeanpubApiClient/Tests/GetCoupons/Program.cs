using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using LeanpubApi.Core.Model;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LeanpubApi.Client;

namespace GetCoupons
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine("GetCoupons [bookID]");
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
            var coupons = await client.GetActiveCouponsAsync(bookId);

            foreach (var coupon in coupons)
            {
                Console.WriteLine("https://leanpub.com/dinet/c/" + coupon.coupon_code);
            }
        }
    }
}
