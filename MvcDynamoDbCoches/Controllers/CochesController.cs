using Microsoft.AspNetCore.Mvc;
using MvcDynamoDbCoches.Models;
using MvcDynamoDbCoches.Services;

namespace MvcDynamoDbCoches.Controllers
{
    public class CochesController : Controller
    {
        private ServiceDynamoDb service;

        public CochesController(ServiceDynamoDb service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Coche> cars = await this.service.GetCochesAsync();
            return View(cars);
        }

        public async Task<IActionResult> Details(int idcoche)
        {
            Coche car = await this.service.FindCocheAsync(idcoche);
            return View(car);
        }

        public async Task<IActionResult> Delete(int idcoche)
        {
            await this.service.DeleteCocheAsync(idcoche);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Coche car)
        {
            await this.service.CreateCocheAsync(car);
            return RedirectToAction("Index");
        }
    }
}
