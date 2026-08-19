namespace API_MVC.Models
{
    public class ContatoModel : IComparable<ContatoModel>
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }

        private static long Contador = 0;

        public ContatoModel(String Nome, String Celular, String Email, String Cpf)
        {
            this.Nome = Nome;
            this.Celular = Celular;
            this.Email = Email;
            this.Cpf = Cpf;
            Id = Contador++;
        }

        public int CompareTo(ContatoModel nome)
        {
            return Nome.CompareTo(nome.Nome);
        }
    }

    public class ListaContatoModel 
    {
        public List<ContatoModel> Lista = new List<ContatoModel>
        {
            new ContatoModel("Ana Silva", "9 9123 4567", "ana.silva@yahoo.com", "123.456.789-00"),
            new ContatoModel("Bruno Costa", "9 9234 5678", "bruno.costa@yahoo.com", "234.567.890-11"),
            new ContatoModel("Carlos Souza", "9 9345 6789", "carlos.souza@yahoo.com", "345.678.901-22"),
            new ContatoModel("Daniela Lima", "9 9456 7890", "daniela.lima@yahoo.com", "456.789.012-33"),
            new ContatoModel("Eduardo Ribeiro", "9 9567 8901", "eduardo.ribeiro@yahoo.com", "567.890.123-44"),
            new ContatoModel("Maria Antônia", "9 9877 6754", "maria.antonia@yahoo.com", "284.591.730-80"),
            new ContatoModel("Carlos Eduardo", "9 9866 9950", "carlos.eduardo@yahoo.com", "519.304.820-41"),
            new ContatoModel("João Pedro", "9 9123 4567", "joao.pedro@gmail.com", "842.167.390-55")
        };
    }
}
