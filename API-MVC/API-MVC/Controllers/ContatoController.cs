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
        ContatoViewModel vm = new ContatoViewModel
        {
            ListaContatos = lista
        };

        try
        {
            if (System.IO.File.Exists(ArquivoDados))
            {
                lista = vm.ListaContatos = Serializa.load(ArquivoDados);

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
        String? nome = HttpContext.Session.GetString("ordena");
        if (nome == null)
        {
            return;
        }
        switch (nome)
        {
            case "Id": lista = lista.OrderBy(x => x.Id).ToList(); break;

            case "nome": lista = lista.OrderBy(x => x.Id).ToList(); break;

            case "email": lista = lista.OrderBy(x => x.Email).ToList(); break;

            case "celular": lista = lista.OrderBy(x => x.Email).ToList(); break;

            case "nascimento": lista = lista.OrderBy(x => x.Email).ToList(); break;

            case "cpf": lista = lista.OrderBy(x => x.Email).ToList(); break;
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(ContatoViewModel vm)
    {
        ModelState.Clear();

        try
        {
            if (!ValidarCPF(vm.NovoContato.CPF))
            {
                ModelState.AddModelError("NovoContato.Cpf", "CPF Inválido.");
            }
            else if (!DateTime.TryParse(vm.NovoContato.Data, out DateTime dt))
            {
                ModelState.AddModelError("NovoContato.Data", "Data inválida.");
            }
            else if (dt.CompareTo(DateTime.Now) > 0)
            {
                ModelState.AddModelError("NovoContato.Data", "Data inválida.");
            }
            else if (vm.NovoContato.Nome == "")
            {
                ModelState.AddModelError("NovoContato.Nome", "Nome do contato não pode estar em branco.");
            }
            else
            {
                vm.NovoContato.Nascimento = DateTime.Parse(vm.NovoContato.Data);

                ContatoModel procura = new ContatoModel("", vm.NovoContato.Nome, "", "", "", "", DateTime.Now);

                int indice = lista.BinarySearch(procura);

                if (indice >= 0)
                {
                    ModelState.AddModelError("NovoContato.Nome", $"Este nome, '{vm.NovoContato.Nome}' já está cadastrado.");
                }
                else
                {
                    // Verifica se o email já existe na lista
                    bool emailExiste = lista.Any(c => c.Email.Equals(vm.NovoContato.Email, StringComparison.OrdinalIgnoreCase));

                    if (emailExiste)
                    {
                        ModelState.AddModelError("NovoContato.Email", $"Este email, '{vm.NovoContato.Email}' já está cadastrado.");
                    }
                    else
                    {
                        vm.NovoContato.Id = (ContatoModel.contador++).ToString("D4");
                        lista.Add(vm.NovoContato);
                        lista.Sort();
                        Serializa.Save(lista, "Dados.txt");
                    }
                }
            }
            vm.ListaContatos = lista;

            if (ModelState.IsValid)
            {
                vm.NovoContato = new ContatoModel();
            }
        }
        catch (Exception)
        {
            //
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