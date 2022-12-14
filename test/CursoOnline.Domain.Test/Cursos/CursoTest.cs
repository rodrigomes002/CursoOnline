using ExpectedObjects;

namespace CursoOnline.Domain.Test.Cursos
{
    public class CursoTest
    {
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

            var curso = new Curso(custoEsperado.Nome, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor);

            custoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveCursoTerUmNomeInvalido(string nomeInvalido)
        {
            var custoEsperado = new
            {
                Nome = nomeInvalido,
                CargaHoraria = (double)80,
                PublicoAlvo = PublicoAlvo.Estudante,
                Valor = (double)950,
            };

            var ex = Assert.Throws<ArgumentException>(() => new Curso(custoEsperado.Nome, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor));
            Assert.Equal("Nome inválido", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerUmaCargaHorariaMenorQueUm(double carcaHorariaInvalida)
        {
            var custoEsperado = new
            {
                Nome = "Informática básica",
                CargaHoraria = carcaHorariaInvalida,
                PublicoAlvo = PublicoAlvo.Estudante,
                Valor = (double)950,
            };

            var ex = Assert.Throws<ArgumentException>(() => new Curso(custoEsperado.Nome, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor));
            Assert.Equal("Carga horária inválida", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerValorMenorQueUm(double valorInvalido)
        {
            var custoEsperado = new
            {
                Nome = "Informática básica",
                CargaHoraria = (double)80,
                PublicoAlvo = PublicoAlvo.Estudante,
                Valor = valorInvalido,
            };

            var ex = Assert.Throws<ArgumentException>(() => new Curso(custoEsperado.Nome, custoEsperado.CargaHoraria, custoEsperado.PublicoAlvo, custoEsperado.Valor));
            Assert.Equal("Valor inválido", ex.Message);
        }
    }
}
