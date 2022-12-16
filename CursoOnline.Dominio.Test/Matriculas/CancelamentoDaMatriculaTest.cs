using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.Dominio.Test._Utils;
using CursoOnline.Test._Builders;
using Moq;

namespace CursoOnline.Test.Matriculas
{
    public class CancelamentoDaMatriculaTest
    {
        private readonly Mock<IMatriculaRepositorio> _matriculaRepositorio;
        private readonly CancelamentoDaMatricula _cancelamentoDaMatricula;

        public CancelamentoDaMatriculaTest()
        {
            _matriculaRepositorio = new Mock<IMatriculaRepositorio>();
            _cancelamentoDaMatricula = new CancelamentoDaMatricula(_matriculaRepositorio.Object);
        }

        [Fact]
        public void DeveCancelarMatricula()
        {
            var matricula = MatriculaBuilder.Novo().Build();
            _matriculaRepositorio.Setup(r => r.ObterPorId(matricula.Id)).Returns(matricula);

            _cancelamentoDaMatricula.Cancelar(matricula.Id);

            Assert.True(matricula.Cancelada);
        }

        [Fact]
        public void DeveNotificarQuandoMatriculaNaoEncontrada()
        {
            Matricula matriculaInvalida = null;
            const int matriculaIdInvalida = 1;
            _matriculaRepositorio.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns(matriculaInvalida);

            Assert.Throws<ExcecaoDeDominio>(() =>
                    _cancelamentoDaMatricula.Cancelar(matriculaIdInvalida))
                .ComMensagem(Resource.MatriculaNaoEncontrada);
        }
    }
}
