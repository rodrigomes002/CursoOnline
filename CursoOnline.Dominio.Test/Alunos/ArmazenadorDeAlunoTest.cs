using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test._Utils;
using CursoOnline.Test._Builders;
using Moq;

namespace AlunoOnline.Test.Alunos
{
    public class ArmazenadorDeAlunoTest
    {
        private readonly AlunoDto _alunoDto;
        private readonly ArmazenadorDeAluno _armazenadorDeAluno;
        private readonly Mock<IAlunoRepositorio> _alunoRepositorio;
        private readonly Faker _faker;

        public ArmazenadorDeAlunoTest()
        {
            _faker = new Faker();

            _alunoDto = new AlunoDto
            {
                Nome = _faker.Random.Word(),
                Cpf = _faker.Person.Cpf(),
                Email = _faker.Person.Email,
                PublicoAlvo = "Estudante",
            };

            _alunoRepositorio = new Mock<IAlunoRepositorio>();
            var conversorDePublicoAlvo = new Mock<IConversorDePublicoAlvo>();
            _armazenadorDeAluno = new ArmazenadorDeAluno(_alunoRepositorio.Object, conversorDePublicoAlvo.Object);
        }

        [Fact]
        public void DeveAdicionarAluno()
        {
            _armazenadorDeAluno.Armazenar(_alunoDto);

            _alunoRepositorio.Verify(r => r.Adicionar(It.Is<Aluno>(a => a.Nome == _alunoDto.Nome)));
        }

        [Fact]
        public void NaoDeveAdicionarAlunoQuandoCpfJaFoiCadastrado()
        {
            var alunoComMesmoCpf = AlunoBuilder.Novo().ComId(34).Build();
            _alunoRepositorio.Setup(r => r.ObterPeloCpf(_alunoDto.Cpf)).Returns(alunoComMesmoCpf);

            Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeAluno.Armazenar(_alunoDto))
                .ComMensagem(Resource.CpfJaCadastrado);
        }

        [Fact]
        public void DeveEditarNomeDoAluno()
        {
            _alunoDto.Id = 35;
            _alunoDto.Nome = _faker.Person.FullName;
            var alunoJaSalvo = AlunoBuilder.Novo().Build();
            _alunoRepositorio.Setup(r => r.ObterPorId(_alunoDto.Id)).Returns(alunoJaSalvo);

            _armazenadorDeAluno.Armazenar(_alunoDto);

            Assert.Equal(_alunoDto.Nome, alunoJaSalvo.Nome);
        }

        [Fact]
        public void NaoDeveEditarDemaisInformacoesDoAluno()
        {
            _alunoDto.Id = 35;
            var alunoJaSalvo = AlunoBuilder.Novo().Build();
            var cpfEsperado = alunoJaSalvo.Cpf;
            var emailEsperado = alunoJaSalvo.Email;
            var publicoAlvoEsperado = alunoJaSalvo.PublicoAlvo;
            _alunoRepositorio.Setup(r => r.ObterPorId(_alunoDto.Id)).Returns(alunoJaSalvo);

            _armazenadorDeAluno.Armazenar(_alunoDto);

            Assert.Equal(cpfEsperado, alunoJaSalvo.Cpf);
            Assert.Equal(emailEsperado, alunoJaSalvo.Email);
            Assert.Equal(publicoAlvoEsperado, alunoJaSalvo.PublicoAlvo);
        }

        [Fact]
        public void NaoDeveAdicionarQuandoForEdicao()
        {
            _alunoDto.Id = 35;
            var alunoJaSalvo = AlunoBuilder.Novo().Build();
            _alunoRepositorio.Setup(r => r.ObterPorId(_alunoDto.Id)).Returns(alunoJaSalvo);

            _armazenadorDeAluno.Armazenar(_alunoDto);

            _alunoRepositorio.Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Never);
        }

    }
}
