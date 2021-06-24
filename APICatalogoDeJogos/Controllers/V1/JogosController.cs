
using APICatalogoDeJogos.Exceptions;
using APICatalogoDeJogos.InputModel;
using APICatalogoDeJogos.Services;
using APICatalogoDeJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogoDeJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        //Preciso receber um jogo e retorna ele.

        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }


        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retorna os jogos sem paginação
        /// </remarks>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não tem jogo</response>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1,int.MaxValue)] int pagina = 1,[FromQuery,Range(1,50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina,quantidade);

            if (jogos.Count() == 0) 
            {
                return NoContent();
            }


            return Ok(jogos); //Tem que retorna um status 200
        }


        /// <summary>
        /// Busca jogo pelo ID
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo</response>
        /// <response code="204">Caso não tem jogo</response>
        /// <returns></returns>

        [HttpGet("{idJogo:guid}")] //RETORNA UM JOGO
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo==null)
            {
                return NoContent();
            }

            return Ok(jogo);
        }

        /// <summary>
        /// Inserir um jogo,não precisa do Id (é gerado automaticamente)
        /// </summary>
        /// <param name="jogoInputModel"></param>
        /// <response code="200">Retorna o jogo cadastrado/response>
        /// <response code="204">Retorna algum erro de digitação</response>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody]JogoInputModel jogoInputModel)
        {

            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
            
        }

        /// <summary>
        /// Editar o escopo do jogo
        /// </summary>
        /// <param name="idJogo"></param>
        /// <param name="jogoInputModel"></param>
        /// <returns></returns>

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromBody]JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch (JogoNaoCadastradoException)
            {
                return NotFound("Não existe jogo");
            }
        }


        /// <summary>
        /// Editar o preço do jogo
        /// </summary>
        /// <param name="idJogo"></param>
        /// <param name="preco"></param>
        /// <returns></returns>
        
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]  //A diferença é que o Put você atualiza o recurso todo, o Patch é só uma coisa
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute]double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpDelete("{idJogo}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException)
            {
                return NotFound("Não existe este jogo");
            }
        }
    }
}
