using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ProjetoEstoque
{
    public class Banco
    {
        private static string conexao = "Data Source=vinheria.db;Version=3;";

        public static void Inicializar()
        {
            using var con = new SQLiteConnection(conexao);
            con.Open();

            string sql = @"CREATE TABLE IF NOT EXISTS produtos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nome TEXT,
                            Descricao TEXT,
                            Quantidade INTEGER,
                            Preco REAL)";
            using var cmd = new SQLiteCommand(sql, con);
            cmd.ExecuteNonQuery();
        }

        public static void Inserir(Produto p)
        {
            using var con = new SQLiteConnection(conexao);
            con.Open();

            var cmd = new SQLiteCommand("INSERT INTO produtos (Nome, Descricao, Quantidade, Preco) VALUES (@nome, @desc, @qtd, @preco)", con);
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@desc", p.Descricao);
            cmd.Parameters.AddWithValue("@qtd", p.Quantidade);
            cmd.Parameters.AddWithValue("@preco", p.Preco);
            cmd.ExecuteNonQuery();
        }

        public static List<Produto> Listar()
        {
            var lista = new List<Produto>();
            using var con = new SQLiteConnection(conexao);
            con.Open();

            var cmd = new SQLiteCommand("SELECT * FROM produtos", con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString(),
                    Descricao = reader["Descricao"].ToString(),
                    Quantidade = Convert.ToInt32(reader["Quantidade"]),
                    Preco = Convert.ToDouble(reader["Preco"])
                });
            }
            return lista;
        }

        public static void Atualizar(Produto p)
        {
            using var con = new SQLiteConnection(conexao);
            con.Open();

            var cmd = new SQLiteCommand("UPDATE produtos SET Nome = @nome, Descricao = @desc, Quantidade = @qtd, Preco = @preco WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@desc", p.Descricao);
            cmd.Parameters.AddWithValue("@qtd", p.Quantidade);
            cmd.Parameters.AddWithValue("@preco", p.Preco);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.ExecuteNonQuery();
        }

        public static void Remover(int id)
        {
            using var con = new SQLiteConnection(conexao);
            con.Open();

            var cmd = new SQLiteCommand("DELETE FROM produtos WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
