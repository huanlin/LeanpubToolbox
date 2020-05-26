using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeanpubApi.Core.Model;
using Newtonsoft.Json;

namespace LeanpubApi.Client
{
    public class LeanpubApiClient
    {
        private static HttpClient _httpClient = new HttpClient();

        private string _apiKey;
        private string _couponTemplateJson;

        const string CouponJsonTemplateFileName = "coupon_template.txt";

        public LeanpubApiClient(string apiKey)
        {
            _apiKey = apiKey;

            if (File.Exists(CouponJsonTemplateFileName))
            {
                _couponTemplateJson = File.ReadAllText(CouponJsonTemplateFileName);
            }
        }

        public async Task<List<Coupon>> GetActiveCouponsAsync(string bookId)
        {
            return await GetCouponsAsync(bookId, (c) => 
            {
                if (c.num_uses >= c.max_uses)
                {
                    return false;
                }

                DateTime endDate;
                if (DateTime.TryParse(c.end_date, out endDate))
                {
                    if (endDate < DateTime.Now.Date)
                    {
                        return false;
                    }
                }
                return true; 
            });
        }

        public async Task<List<Coupon>> GetCouponsAsync(string bookId, Func<Coupon, bool> predicate = null)
        {
            var url = $"https://leanpub.com/{bookId}/coupons.json?api_key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var coupons = JsonConvert.DeserializeObject<List<Coupon>>(json);
                if (predicate == null)
                {
                    return coupons.ToList();
                }
                return coupons.Where(predicate).ToList();
            }
            return new List<Coupon>();
        }

        public async Task<bool> CreateCouponAsync(string bookId, CouponForCreation coupon)
        {
            if (string.IsNullOrWhiteSpace(_couponTemplateJson))
            {
                throw new Exception($"Coupon JSON template file not loaded: {CouponJsonTemplateFileName}");
            }

            var url = $"https://leanpub.com/{bookId}/coupons.json?api_key={_apiKey}";

            var jsonString = _couponTemplateJson
                .Replace("{{coupon_code}}", coupon.coupon_code)
                .Replace("{{discounted_price}}", $"{coupon.discounted_price:0.0}")
                .Replace("{{start_date}}", $"{coupon.start_date:yyyy-MM-dd}")
                .Replace("{{end_date}}", $"{coupon.end_date:yyyy-MM-dd}")
                .Replace("{{max_uses}}", $"{coupon.max_uses}");

            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            var msg = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed: {msg}");
        }
    }
}