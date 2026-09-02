using MessagePack;

namespace Contatos.Models;

[MessagePackObject(AllowPrivate = true)]
public class ContatoModel : IComparable<ContatoModel>
{
    [Key(0)]
    public string Id { get; set; } = "";

    [Key(1)]
    public string Nome { get; set; } = "";

    [Key(2)]
    public string Email { get; set; } = "";

    [Key(3)]
    public string Celular { get; set; } = "";

    [Key(4)]
    public string Nascimento { get; set; } = "";

    [Key(5)]
    public string CPF { get; set; } = "";

    public static int contador { get; set; } = 1;

    [SerializationConstructor]
    public ContatoModel(string id, string nome, string email, string celular, string nascimento, string cpf)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Celular = celular;
        Nascimento = nascimento;
        CPF = cpf;
    }

    public ContatoModel()
    {
    }

    int IComparable<ContatoModel>.CompareTo(ContatoModel? other)
    {
        if (Nome == null || other == null || other.Nome == null)
            return 0;

        return Nome.CompareTo(other.Nome);
    }
}

public class ContatoViewModel
{
    public ContatoModel NovoContato { get; set; } = new ContatoModel();
    public List<ContatoModel> ListaContatos { get; set; } = new List<ContatoModel>();
}
