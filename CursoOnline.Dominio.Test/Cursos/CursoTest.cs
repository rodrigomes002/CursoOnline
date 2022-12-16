using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test._Builders;
using CursoOnline.Dominio.Test._Utils;
using ExpectedObjects;
using Xunit.Abstractions;

namespace CursoOnline.Dominio.Test.Cursos
{
    public class CursoTest : IDisposable
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _nome;
        private readonly double _cargaHoraria;
        private readonly PublicoAlvo _publicoAlvo;
        private readonly double _valor;
        private readonly string _descricao;

        //setup
        public CursoTest(ITestOutputHelper outputHelper)
        {
            this._outputHelper = outputHelper;
            this._outputHelper.WriteLine("Construtor sendo executado!");

            //bogus - dados aleatórios
            var faker = new Faker();

            _nome = faker.Random.Word();
            _cargaHoraria = faker.Random.Double(50, 1000);
            _publicoAlvo = PublicoAlvo.Estudante;
            _valor = faker.Random.Double(100, 1000);
            _descricao = faker.Lorem.Paragraph();
        }

        //cleanup
        public void Dispose()
        {
            this._outputHelper.WriteLine("Dispose sendo executado!");
        }

        [Fact]
        public void DeveCriarCurso()
        {
            var custoEsperado = new
            {
                Nome = _nome,
                CargaHoraria = _cargaHoraria,
                PublicoAlvo = _publicoAlvo,
                Valor = _valor,
                Descricao = _descricao
            };

            var curso = new Curso(custoEsperado.Nome, custoEsperado.Descricao, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor);

            custoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCursoTerUmNomeInvalido(string nomeInvalido)
        {
            //exemplo com assert comum
            Assert.Throws<ExcecaoDeDominio>(() => CursoBuilder.Novo().ComNome(nomeInvalido).Build())
                .ComMensagem(Resource.NomeInvalido);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerUmaCargaHorariaMenorQueUm(double carcaHorariaInvalida)
        {
            //exemplo com método de extensão
            Assert.Throws<ExcecaoDeDominio>(() => CursoBuilder.Novo().ComCargaHoraria(carcaHorariaInvalida).Build())
                .ComMensagem(Resource.CargaHorariaInvalida);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerValorMenorQueUm(double valorInvalido)
        {
            Assert.Throws<ExcecaoDeDominio>(() => CursoBuilder.Novo().ComValor(valorInvalido).Build())
                .ComMensagem(Resource.ValorInvalido);
        }

        [Fact]
        public void DeveAlterarNome()
        {
            var nomeEsperado = "José";
            var curso = CursoBuilder.Novo().Build();
            curso.AlterarNome(nomeEsperado);
            Assert.Equal(nomeEsperado, curso.Nome);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveAlterarComNomeInvalido(string nomeInvalido)
        {
            var curso = CursoBuilder.Novo().Build();

            Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarNome(nomeInvalido))
                .ComMensagem(Resource.NomeInvalido);
        }

    }
}
