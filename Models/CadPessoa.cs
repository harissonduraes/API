﻿namespace API.Models
{
    public class CadPessoa
    {
        public string? Nome { get; private set; }
        public int Idade { get; private set; }
        public float Peso { get; private set; }
        public float Altura { get; private set; }

        public List<CProfissao> Profissoes { get; private set; } = new List<CProfissao>();

        public CadPessoa(string? nome, int idade)
        {
            Nome = nome;
            Idade = idade;
        }

        public void AlterarAltura(float altura)
        {
            Altura = altura;
        }

        public void AlterarPeso(float peso)
        {
            Peso = peso;
        }

        public void AddProfissao(CProfissao profissao)
        {
            Profissoes.Add(profissao);
        }
        #region Classes
        public class CProfissao
        {
            public CProfissao(string empresaNome, string cargoNome, string empresaCnpj)
            {
                EmpresaNome = empresaNome;
                CargoNome = cargoNome;
                EmpresaCnpj = empresaCnpj;
            }

            public string EmpresaNome { get; private set; }
            public string CargoNome { get; private set; }
            public string EmpresaCnpj { get; private set; }
        }
        #endregion
    }


}