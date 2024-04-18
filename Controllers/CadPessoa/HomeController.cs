using API.Contexts;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace API.Controllers.CadPessoa
{
    [ApiController, Route("api/cad-pessoa")]
    public class HomeController : ControllerBase
    {
        public HomeController(IMongoDBContext context)
        {
            _context = context;
        }

        private readonly IMongoDBContext _context;
        
        [HttpPost]
        //public IActionResult Create(CadPessoaViewModel viewModel)
        public async Task<IActionResult> Create(CadPessoaViewModel viewModel, CancellationToken cancellationToken)
        {
            var cadPessoa = new Models.CadPessoa(viewModel.Nome, viewModel.Idade);
            cadPessoa.AlterarPeso(viewModel.Peso);
            cadPessoa.AlterarAltura(viewModel.Altura);
            cadPessoa.AlterarIdade(viewModel.Idade);

            //loop mais completo 
            /*foreach(var profissao in viewModel.Profissoes)
            {
                var cadPessoaProfissao = new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    cargoNome: profissao.CargoNome,
                    empresaCnpj: profissao.EmpresaCnpj);
            }*/

            //loop menos completo e mais performático, sem continue ou break
            viewModel.Profissoes.ForEach(profissao =>
            {
                cadPessoa.AddProfissao(new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    cargoNome: profissao.CargoNome,
                    empresaCnpj: profissao.EmpresaCnpj));
            });

            //Criando validação
            if (!viewModel.Profissoes.Any())
                //return BadRequest("Selecione pelo menos uma profissão!");
                return ValidationProblem("Selecione pelo menos uma profissão!");

            if (!await _context.AnyAsync(viewModel.Nome, cancellationToken))
                await _context.InsertAsync(cadPessoa, cancellationToken);
            else 
                return ValidationProblem("Usuário já cadastrado!");

            return Ok(new
            {
                Mensagem = "Cadastro criado com sucesso!",
                Data = cadPessoa,
            });
        }

        [HttpPost, Route("pesquisa")]
        public async Task<IActionResult> GetCadPessoaAsync(PesquisaPessoaViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(viewModel.Pesquisar))
            {
                var cadPessoas = await _context.GetPesquisaAsync(viewModel.Pesquisar, cancellationToken);

                return Ok(new
                {
                    Mensagem = $"Resultado para a busca: {viewModel.Pesquisar}",
                    Resultados = cadPessoas
                });
            }
            else
            {
                return ValidationProblem("Preencha o campo pesquisar.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCadPessoaAsync(CadPessoaViewModel viewModel, CancellationToken cancellationToken)
        {
            var cadPessoa = await _context.GetAsync(viewModel.Nome, cancellationToken);

            cadPessoa.AlterarAltura(viewModel.Altura);
            cadPessoa.AlterarPeso(viewModel.Peso);
            cadPessoa.AlterarIdade(viewModel.Idade);

            viewModel.Profissoes.ForEach(profissao =>
            {
                cadPessoa.AddProfissao(new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    empresaCnpj: profissao.EmpresaCnpj,
                    cargoNome: profissao.CargoNome));
            });

            await _context.UpdateAsync(cadPessoa, cancellationToken);

            return Ok(new
            {
                Mensagem = "Registro atualizado com sucesso!",
                Data = cadPessoa,
            });
        }

        [HttpDelete, Route("delete/{nome}")]
        public async Task<IActionResult> DeleteCadPessoaAsync(string nome, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(nome, cancellationToken);

            return Ok(new
            {
                Mensagem = "Registro deletado com sucesso!",
                Data = new
                {
                    nome
                }
            });
        }
            //else
            //    return BadRequest(new
            //    {
            //        Mensagem = "Nenhum registro encontrado!"
            //    });
                //return BadRequest("Nenhum registro encontrado");
                //return Ok(new
                //{
                //    Mensagem = "Nenhum registro encontrado!"
                //});
    }
}
