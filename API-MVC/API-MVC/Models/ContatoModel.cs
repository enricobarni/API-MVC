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
    public string Telefone { get; set; } = "";

    [Key(5)]
    public string CPF { get; set; } = "";

    [Key(6)]
    public DateTime Nascimento { get; set; } = new DateTime();

    [IgnoreMember]
    public String Data { get; set; } = "";

    [IgnoreMember] // Para não confundir a serialização
    public string? IdBusca { get; set; } = "";


    public static int contador { get; set; } = 1;

    [SerializationConstructor]
    public ContatoModel(string id, string nome, string email, string celular, string telefone, string cpf, DateTime nascimento)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Celular = celular;
        Telefone = telefone;
        CPF = cpf;
        Nascimento = nascimento;
    }

    public ContatoModel()
    {
    }

    int IComparable<ContatoModel>.CompareTo(ContatoModel? other)
    {
        if (Id == null || other == null || other.Id == null)
            return 0;

        return this.Id.CompareTo(other.Id);
    }
}

public class ContatoViewModel
{
    public ContatoModel NovoContato { get; set; } = new ContatoModel();
    public List<ContatoModel> ListaContatos { get; set; } = new List<ContatoModel>();
    public String selecaoId { get; set; } = "0000";

}
