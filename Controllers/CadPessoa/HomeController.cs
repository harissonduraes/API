﻿using API.ViewModels;
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

        [HttpPost, Route("pesquisa")]
        public async Task<IActionResult> GetCadPessoaAsync(PesquisaPessoaViewModel viewModel, CancellationToken cancellationToken)
        {
            var bancoDeDados = new Contexts.MongoDBContext();
            var colecao = bancoDeDados.GetCollection<Models.CadPessoa>("CadPessoa");

            if (!string.IsNullOrEmpty(viewModel.Pesquisar))
            {
                var cadPessoas = await colecao
                    .AsQueryable()
                    .Where(w => w.Nome.Contains(viewModel.Pesquisar))
                    .ToListAsync(cancellationToken);

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
            var bd = new Contexts.MongoDBContext();
            var colecao = bd.GetCollection<Models.CadPessoa>("CadPessoa");

            var cadPessoa = await colecao.AsQueryable()
                .Where(w => w.Nome == viewModel.Nome)
                .SingleOrDefaultAsync(cancellationToken);

            cadPessoa.AlterarAltura(viewModel.Altura);
            cadPessoa.AlterarPeso(viewModel.Peso);
            cadPessoa.AlterarPeso(viewModel.Idade);

            viewModel.Profissoes.ForEach(profissao =>
            {
                cadPessoa.AddProfissao(new Models.CadPessoa.CProfissao(
                    empresaNome: profissao.EmpresaNome,
                    empresaCnpj: profissao.EmpresaCnpj,
                    cargoNome: profissao.CargoNome));
            });

            await colecao.ReplaceOneAsync(w => w.Nome == cadPessoa.Nome, cadPessoa);

            return Ok(new
            {
                Mensagem = "Registro atualizado com sucesso!",
                Data = cadPessoa,
            });
        }

        [HttpDelete, Route("delete/{nome}")]
        public async Task<IActionResult> DeleteCadPessoaAsync(string nome, CancellationToken cancellationToken)
        {
            var bd = new Contexts.MongoDBContext();
            var colecao = bd.GetCollection<Models.CadPessoa>("CadPessoa");

            var cadPessoa = await colecao
                .AsQueryable()
                .Where(w => w.Nome == nome)
                .SingleOrDefaultAsync(cancellationToken);

            if (cadPessoa != null)
            {
                await colecao.DeleteOneAsync(w => w.Nome == nome);

                return Ok(new
                {
                    Mensagem = "Registro deletado com sucesso!",
                    Data = new
                    {
                        cadPessoa.Nome
                    }
                });
            }
            else
                return BadRequest(new
                {
                    Mensagem = "Nenhum registro encontrado!"
                });
                //return BadRequest("Nenhum registro encontrado");
                //return Ok(new
                //{
                //    Mensagem = "Nenhum registro encontrado!"
                //});



        }
    }
}
