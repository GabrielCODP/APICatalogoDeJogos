using APICatalogoDeJogos.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogoDeJogos.Repositories
{
    public class JogoMySQLServeRepository : IJogoRepository
    {
        private readonly MySqlConnection _mySqlConnection;

        public JogoMySQLServeRepository(IConfiguration configuration)
        {
            _mySqlConnection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }


        public async Task<List<Jogo>> Obter (int pagina,int quantidade)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos order by id offser {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await _mySqlConnection.OpenAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(comando, _mySqlConnection);
            MySqlDataReader mySqlDataReader = (MySqlDataReader)await mySqlCommand.ExecuteReaderAsync();

            while (mySqlDataReader.Read())
            {
                jogos.Add(new Jogo {
                    Id = (Guid) mySqlDataReader["Id"],
                    Nome = (string)mySqlDataReader["Nome"],
                    Produtora = (string)mySqlDataReader["Produtora"],
                    Preco =(double)mySqlDataReader["Preco"]
                });
            }

            await _mySqlConnection.CloseAsync();

            return jogos;
        }


        public async Task<Jogo> Obter(Guid id) 
        {
            Jogo jogo = null;

            var comando = $"select * from Jogos where Id = {id}";

            await _mySqlConnection.OpenAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(comando, _mySqlConnection);
            MySqlDataReader mySqlDataReader = (MySqlDataReader)await mySqlCommand.ExecuteReaderAsync();

            while (mySqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = (Guid)mySqlDataReader["Id"],
                    Nome = (string)mySqlDataReader["Nome"],
                    Produtora = (string)mySqlDataReader["Produtora"],
                    Preco = (double)mySqlDataReader["Preco"]
                };
            }

            await _mySqlConnection.CloseAsync();

            return jogo;
        }

        public async Task Inserir(Jogo jogo)
        {
            var comando = $"insert Jogos(Id,Nome,Produtora,Preco) values ('{jogo.Id}','{jogo.Nome}','{jogo.Produtora}',{jogo.Preco.ToString().Replace(",", ".")})";

            await _mySqlConnection.OpenAsync();
            MySqlCommand mySqlCommand = new MySqlCommand(comando, _mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _mySqlConnection.CloseAsync();
        }

        public async Task Atualizar(Jogo jogo)
        {
            var comando = $"update Jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}',Preco = {jogo.Preco.ToString().Replace(",",".")} Where Id = '{jogo.Id}'";

            await _mySqlConnection.OpenAsync();
            MySqlCommand mySqlCommand = new MySqlCommand(comando, _mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _mySqlConnection.CloseAsync();
        }


        public async Task Remove(Guid id)
        {
            var comando = $"delete from Jogos where Id = '{id}'";

            await _mySqlConnection.OpenAsync();
            MySqlCommand mySqlCommand = new MySqlCommand(comando, _mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            await _mySqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            _mySqlConnection?.Close();
            _mySqlConnection?.Dispose();
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            throw new NotImplementedException();
        }
    }
}
