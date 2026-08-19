using API_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MVC.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            ListaContatoModel Modelo = new ListaContatoModel();
            Modelo.Lista.Sort();
            return View(Modelo);
        }
    }
}
