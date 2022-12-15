using Bogus;
using CursoOnline.Domain.Cursos;
using CursoOnline.Domain.Test._Builders;
using CursoOnline.Domain.Test._Utils;
using Moq;

namespace CursoOnline.Domain.Test.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly CursoDTO _cursoDTO;
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;

        public ArmazenadorDeCursoTest()
        {
            var faker = new Faker();

            _cursoDTO = new CursoDTO
            {
                Nome = faker.Random.Word(),
                Descricao = faker.Lorem.Paragraph(),
                CargaHoraria = faker.Random.Double(50, 1000),
                PublicoAlvo = "Estudante",
                Valor = faker.Random.Double(1000, 2000)
            };

            _cursoRepositoryMock = new Mock<ICursoRepository>();
            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositoryMock.Object);
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

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem("Nome do curso já consta no banco de dados");
        }

        [Fact]
        public void NaoDeveInformarPublicoAlvoInvalido()
        {
            var publicoAlvoInvalido = "Medico";
            _cursoDTO.PublicoAlvo = publicoAlvoInvalido;
            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem("Publico Alvo inválido");
        }

       
    }
}
