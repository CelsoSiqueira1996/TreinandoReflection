using System;
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
            Idade = 4
        });

        ManipulandoObjetos.PropriedadesEMetodos(pessoa);

        var metodo = tipoPessoa.GetMethod("AdicionarPet");
        metodo.Invoke(pessoa, new object[]{new Pet()
        {
            Nome = "Toquinho",
            Especie = "Cachorro",
            Idade = 14
        }
    });
        var metodo2 = tipoPessoa.GetMethod("QtdPetsVivos");
        Console.WriteLine(metodo2.Invoke(pessoa, null));
        var metodo3 = tipoPessoa.GetMethod("PetMorreu");
        metodo3.Invoke(pessoa, new object[] { "Toquinho", "Cachorro", 14 });
        Console.WriteLine(metodo2.Invoke(pessoa, null));
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

    public static T CriarNovaInstancia<T>()
    {
        return Activator.CreateInstance<T>();
    }
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

    public int Idade()
    {
        return (int)Math.Floor((DateTime.Now - DataNascimento).TotalDays / 365);
    }

    public int NovoFilho()
    {
        return this.NumeroFilhos += 1;
    }
    public void AdicionarPet(Pet pet)
    {
        Pets.Add(pet);
    }

    public void PetMorreu(string nome, string especie, int idade)
    {
        Pet? pet = Pets.FirstOrDefault(x => x.Nome == nome && x.Especie == especie && x.Idade == idade);
        if(pet != null)
        {
            pet.Vivo = false;
            return;
        }
        return;
    }

    public int QtdPetsVivos()
    {
        return Pets.Count(x => x.Vivo == true);
    }
}

public class Pet
{
    public string Nome { get; set; }
    public string Especie { get; set; }
    public int Idade { get; set; }
    public bool Vivo { get; set; } = true;
}
