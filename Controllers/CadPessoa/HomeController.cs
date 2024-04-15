using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace API.Controllers.CadPessoa
{
    [ApiController, Route("api/cad-pessoa")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        //public IActionResult Create(CadPessoaViewModel viewModel)
        public async Task<IActionResult> Create(CadPessoaViewModel viewModel, CancellationToken cancellation)
        {
            var cadPessoa = new Models.CadPessoa(viewModel.Nome, viewModel.Idade);
            cadPessoa.AlterarPeso(viewModel.Peso);
            cadPessoa.AlterarAltura(viewModel.Altura);

            //loop mais completo que pode fazer lógica dentro
            /*foreach(var profissao in viewModel.Profissoes)
            {
                var cadPessoaProfissao = new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    cargoNome: profissao.CargoNome,
                    empresaCnpj: profissao.EmpresaCnpj);
            }*/

            //loop menos completo e mais performático
            viewModel.Profissoes.ForEach(profissao =>
            {
                cadPessoa.AddProfissao(new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    cargoNome: profissao.CargoNome,
                    empresaCnpj: profissao.EmpresaCnpj));
            });

            if (!viewModel.Profissoes.Any())
                //return BadRequest("Selecione pelo menos uma profissão!");
                return ValidationProblem("Selecione pelo menos uma profissão!");

            var bd = new Contexts.MongoDBContext();
            var colecao = bd.GetCollection<Models.CadPessoa>("CadPessoa");

            var colecaoBusca = colecao.AsQueryable();

            if (!await colecaoBusca
                .Where(w => w.Nome == cadPessoa.Nome)
                .AnyAsync(cancellation))
            {
                //Já cria bd e coleção se não tiver
                await colecao.InsertOneAsync(cadPessoa, null, cancellation);
            }
            else return ValidationProblem("Usuário já cadastrado.");

            return Ok(new
            {
                Mensagem = "Cadastro criado com sucesso!",
                Data = cadPessoa,
            });
        }
    }
}
