namespace Datatables.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DatatablesController : Controller
    {
        [HttpGet]
        public IActionResult CountryListClientSide()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CountryListServerSide()
        {
            return View();
        }
    }
}