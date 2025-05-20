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

        public async Task<IActionResult> Index(string? filtro)
        {
            List<Coche> cars;

            if (!string.IsNullOrEmpty(filtro))
            {
                cars = await this.service.BuscarCochesAsync(filtro);
            }
            else
            {
                cars = await this.service.GetCochesAsync();
            }

            return View(cars);
        }

        public async Task<IActionResult> Details(string idcoche)
        {
            Coche car = await this.service.FindCocheAsync(idcoche);
            return View(car);
        }

        public async Task<IActionResult> Delete(string idcoche)
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
            // Asegura que el ID sea string válido
            car.IdCoche = Guid.NewGuid().ToString();

            // Verificamos si se ha informado el motor
            if (car.Motor != null)
            {
                bool motorVacio = string.IsNullOrEmpty(car.Motor.Tipo)
                    && car.Motor.Caballos == 0
                    && car.Motor.Potencia == 0;

                if (motorVacio)
                {
                    car.Motor = null;
                }
            }

            await this.service.CreateCocheAsync(car);
            return RedirectToAction("Index");
        }
    }
}
