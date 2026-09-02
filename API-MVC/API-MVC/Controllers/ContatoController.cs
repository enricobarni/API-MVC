using Contatos.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.Controllers;

public class ContatoController : Controller
{
    private static List<ContatoModel> lista = new List<ContatoModel>();
    private readonly IWebHostEnvironment _env;

    public ContatoController(IWebHostEnvironment env)
    {
        _env = env;
    }

    private string ArquivoDados => Path.Combine(_env.ContentRootPath, "Dados.txt");

    public ActionResult Index()
    {
        try
        {
            if (System.IO.File.Exists(ArquivoDados))
            {
                lista = Serializa.load(ArquivoDados);

                if (lista.Count > 0)
                {
                    string? maiorId = lista.Max(c => c.Id);

                    if (!string.IsNullOrEmpty(maiorId))
                        ContatoModel.contador = int.Parse(maiorId) + 1;
                }
            }
        }
        catch
        {
            // Mantém o comportamento demonstrado no material.
        }

        Ordenar();

        ContatoViewModel vm = new ContatoViewModel
        {
            ListaContatos = lista
        };

        return View(vm);
    }

    public ActionResult Ordena(string nome)
    {
        HttpContext.Session.SetString("ordena", nome); // Coloca na "sessão" "ordena" o texto recebido no ‘nome’
        Ordenar();
        return RedirectToAction("Index");

    }

    public void Ordenar()
    {
        string? nome = HttpContext.Session.GetString("ordena");
        if (nome == null)
        {
            return;
        }
        switch (nome)
        {
            case "id": lista = lista.OrderBy(x => x.Id).ToList(); break;

            case "nome": lista = lista.OrderBy(x => x.Nome).ToList(); break;

            case "email": lista = lista.OrderBy(x => x.Email).ToList(); break;

            case "celular": lista = lista.OrderBy(x => x.Celular).ToList(); break;

            case "telefone": lista = lista.OrderBy(x => x.Telefone).ToList(); break;

            case "cpf": lista = lista.OrderBy(x => x.CPF).ToList(); break;

            case "nascimento": lista = lista.OrderBy(x => x.Nascimento).ToList(); break;
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(ContatoViewModel vm)
    {
        try
        {
            if (!String.IsNullOrWhiteSpace(vm.NovoContato.IdBusca))
            {
                vm.selecaoId = int.Parse("0" + vm.NovoContato.IdBusca).ToString("D4");
                vm.ListaContatos = lista;
                bool existe = lista.Any(x => x.Id == vm.selecaoId);
                if (!existe)
                {
                    TempData["ErrosModal"] = "Nenhum contato encontrado com este ID !";// (Vamos tratar isso daqui a pouco)                    
                }
                vm.NovoContato.IdBusca = "";
                return View(vm);
            }
            vm.NovoContato.Id = (ContatoModel.contador++).ToString("D4");
            lista.Add(vm.NovoContato);
            lista.Sort();
            Serializa.Save(lista, "Dados.txt");
            vm.NovoContato = new ContatoModel();
            vm.ListaContatos = lista;
        }
        catch (Exception)
        {

        }

        return View(vm);
    }

    // MÉTODO PARA VALIDAR CPF
    private bool ValidarCPF(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        // Impede CPFs como 11111111111
        if (cpf.Distinct().Count() == 1)
            return false;

        // PRIMEIRO DÍGITO
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (10 - i);
        }

        int resto = soma % 11;

        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (digito1 != int.Parse(cpf[9].ToString()))
            return false;

        // SEGUNDO DÍGITO
        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * (11 - i);
        }

        resto = soma % 11;

        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (digito2 != int.Parse(cpf[10].ToString()))
            return false;

        return true;
    }
}