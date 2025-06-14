using System;
using System.Globalization;
using LIVE;

class Program
{
    static Estoque meuEstoque = new Estoque();

    static void Main(string[] args)
    {
        bool executando = true;
        while (executando)
        {
            Console.Clear();
            Console.WriteLine("***********************************************************");
            Console.WriteLine("******************* ESTOQUE DE VINHO *********************");
            Console.WriteLine("***********************************************************");
            Console.WriteLine("****************** 1 - Cadastrar Novo Vinho ***************");
            Console.WriteLine("****************** 2 - Listar Todos os Vinhos *************");
            Console.WriteLine("****************** 3 - Listar Vinhos Ativos ***************");
            Console.WriteLine("****************** 4 - Editar Vinho ***********************");
            Console.WriteLine("****************** 5 - Remover Vinho **********************");
            Console.WriteLine("****************** 6 - Desativar Vinho ********************");
            Console.WriteLine("****************** 7 - Ativar Vinho ***********************");          
            Console.WriteLine("***********************************************************");
            Console.WriteLine("****************** 0 - Sair *******************************");
            Console.WriteLine("***********************************************************");
            Console.Write("******************* Escolha uma opção: ");

            string opcao = Console.ReadLine() ?? "";
            Console.WriteLine("***********************************************************");


            switch (opcao)
            {
                case "1":
                    CadastrarVinhoUI();
                    break;
                case "2":
                    meuEstoque.ListarTodosOsVinhos();
                    break;
                case "3":
                    meuEstoque.ListarVinhosAtivos();
                    break;
                case "4":
                    EditarVinhoUI();
                    break;
                case "5":
                    RemoverVinhoUI();
                    break;
                case "6": 
                    AlterarStatusVinhoUI(false);
                    break;
                case "7":
                    AlterarStatusVinhoUI(true);
                    break;
                case "0":
                    executando = false;
                    Console.WriteLine("Saindo do sistema...");
                    break;
            }

            if (executando)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void CadastrarVinhoUI()
    {
        Console.WriteLine("\n--- Cadastrar Novo Vinho ---");
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? "";

        Console.Write("Tipo (Ex: Tinto, Branco, Rosé): ");
        string tipo = Console.ReadLine() ?? "";

        int safra = 0;
        bool safraValida = false;
        while (!safraValida)
        {
            Console.Write("Safra (ano, digite 0 se não aplicável): ");
            string safraInput = Console.ReadLine() ?? "";
            if (int.TryParse(safraInput, out safra) && (safra == 0 || (safra >= 1800 && safra <= DateTime.Now.Year + 10)))
            {
                safraValida = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Safra inválida. Digite um ano válido (ex: 2022) ou 0.");
                Console.ResetColor();
            }
        }

        Console.Write("Produtor: ");
        string produtor = Console.ReadLine() ?? "";

        decimal preco = 0;
        bool precoValido = false;
        while (!precoValido)
        {
            Console.Write("Preço (Ex: 25.90 ou 25,90): ");
            string precoInput = Console.ReadLine() ?? "";
            if (decimal.TryParse(precoInput.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out preco))
            {
                if (preco > 0)
                {
                    precoValido = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Preço deve ser maior que zero.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Formato de preço inválido. Use ponto ou vírgula como separador decimal.");
                Console.ResetColor();
            }
        }

        int quantidade = -1;
        bool qtdValida = false;
        while (!qtdValida)
        {
            Console.Write("Quantidade em Estoque: ");
            string qtdInput = Console.ReadLine() ?? "";
            if (int.TryParse(qtdInput, out quantidade) && quantidade >= 0)
            {
                qtdValida = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Quantidade inválida. Digite um número igual ou maior que zero.");
                Console.ResetColor();
            }
        }

        Console.Write("Descrição: ");
        string descricao = Console.ReadLine() ?? "";

        meuEstoque.CadastrarVinho(nome, tipo, safra, produtor, preco, quantidade, descricao);
    }
    static void EditarVinhoUI()
    {
        Console.WriteLine("\n--- Editar Vinho ---");
        meuEstoque.ListarTodosOsVinhos();

        Console.Write("Digite o ID do vinho que deseja editar: ");
        string idInput = Console.ReadLine() ?? "";
        if (!int.TryParse(idInput, out int idParaEditar))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ID inválido.");
            Console.ResetColor();
            return;
        }

        Vinho? vinhoExistente = meuEstoque.BuscarVinhoPorId(idParaEditar);
        if (vinhoExistente == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vinho com ID {idParaEditar} não encontrado.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("\nEditando o seguinte vinho:");
        Console.WriteLine(vinhoExistente.ToString());
        Console.WriteLine("\nDeixe o campo em branco e pressione Enter para manter o valor atual.");

        Console.Write($"Novo Nome (atual: {vinhoExistente.Nome}): ");
        string nomeInput = Console.ReadLine() ?? "";
        string novoNome = string.IsNullOrWhiteSpace(nomeInput) ? vinhoExistente.Nome : nomeInput;

        Console.Write($"Novo Tipo (atual: {vinhoExistente.Tipo}): ");
        string tipoInput = Console.ReadLine() ?? "";
        string novoTipo = string.IsNullOrWhiteSpace(tipoInput) ? vinhoExistente.Tipo : tipoInput;

        int novaSafra = vinhoExistente.Safra;
        bool safraValida = false;
        while (!safraValida)
        {
            Console.Write($"Nova Safra (ano, atual: {vinhoExistente.Safra}, deixe em branco para manter): ");
            string safraInput = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(safraInput))
            {
                safraValida = true;
            }
            else if (int.TryParse(safraInput, out int safraTemp) && (safraTemp == 0 || (safraTemp >= 1800 && safraTemp <= DateTime.Now.Year + 10)))
            {
                novaSafra = safraTemp;
                safraValida = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Safra inválida. Digite um ano válido ou 0, ou deixe em branco para manter.");
                Console.ResetColor();
            }
        }

        Console.Write($"Novo Produtor (atual: {vinhoExistente.Produtor}): ");
        string produtorInput = Console.ReadLine() ?? "";
        string novoProdutor = string.IsNullOrWhiteSpace(produtorInput) ? vinhoExistente.Produtor : produtorInput;

        decimal novoPreco = vinhoExistente.Preco;
        bool precoValido = false;
        while (!precoValido)
        {
            Console.Write($"Novo Preço (atual: {vinhoExistente.Preco:F2}, deixe em branco para manter): ");
            string precoInput = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(precoInput))
            {
                precoValido = true;
            }
            else if (decimal.TryParse(precoInput.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoTemp))
            {
                if (precoTemp > 0)
                {
                    novoPreco = precoTemp;
                    precoValido = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Preço deve ser maior que zero.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Formato de preço inválido. Use ponto ou vírgula, ou deixe em branco para manter.");
                Console.ResetColor();
            }
        }

        int novaQuantidade = vinhoExistente.QuantidadeEmEstoque;
        bool qtdValida = false;
        while (!qtdValida)
        {
            Console.Write($"Nova Quantidade em Estoque (atual: {vinhoExistente.QuantidadeEmEstoque}, deixe em branco para manter): ");
            string qtdInput = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(qtdInput))
            {
                qtdValida = true;
            }
            else if (int.TryParse(qtdInput, out int qtdTemp) && qtdTemp >= 0)
            {
                novaQuantidade = qtdTemp;
                qtdValida = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Quantidade inválida. Digite um número igual ou maior que zero, ou deixe em branco para manter.");
                Console.ResetColor();
            }
        }

        Console.Write($"Nova Descrição (atual: {vinhoExistente.Descricao}): ");
        string descricaoInput = Console.ReadLine() ?? "";
        string novaDescricao = string.IsNullOrWhiteSpace(descricaoInput) ? vinhoExistente.Descricao : descricaoInput;

        meuEstoque.EditarVinho(idParaEditar, novoNome, novoTipo, novaSafra, novoProdutor, novoPreco, novaQuantidade, novaDescricao);
    }

    static void RemoverVinhoUI()
    {
        Console.WriteLine("\n--- Remover Vinho ---");
        meuEstoque.ListarTodosOsVinhos();

        Console.Write("Digite o ID do vinho que deseja remover: ");
        string idRemoverInput = Console.ReadLine() ?? "";
        if (!int.TryParse(idRemoverInput, out int idParaRemover))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ID inválido.");
            Console.ResetColor();
            return;
        }

        Vinho? vinhoExistente = meuEstoque.BuscarVinhoPorId(idParaRemover);
        if (vinhoExistente == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vinho com ID {idParaRemover} não encontrado.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("\nVocê selecionou o seguinte vinho para remoção:");
        Console.WriteLine(vinhoExistente.ToString());

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"\nTem certeza que deseja remover o vinho '{vinhoExistente.Nome}' (ID: {idParaRemover})? (S/N): ");
        Console.ResetColor();
        string confirmacao = Console.ReadLine()?.Trim().ToUpper() ?? "N";

        if (confirmacao == "S")
        {
            meuEstoque.RemoverVinho(idParaRemover);
        }
        else
        {
            Console.WriteLine("Remoção cancelada.");
        }
    }
    
