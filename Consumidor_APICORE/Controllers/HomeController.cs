using Consumidor_APICORE.Models;
using Consumidor_APICORE.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Consumidor_APICORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServicio_API _servicioApi;

        public HomeController(IServicio_API servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> Index()
        {
            List<Producto> Lista = await _servicioApi.Lista();
            
            return View(Lista);
        }

        public async Task<IActionResult> Producto(int idProducto)
        {
            Producto modelo_producto = new Producto();

            ViewBag.Accion = "Nuevo Producto";

            if(idProducto != 0)
            {
                modelo_producto = await _servicioApi.Obtener(idProducto);
                ViewBag.Accion = "Editar producto";
            }
            

            return View(modelo_producto);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Producto ob_producto)
        {
            bool respuesta;

            if(ob_producto.IdProducto == 0)
            {
                respuesta = await _servicioApi.Guardar(ob_producto);
            }
            else
            {
                respuesta = await _servicioApi.Editar(ob_producto);
            }

            //return respuesta ?  RedirectToAction("Index") :  NoContent();

            if(respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NoContent();//Se quedara en la vista sin realizar ningun tipo de acciom
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            var respuesta = await _servicioApi.Eliminar(idProducto);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NoContent(); //Se quedara en la vista sin realizar ningun tipo de acciom
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}