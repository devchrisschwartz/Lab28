using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Lab28.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie cookie = new HttpCookie("deck_id");
            if (Request.Cookies["deck_id"] == null)
            {
                HttpWebRequest apiShuffleRequest = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");

                apiShuffleRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

                HttpWebResponse apiShuffleResponse = (HttpWebResponse)apiShuffleRequest.GetResponse();

                if (apiShuffleResponse.StatusCode == HttpStatusCode.OK) // check if we got an http status of 200
                {
                    StreamReader responseData = new StreamReader(apiShuffleResponse.GetResponseStream());

                    string deck = responseData.ReadToEnd(); // reads the data from the response and stores it in a string

                    JObject jsonDeck = JObject.Parse(deck);

                    ViewBag.deckID = jsonDeck["deck_id"];
                    cookie["deck_id"] = jsonDeck["deck_id"].ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                cookie = Request.Cookies["deck_id"];
            }
            string temp = cookie["deck_id"];
            HttpWebRequest apiDrawRequest = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{temp}/draw/?count=5");

            apiDrawRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            HttpWebResponse apiDrawResponse = (HttpWebResponse)apiDrawRequest.GetResponse();

            if (apiDrawResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader responseData = new StreamReader(apiDrawResponse.GetResponseStream());

                string draw = responseData.ReadToEnd();

                JObject jsonDraw = JObject.Parse(draw);

                ViewBag.CardDraw = jsonDraw["cards"];
            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DrawNewCards(string deckid)
        {
            HttpWebRequest apiDrawRequest = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{deckid}/draw/?count=5");

            apiDrawRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            HttpWebResponse apiDrawResponse = (HttpWebResponse)apiDrawRequest.GetResponse();

            if (apiDrawResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader responseData = new StreamReader(apiDrawResponse.GetResponseStream());

                string draw = responseData.ReadToEnd();

                JObject jsonDraw = JObject.Parse(draw);

                ViewBag.CardDraw = jsonDraw["cards"];
            }

            ViewBag.deckID = deckid;

            return View("Index");

            //return RedirectToAction("Index");
        }
    }
}