    static void AlterarStatusVinhoUI(bool ativar)
    {
        string acao = ativar ? "Ativar" : "Desativar";
        string estadoAlvo = ativar ? "ativo" : "desativado";

        Console.WriteLine($"\n--- {acao} Vinho ---");
        Console.WriteLine("Lista de vinhos atual:");
        meuEstoque.ListarTodosOsVinhos();


        Console.Write($"Digite o ID do vinho que deseja {acao.ToLower()}: ");
        string idInput = Console.ReadLine() ?? "";
        if (!int.TryParse(idInput, out int idParaAlterar))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ID inválido.");
            Console.ResetColor();
            return;
        }

        Vinho? vinhoExistente = meuEstoque.BuscarVinhoPorId(idParaAlterar);
        if (vinhoExistente == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Vinho com ID {idParaAlterar} não encontrado.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("\nVocê selecionou o seguinte vinho:");
        Console.WriteLine(vinhoExistente.ToString());

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"\nTem certeza que deseja {acao.ToLower()} o vinho '{vinhoExistente.Nome}' (ID: {idParaAlterar})? (S/N): ");
        Console.ResetColor();
        string confirmacao = Console.ReadLine()?.Trim().ToUpper() ?? "N";

        if (confirmacao == "S")
        {
            bool sucesso;
            if (ativar)
            {
                sucesso = meuEstoque.AtivarVinho(idParaAlterar);
            }
            else
            {
                sucesso = meuEstoque.DesativarVinho(idParaAlterar);
            }
        }
        else
        {
            Console.WriteLine($"{acao} cancelada.");
        }
    }

}