using System;
using ProjetoEstoque;

class Program
{
    static void Main()
    {
        Banco.Inicializar();

        while (true)
        {
            Console.WriteLine("\n[1] Listar  [2] Adicionar  [3] Atualizar  [4] Remover  [0] Sair");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    foreach (var p in Banco.Listar())
                        Console.WriteLine($"{p.Id}: {p.Nome} - {p.Quantidade} unid - R$ {p.Preco:F2}");
                    break;

                case "2":
                    Produto novo = new Produto();
                    Console.Write("Nome: "); novo.Nome = Console.ReadLine();
                    Console.Write("Descrição: "); novo.Descricao = Console.ReadLine();
                    Console.Write("Quantidade: "); novo.Quantidade = int.Parse(Console.ReadLine());
                    Console.Write("Preço: "); novo.Preco = double.Parse(Console.ReadLine());
                    Banco.Inserir(novo);
                    Console.WriteLine("Produto inserido.");
                    break;

                case "3":
                    Console.Write("ID do produto: ");
                    int idAlt = int.Parse(Console.ReadLine());
                    Produto alt = new Produto { Id = idAlt };
                    Console.Write("Novo Nome: "); alt.Nome = Console.ReadLine();
                    Console.Write("Nova Descrição: "); alt.Descricao = Console.ReadLine();
                    Console.Write("Nova Quantidade: "); alt.Quantidade = int.Parse(Console.ReadLine());
                    Console.Write("Novo Preço: "); alt.Preco = double.Parse(Console.ReadLine());
                    Banco.Atualizar(alt);
                    Console.WriteLine("Produto atualizado.");
                    break;

                case "4":
                    Console.Write("ID do produto a remover: ");
                    int idDel = int.Parse(Console.ReadLine());
                    Banco.Remover(idDel);
                    Console.WriteLine("Produto removido.");
                    break;

                case "0":
                    return;
            }
        }
    }
}
