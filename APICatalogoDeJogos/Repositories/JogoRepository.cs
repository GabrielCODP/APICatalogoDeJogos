using APICatalogoDeJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogoDeJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        public static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"), new Jogo{Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),Nome="Fifa 21", Produtora = "EA", Preco = 200} },
            {Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new Jogo{Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),Nome="GTA",Produtora="Rockstar North",Preco=150} }

        }; //Objeto em mémoria


        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        } //Ele bem dentro do dicionario, é uma logia para pegar 5 pagianas.

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;
            
            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    retorno.Add(jogo);
            }
            return Task.FromResult(retorno);
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }


        public Task Remove(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar Conexao com o Banco
        }
    }
}
