public class TreinandoReflection
{
    static void Main(string[] args)
    {
        var pessoa = ManipulandoObjetos.CriarNovaInstancia<Pessoa>();
        var tipoPessoa = pessoa.GetType();
        var propriedadeNome = tipoPessoa.GetProperty("Nome");
        propriedadeNome.SetValue(pessoa, "Celso");
        var propriedadeDataNascimento = tipoPessoa.GetProperty("DataNascimento");
        propriedadeDataNascimento.SetValue(pessoa, new DateTime(1996, 03, 25));
        var propriedadeAltura = tipoPessoa.GetProperty("Altura");
        propriedadeAltura.SetValue(pessoa, 1.87);
        var propriedadeCasado = tipoPessoa.GetProperty("Casado");
        propriedadeCasado.SetValue(pessoa, true);
        var propriedadeNumeroFilhos = tipoPessoa.GetProperty("NumeroFilhos");
        propriedadeNumeroFilhos.SetValue(pessoa, 1);
        pessoa.AdicionarPet(new Pet()
        {
            Nome = "Petisco",
            Especie = "Cachorro",
            DataNascimento = new DateTime(2019, 08, 29)
        });

        ManipulandoObjetos.PropriedadesEMetodos(pessoa);

        var metodo1 = tipoPessoa.GetMethod("AdicionarPet");
        metodo1.Invoke(pessoa, new object[]{new Pet()
        {
            Nome = "Toquinho",
            Especie = "Cachorro",
            DataNascimento = new DateTime(2009,11,29)
        }
    });
        var metodo2 = tipoPessoa.GetMethod("QtdPetsVivos");
        Console.WriteLine($"Número de Pets vivos: {metodo2.Invoke(pessoa, null)}");
        var metodo3 = tipoPessoa.GetMethod("PetMorreu");
        metodo3.Invoke(pessoa, new object[] { "Toquinho", "Cachorro", new DateTime(2009, 11, 29) });
        Console.WriteLine($"Número de Pets vivos: {metodo2.Invoke(pessoa, null)}");
        var metodo4 = tipoPessoa.GetMethod("NovoFilho");
        Console.WriteLine($"Número de filhos: {metodo4.Invoke(pessoa, null)}");
        var metodo5 = tipoPessoa.GetMethod("Idade");
        Console.WriteLine($"Idade: {metodo5.Invoke(pessoa, null)}");
    }
}

public static class ManipulandoObjetos
{
    public static void PropriedadesEMetodos<T>(T value)
    {
        var tipoObjeto = value.GetType();
        var propriedadesObjeto = tipoObjeto.GetProperties();
        foreach(var propriedade in propriedadesObjeto)
        {
            Console.WriteLine($"{propriedade.Name}: {propriedade.GetValue(value)}");
        }
        Console.WriteLine();
        var metodosObjeto = tipoObjeto.GetMethods();
        foreach(var metodo in metodosObjeto)
        {
            Console.WriteLine($"Método: {metodo.Name}\nTipo do retorno: {metodo.ReturnType.Name}");
            var parametrosMetodo = metodo.GetParameters();
            foreach(var parametro in parametrosMetodo)
            {
                Console.WriteLine($"Parâmetro: {parametro.Name}");
            }
            Console.WriteLine();
        }
    }

    public static T CriarNovaInstancia<T>() => Activator.CreateInstance<T>();
}

public class Pessoa
{
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public double Altura { get; set; }
    public bool Casado { get; set; }
    public int NumeroFilhos { get; set; }
    private List<Pet> Pets = new List<Pet>();

    public void Casar()
    {
        if (this.Casado!)
        {
            this.Casado = true;
            return;
        }
    }

    public void Separar()
    {
        if (this.Casado)
        {
            this.Casado = false;
            return;
        }
    }

    public int Idade() => (int)Math.Floor((DateTime.Now - DataNascimento).TotalDays / 365);

    public int NovoFilho() => ++this.NumeroFilhos;

    public void AdicionarPet(Pet pet) => Pets.Add(pet);

    public void PetMorreu(string nome, string especie, DateTime dataNascimento)
    {
        Pet? pet = Pets.FirstOrDefault(x => x.Nome == nome && x.Especie == especie && x.DataNascimento == dataNascimento);
        if(pet != null)
        {
            pet.Vivo = false;
            return;
        }
        return;
    }

    public int QtdPetsVivos() => Pets.Count(x => x.Vivo == true);
}

public class Pet
{
    public string Nome { get; set; }
    public string Especie { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Vivo { get; set; } = true;
}
