using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test._Utils;
using CursoOnline.Test._Builders;
using ExpectedObjects;

namespace CursoOnline.Test.Alunos
{
    public class AlunoTest
    {
        private readonly string _nome;
        private readonly string _cpf;
        private readonly string _email;
        private readonly PublicoAlvo _publicoAlvo;
        private readonly Faker _faker;

        public AlunoTest()
        {
            _faker = new Faker();

            _nome = _faker.Random.Word();
            _cpf = _faker.Person.Cpf();
            _email = _faker.Person.Email;
            _publicoAlvo = PublicoAlvo.Empreendedor;
        }

        [Fact]
        public void DeveCriarAluno()
        {
            var alunoEsperado = new
            {
                Nome = _nome,
                Cpf = _cpf,
                Email = _email,
                PublicoAlvo = _publicoAlvo,
            };

            var aluno = new Aluno(alunoEsperado.Nome, alunoEsperado.Email, alunoEsperado.Cpf, alunoEsperado.PublicoAlvo);

            alunoEsperado.ToExpectedObject().ShouldMatch(aluno);
        }


        [Fact]
        public void DeveAlterarNome()
        {
            var novoNomeEsperado = _faker.Person.FullName;

            var aluno = AlunoBuilder.Novo().Build();
            aluno.AlterarNome(novoNomeEsperado);

            Assert.Equal(novoNomeEsperado, aluno.Nome);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCriarAlunoComNomeInvalido(string nomeInvalido)
        {
            Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComNome(nomeInvalido).Build())
                .ComMensagem(Resource.NomeInvalido);

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("cpf invalido")]
        [InlineData("000000")]
        public void NaoDeveCriarAlunoComCpfInvalido(string cpfInvalido)
        {
            Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComCpf(cpfInvalido).Build())
                .ComMensagem(Resource.CpfInvalido);

        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("email invalido")]
        [InlineData("email@email")]
        public void NaoDeveCriarAlunoComEmailInvalido(string emailInvalido)
        {
            Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComEmail(emailInvalido).Build())
                .ComMensagem(Resource.EmailInvalido);

        }
    }
}
