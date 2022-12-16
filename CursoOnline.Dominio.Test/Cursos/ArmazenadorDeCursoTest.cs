using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test._Builders;
using CursoOnline.Dominio.Test._Utils;
using Moq;

namespace CursoOnline.Dominio.Test.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly CursoDto _cursoDTO;
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly Mock<ICursoRepositorio> _cursoRepositoryMock;
        private readonly Mock<IConversorDePublicoAlvo> _conversorDePublicoAlvo;
        
        public ArmazenadorDeCursoTest()
        {
            var faker = new Faker();

            _cursoDTO = new CursoDto
            {
                Nome = faker.Random.Word(),
                Descricao = faker.Lorem.Paragraph(),
                CargaHoraria = faker.Random.Double(50, 1000),
                PublicoAlvo = "Estudante",
                Valor = faker.Random.Double(1000, 2000)
            };

            _cursoRepositoryMock = new Mock<ICursoRepositorio>();
            _conversorDePublicoAlvo = new Mock<IConversorDePublicoAlvo>();
            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositoryMock.Object, _conversorDePublicoAlvo.Object);
        }

        [Fact]
        public void DeveAdicionarCurso()
        {
            _armazenadorDeCurso.Armazenar(_cursoDTO);

            _cursoRepositoryMock.Verify(r =>
                r.Adicionar(
                    It.Is<Curso>(c =>
                        c.Nome == _cursoDTO.Nome &&
                        c.Descricao == _cursoDTO.Descricao
                    )
                )
            );
        }

        [Fact]
        public void NaoDeveAdicionarCursoComMesmoNomeDeOutroJaSalvo()
        {
            //stub
            var cursoJaSalvo = CursoBuilder.Novo().ComId(123).ComNome(_cursoDTO.Nome).Build();
            _cursoRepositoryMock.Setup(r => r.ObterPeloNome(_cursoDTO.Nome)).Returns(cursoJaSalvo);

            Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem(Resource.NomeDoCursoJaExiste);
        }

        [Fact]
        public void NaoDeveInformarPublicoAlvoInvalido()
        {
            var publicoAlvoInvalido = "Medico";
            _cursoDTO.PublicoAlvo = publicoAlvoInvalido;
            Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem(Resource.PublicoAlvoInvalido);
        }


        [Fact]
        public void DeveAlterarDadosDoCurso()
        {
            _cursoDTO.Id = 323;
            var curso = CursoBuilder.Novo().Build();
            _cursoRepositoryMock.Setup(r => r.ObterPorId(_cursoDTO.Id)).Returns(curso);

            _armazenadorDeCurso.Armazenar(_cursoDTO);

            Assert.Equal(_cursoDTO.Nome, curso.Nome);
            Assert.Equal(_cursoDTO.Valor, curso.Valor);
            Assert.Equal(_cursoDTO.CargaHoraria, curso.CargaHoraria);
        }


        [Fact]
        public void NaoDeveAdicionarNoRepositorioQuandoCursoJaExiste()
        {
            _cursoDTO.Id = 323;
            var curso = CursoBuilder.Novo().Build();
            _cursoRepositoryMock.Setup(r => r.ObterPorId(_cursoDTO.Id)).Returns(curso);

            _armazenadorDeCurso.Armazenar(_cursoDTO);

            _cursoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
        }

    }
}
