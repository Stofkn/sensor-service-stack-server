using System.Web.Mvc;
using ServiceImplementation;

namespace SensingControlWebService.Controllers
{
    public class HomeController : Controller {

        private SensorsService service = new SensorsService();

        // Single page web app

        // GET: /Home/

        public ActionResult Index(){
            ViewBag.lastTemperature = SensorsService.LastTemperature.LastValue.ToString("0.00").Replace(',', '.');
            ViewBag.lastHumidity = SensorsService.LastHumidity.LastValue.ToString("0.00").Replace(',', '.');
            ViewBag.lastCo2 = SensorsService.LastCo2.LastValue.ToString("0.00").Replace(',', '.');
            return View();
        }

    }
}
