using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoDeJogos.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CicloDeVidaIdController : ControllerBase
    {

        public readonly IExemploSingleton _exemploSingleton1; //Gera uma instancia do recurso e pega a instancia e fica até o final do projeto, quando a aplicação ficar no ar fica a instancia para sempre
        public readonly IExemploSingleton _exemploSingleton2;

        public readonly IExemploScoped _exemploScoped1; //Ele pendura durante toda requisição
        public readonly IExemploScoped _exemploScoped2;


        public readonly IExemploTransient _exemploTransient1;
        public readonly IExemploTransient _exemploTransient2;

        public CicloDeVidaIdController(IExemploSingleton exemploSingleton1, IExemploSingleton exemploSingleton2, IExemploScoped exemploScoped1, IExemploScoped exemploScoped2, IExemploTransient exemploTransient1, IExemploTransient exemploTransient2)
        {
            _exemploSingleton1 = exemploSingleton1;
            _exemploSingleton2 = exemploSingleton2;
            _exemploScoped1 = exemploScoped1;
            _exemploScoped2 = exemploScoped2;
            _exemploTransient1 = exemploTransient1;
            _exemploTransient2 = exemploTransient2;
        }

        /// <summary>
        /// Observa o ID, vê a diverença de Singleton,Scoped,Transient.(Atualizar pagina)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<string> Get()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Singleton 1: {_exemploSingleton1.Id}");
            stringBuilder.AppendLine($"Singleton 2: {_exemploSingleton2.Id}");

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Scoped 1: {_exemploScoped1.Id}");
            stringBuilder.AppendLine($"Scoped 2: {_exemploScoped2.Id}");

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Transient 1: {_exemploTransient1.Id}");
            stringBuilder.AppendLine($"Transient 2: {_exemploTransient2.Id}");

            return Task.FromResult(stringBuilder.ToString());
        }

        public interface IExemploGeral
        {
            public Guid Id { get; }
        }

        public interface IExemploSingleton : IExemploGeral
        {
        }

        public interface IExemploScoped : IExemploGeral { }
        public interface IExemploTransient : IExemploGeral { }

        public class ExemploCicloDeVida : IExemploSingleton, IExemploScoped, IExemploTransient
        {
            private readonly Guid _guid;

            public ExemploCicloDeVida()
            {
                _guid = Guid.NewGuid();
            }

            public Guid Id => _guid;
        }
    }
}
