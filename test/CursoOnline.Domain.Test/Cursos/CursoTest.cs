namespace CursoOnline.Domain.Test.Cursos
{    
    public class CursoTest
    {
        [Fact]
        public void DeveCriarCurso()
        {
            const string nome = "Informática básica";
            const double cargaHoraria = 80;
            const string publicoAlvo = "Estudantes";
            const double valor = 950;

            var curso = new Curso(nome, cargaHoraria, publicoAlvo, valor);
            
            Assert.Equal(nome, curso.Nome);
            Assert.Equal(cargaHoraria, curso.CargaHoraria);
            Assert.Equal(publicoAlvo, curso.PublicoAlvo);
            Assert.Equal(valor, curso.Valor);
        }
    }
}
