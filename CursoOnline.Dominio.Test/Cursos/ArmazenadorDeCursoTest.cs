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
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly CursoDto _cursoDTO;
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
            var cursoJaSalvo = CursoBuilder.Novo().ComNome(_cursoDTO.Nome).Build();
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


    }
}
