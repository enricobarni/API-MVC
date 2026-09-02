using MessagePack;

namespace Contatos.Models;

public class Serializa
{
    public static void save(List<ContatoModel> lista, string arquivo)
    {
        if (lista == null || lista.Count == 0)
            return;

        try
        {
            byte[] bytes = MessagePackSerializer.Serialize(lista);
            File.WriteAllBytes(arquivo, bytes);
        }
        catch
        {
            return;
        }
    }

    public static List<ContatoModel> load(string arquivo)
    {
        try
        {
            if (!File.Exists(arquivo))
                return new List<ContatoModel>();

            byte[] bytes = File.ReadAllBytes(arquivo);
            if (bytes.Length == 0)
                return new List<ContatoModel>();

            return MessagePackSerializer.Deserialize<List<ContatoModel>>(bytes);
        }
        catch
        {
            return new List<ContatoModel>();
        }
    }
}
