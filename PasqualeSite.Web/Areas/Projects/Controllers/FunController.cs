using PasqualeSite.Web.Areas.Projects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Areas.Projects.Controllers
{
    public class FunController : Controller
    {
        // GET: Projects/Fun/HearthBuilder
        public ActionResult HearthBuilder()
        {
            return View();
        }

        public ActionResult HearthCalculator()
        {
            return View();
        }

        public ActionResult Stocks()
        {
            return View();
        }

        [ValidateInput(enableValidation:false)]
        [HttpPost]
        public FileContentResult ExportDeck(List<HearthCard> cards)
        {
            StringBuilder sb = new StringBuilder("Cost,Name,Attack,Health,Rarity");
            foreach (var card in cards)
            {
                sb.AppendFormat("\n\"{0}\",\"{1}\",{2},\"{3}\",\"{4}\"", card.Cost, card.Name, card.Attack, card.Health, card.Rarity);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "NewDeck.csv");
        }
    }
}