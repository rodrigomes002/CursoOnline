using Bogus;
using CursoOnline.Domain.Cursos;
using CursoOnline.Domain.Test._Builders;
using CursoOnline.Domain.Test._Utils;
using ExpectedObjects;
using Xunit.Abstractions;

namespace CursoOnline.Domain.Test.Cursos
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

            var curso = new Curso(custoEsperado.Nome, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor, custoEsperado.Descricao);

            custoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCursoTerUmNomeInvalido(string nomeInvalido)
        {
            //exemplo com assert comum
            var ex = Assert.Throws<ArgumentException>(() => CursoBuilder.Novo().ComNome(nomeInvalido).Build());
            Assert.Equal("Nome inválido", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerUmaCargaHorariaMenorQueUm(double carcaHorariaInvalida)
        {
            //exemplo com método de extensão
            Assert.Throws<ArgumentException>(() => CursoBuilder.Novo().ComCargaHoraria(carcaHorariaInvalida).Build())
                .ComMensagem("Carga horária inválida");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerValorMenorQueUm(double valorInvalido)
        {
            var ex = Assert.Throws<ArgumentException>(() => CursoBuilder.Novo().ComValor(valorInvalido).Build());
            Assert.Equal("Valor inválido", ex.Message);
        }

        
    }
}
