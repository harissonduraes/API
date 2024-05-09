using API.Models;

namespace API.ViewModels
{
    public sealed class CadPessoaViewModel : BaseModel
    {
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public float Peso { get; set; }
        public float Altura { get; set; }
        public List<CProfissao> Profissoes { get; set; } = new List<CProfissao>();
        public class CProfissao
        {
            public string? EmpresaNome { get; set; }
            public string? CargoNome { get; set; }
            public string? EmpresaCnpj { get; set; }
        }
    }
}
