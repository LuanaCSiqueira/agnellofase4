using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace LIVE 
{
    public class Estoque
    {
        private List<Vinho> vinhos;
        private string caminhoArquivo;
        private int proximoId;

        public Estoque()
        {
            vinhos = new List<Vinho>();
            string nomeDaAplicacao = "MinhaVinheriaLIVE";
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string diretorioDaAplicacao = Path.Combine(appDataPath, nomeDaAplicacao);
            caminhoArquivo = Path.Combine(diretorioDaAplicacao, "banco_vinhos.txt");

            CarregarVinhos();
        }

        private void GarantirDiretorioExiste()
        {
            string? diretorio = Path.GetDirectoryName(caminhoArquivo);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
        }

        private void CarregarVinhos()
        {
            GarantirDiretorioExiste();
            vinhos.Clear();

            if (!File.Exists(caminhoArquivo))
            {
                proximoId = 1;
                return;
            }

            try
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);
                Vinho? vinhoAtual = null;
                int maiorIdEncontrado = 0;

                foreach (string linhaOriginal in linhas)
                {
                    string linha = linhaOriginal.Trim();
                    if (string.IsNullOrEmpty(linha)) continue;

                    if (linha.StartsWith("Id:"))
                    {
                        vinhoAtual = new Vinho();
                        if (int.TryParse(linha.Substring(linha.IndexOf(':') + 1).Trim(), out int idTemp))
                        {
                            vinhoAtual.Id = idTemp;
                            if (vinhoAtual.Id > maiorIdEncontrado) maiorIdEncontrado = vinhoAtual.Id;
                        }
                        else { vinhoAtual = null; continue; }
                    }
                    else if (vinhoAtual != null)
                    {
                        string valor = "";
                        int colonIndex = linha.IndexOf(':');
                        if (colonIndex != -1 && colonIndex + 1 < linha.Length)
                        {
                            valor = linha.Substring(colonIndex + 1).Trim();
                        }
                        else if (linha != "-------------------------------")
                        {
                            continue;
                        }


                        if (linha.StartsWith("Nome:")) vinhoAtual.Nome = valor;
                        else if (linha.StartsWith("Tipo:")) vinhoAtual.Tipo = valor;
                        else if (linha.StartsWith("Safra:"))
                        {
                            if (int.TryParse(valor, out int safraTemp)) vinhoAtual.Safra = safraTemp;
                        }
                        else if (linha.StartsWith("Produtor:")) vinhoAtual.Produtor = valor;
                        else if (linha.StartsWith("Preco:"))
                        {
                            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoTemp)) vinhoAtual.Preco = precoTemp;
                        }
                        else if (linha.StartsWith("QuantidadeEmEstoque:"))
                        {
                            if (int.TryParse(valor, out int qtdTemp)) vinhoAtual.QuantidadeEmEstoque = qtdTemp;
                        }
                        else if (linha.StartsWith("Descricao:")) vinhoAtual.Descricao = valor;
                        else if (linha.StartsWith("Ativo:"))
                        {
                            if (bool.TryParse(valor, out bool ativoTemp)) vinhoAtual.Ativo = ativoTemp;
                        }
                        else if (linha == "-------------------------------")
                        {
                            if (vinhoAtual.Id != 0 && !vinhos.Any(v => v.Id == vinhoAtual.Id))
                            {
                                vinhos.Add(vinhoAtual);
                            }
                            vinhoAtual = null;
                        }
                    }
                }
                if (vinhoAtual != null && vinhoAtual.Id != 0 && !vinhos.Any(v => v.Id == vinhoAtual.Id))
                {
                    vinhos.Add(vinhoAtual);
                }

                proximoId = vinhos.Any() ? vinhos.Max(v => v.Id) + 1 : 1;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro crítico ao carregar vinhos: {ex.Message}.");
                Console.WriteLine("O arquivo 'banco_vinhos.txt' pode estar corrompido ou ter um formato inesperado.");
                Console.ResetColor();
                vinhos.Clear();
                proximoId = 1;
            }
        }

        public void SalvarVinhos()
        {
            GarantirDiretorioExiste();
            try
            {
                List<string> linhasParaSalvar = new List<string>();
                foreach (var vinho in vinhos)
                {
                    linhasParaSalvar.Add($"Id:{vinho.Id}");
                    linhasParaSalvar.Add($"Nome:{vinho.Nome}");
                    linhasParaSalvar.Add($"Tipo:{vinho.Tipo}");
                    linhasParaSalvar.Add($"Safra:{vinho.Safra}");
                    linhasParaSalvar.Add($"Produtor:{vinho.Produtor}");
                    linhasParaSalvar.Add($"Preco:{vinho.Preco.ToString(CultureInfo.InvariantCulture)}");
                    linhasParaSalvar.Add($"QuantidadeEmEstoque:{vinho.QuantidadeEmEstoque}");
                    linhasParaSalvar.Add($"Descricao:{vinho.Descricao}");
                    linhasParaSalvar.Add($"Ativo:{vinho.Ativo.ToString().ToLower()}");
                    linhasParaSalvar.Add("-------------------------------");
                }
                File.WriteAllLines(caminhoArquivo, linhasParaSalvar);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao salvar vinhos: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void CadastrarVinho(string nome, string tipo, int safra, string produtor, decimal preco, int quantidade, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome)) { Console.WriteLine("Erro: Nome do vinho não pode ser vazio."); return; }
            if (preco <= 0) { Console.WriteLine("Erro: Preço deve ser maior que zero."); return; }
            if (quantidade < 0) { Console.WriteLine("Erro: Quantidade em estoque não pode ser negativa."); return; }

            Vinho novoVinho = new Vinho(proximoId, nome, tipo, safra, produtor, preco, quantidade, descricao);
            vinhos.Add(novoVinho);
            proximoId++;
            SalvarVinhos();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vinho '{nome}' cadastrado com sucesso! ID: {novoVinho.Id}");
            Console.ResetColor();
        }

        public void ListarTodosOsVinhos()
        {
            Console.WriteLine("\n--- Lista Completa de Vinhos ---");
            if (!vinhos.Any()) { Console.WriteLine("Nenhum vinho cadastrado."); return; }
            foreach (var vinho in vinhos) { Console.WriteLine(vinho.ToString()); }
        }

        public void ListarVinhosAtivos()
        {
            Console.WriteLine("\n--- Lista de Vinhos Ativos ---");
            var vinhosAtivos = vinhos.Where(v => v.Ativo).ToList();
            if (!vinhosAtivos.Any()) { Console.WriteLine("Nenhum vinho ativo no estoque."); return; }
            foreach (var vinho in vinhosAtivos) { Console.WriteLine(vinho.ToString()); }
        }

        public Vinho? BuscarVinhoPorId(int id)
            return vinhos.FirstOrDefault(v => v.Id == id);
        }

        public void EditarVinho(int id, string novoNome, string novoTipo, int novaSafra, string novoProdutor, decimal novoPreco, int novaQuantidade, string novaDescricao)
        {
            Vinho? vinhoParaEditar = BuscarVinhoPorId(id);

            if (vinhoParaEditar == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: Vinho com ID {id} não encontrado para edição.");
                Console.ResetColor();
                return;
            }

            vinhoParaEditar.Nome = novoNome;
            vinhoParaEditar.Tipo = novoTipo;
            vinhoParaEditar.Safra = novaSafra;
            vinhoParaEditar.Produtor = novoProdutor;
            vinhoParaEditar.Preco = novoPreco;
            vinhoParaEditar.QuantidadeEmEstoque = novaQuantidade;
            vinhoParaEditar.Descricao = novaDescricao;

            SalvarVinhos();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vinho ID {id} ('{vinhoParaEditar.Nome}') atualizado com sucesso!");
            Console.ResetColor();
        }

        public bool RemoverVinho(int id)
        {
            Vinho? vinhoParaRemover = BuscarVinhoPorId(id);

            if (vinhoParaRemover == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: Vinho com ID {id} não encontrado para remoção.");
                Console.ResetColor();
                return false;
            }

            vinhos.Remove(vinhoParaRemover);
            SalvarVinhos();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vinho ID {id} ('{vinhoParaRemover.Nome}') removido com sucesso!");
            Console.ResetColor();
            return true;
        }

        public bool DesativarVinho(int id)
        {
            Vinho? vinhoParaDesativar = BuscarVinhoPorId(id);

            if (vinhoParaDesativar == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: Vinho com ID {id} não encontrado.");
                Console.ResetColor();
                return false;
            }

            if (!vinhoParaDesativar.Ativo)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Vinho ID {id} ('{vinhoParaDesativar.Nome}') já está desativado.");
                Console.ResetColor();
                return false;
            }

            vinhoParaDesativar.Ativo = false;
            SalvarVinhos();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vinho ID {id} ('{vinhoParaDesativar.Nome}') desativado com sucesso!");
            Console.ResetColor();
            return true;
        }

        public bool AtivarVinho(int id)
        {
            Vinho? vinhoParaAtivar = BuscarVinhoPorId(id);

            if (vinhoParaAtivar == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: Vinho com ID {id} não encontrado.");
                Console.ResetColor();
                return false;
            }

            if (vinhoParaAtivar.Ativo)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Vinho ID {id} ('{vinhoParaAtivar.Nome}') já está ativo.");
                Console.ResetColor();
                return false;
            }

            vinhoParaAtivar.Ativo = true;
            SalvarVinhos();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Vinho ID {id} ('{vinhoParaAtivar.Nome}') ativado com sucesso!");
            Console.ResetColor();
            return true;
        }

    }
}