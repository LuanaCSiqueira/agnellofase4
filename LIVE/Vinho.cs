using System;

namespace LIVE
{
    public class Vinho
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public int Safra { get; set; }
        public string Produtor { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEmEstoque { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public Vinho()
        {
            Nome = string.Empty;
            Tipo = string.Empty;
            Produtor = string.Empty;
            Descricao = string.Empty;
        }

        public Vinho(int id, string nome, string tipo, int safra, string produtor, decimal preco, int quantidade, string descricao)
        {
            Id = id;
            Nome = nome ?? string.Empty;
            Tipo = tipo ?? string.Empty;
            Safra = safra;
            Produtor = produtor ?? string.Empty;
            Preco = preco;
            QuantidadeEmEstoque = quantidade;
            Descricao = descricao ?? string.Empty;
            Ativo = true;
        }

        public override string ToString()
        {
            return $"ID: {Id}\n" +
                   $"Nome: {Nome}\n" +
                   $"Tipo: {Tipo}\n" +
                   $"Safra: {Safra}\n" +
                   $"Produtor: {Produtor}\n" +
                   $"Preço: R${Preco:F2}\n" +
                   $"Estoque: {QuantidadeEmEstoque}\n" +
                   $"Descrição: {Descricao}\n" +
                   $"Ativo: {(Ativo ? "Sim" : "Não")}\n" +
                   $"---------------------------------";
        }
    }
}