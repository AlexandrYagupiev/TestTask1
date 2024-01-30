using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("ListOfApplications")]
    public class ListOfApplicationsController : Controller
    {
        [HttpPost("CreateNewRequest")]
        public IActionResult CreateNewRequest(string url, string mail)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Subscriptiont subscription = new Subscriptiont { url = url, mail = mail };
                    db.Sublist.Add(subscription);
                    db.SaveChanges();
                }
                return this.Ok("Подписка на квартиру оформлена");
            }
            catch
            {
                return this.BadRequest("Произошла ошибка оформления");
            }
        }
        [HttpGet("GetListApartments")]
        public async Task<IActionResult> GetListApartments(string mail)
        {
            try
            { 
                List<ReturnSubscription> returnSubscriptionList = new List<ReturnSubscription>();
                var pricesMinList = new List<string>();
                var pricesMaxList = new List<string>();               
                using (ApplicationContext db = new ApplicationContext())
                {
                    var applications = db.Sublist.Where(p => p.mail == mail).ToList();
                    foreach (var app in applications) 
                    {
                        var newReturnSubscription = new ReturnSubscription();
                        newReturnSubscription.url = app.url;
                        newReturnSubscription.pricesMin = await GetСurrentPricesMin(app.url);
                        newReturnSubscription.pricesMax = await GetСurrentPricesMax(app.url);
                        returnSubscriptionList.Add(newReturnSubscription);
                    }
                }
                return this.Ok(returnSubscriptionList);
            }
            catch
            {
                return this.BadRequest("Произошла ошибка получения списка актуальных цен");
            }
        }
        static HttpClient httpClient = new HttpClient();
        static async Task<string> GetСurrentPricesMin(string apartmensURL)
        {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apartmensURL);
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string contentHtml = await response.Content.ReadAsStringAsync();
                var pricesMin = contentHtml.Substring(contentHtml.IndexOf("range-slider__input-field min j-range-slider-input") + 150, 100);
                pricesMin = Regex.Replace(pricesMin, @"[ \r\n\t]", "").Replace("value=\"", "").Replace("\">", "");             
                return pricesMin;
        }

        static async Task<string> GetСurrentPricesMax(string apartmensURL)
        {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apartmensURL);
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string contentHtml = await response.Content.ReadAsStringAsync();
                var pricesMax = contentHtml.Substring(contentHtml.IndexOf("range-slider__input-field max j-range-slider-input") + 150, 100);
                pricesMax = Regex.Replace(pricesMax, @"[ \r\n\t]", "").Replace("value=\"", "").Replace("\">", "");
                return pricesMax;
        }
    }
}
