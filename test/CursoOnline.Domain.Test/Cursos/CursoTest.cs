using CursoOnline.Domain.Test._Utils;
using ExpectedObjects;
using Xunit.Abstractions;

namespace CursoOnline.Domain.Test.Cursos
{
    public class CursoTest : IDisposable
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly string nome;
        private readonly double cargaHoraria;
        private readonly PublicoAlvo publicoAlvo;
        private readonly double valor;

        //setup
        public CursoTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.outputHelper.WriteLine("Construtor sendo executado!");

            nome = "Informática básica";
            cargaHoraria = 80;
            publicoAlvo = PublicoAlvo.Estudante;
            valor = 950;
        }

        //cleanup
        public void Dispose()
        {
            this.outputHelper.WriteLine("Dispose sendo executado!");
        }

        [Fact]
        public void DeveCriarCurso()
        {
            var custoEsperado = new
            {
                Nome = "Informática básica",
                CargaHoraria = (double)80,
                PublicoAlvo = PublicoAlvo.Estudante,
                Valor = (double)950,
            };

            var curso = new Curso(nome, cargaHoraria, publicoAlvo, valor);

            custoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCursoTerUmNomeInvalido(string nomeInvalido)
        {
            //exemplo com assert comum
            var ex = Assert.Throws<ArgumentException>(() => new Curso(nomeInvalido, cargaHoraria, publicoAlvo, valor));
            Assert.Equal("Nome inválido", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerUmaCargaHorariaMenorQueUm(double carcaHorariaInvalida)
        {
            //exemplo com método de extensão
            Assert.Throws<ArgumentException>(() => new Curso(nome, carcaHorariaInvalida, publicoAlvo, valor))
                .ComMensagem("Carga horária inválida");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerValorMenorQueUm(double valorInvalido)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Curso(nome, cargaHoraria, publicoAlvo, valorInvalido));
            Assert.Equal("Valor inválido", ex.Message);
        }

        
    }
}
