using API_MVC.Models;
using Contatos.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MVC.Controllers
{
    public class ContatoController : Controller
    {
        public ActionResult Index()
        {
            {
                ContatoViewModel vm = new ContatoViewModel
                {
                    ListaContatos = lista
                };

                if (System.IO.File.Exists("Dados.Txt"))
                {
                    lista = vm.ListaContatos = (List<ContatoModel>)Serializa.load("Dados.Txt");
                    string? maiorId = lista.Max(c => c.Id);
                    if (maiorId != null && maiorId != "")
                    {
                        ContatoModel.contador = int.Parse(maiorId) + 1;
                    }
                }
                return View(vm);
            }

        }
    }
}
