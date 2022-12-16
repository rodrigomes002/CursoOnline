using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.PublicosAlvo;
using CursoOnline.Dominio.Test._Builders;
using CursoOnline.Dominio.Test._Utils;
using ExpectedObjects;

namespace CursoOnline.Dominio.Test.Cursos
{
    public class CursoTest
    {
        private readonly string _nome;
        private readonly double _cargaHoraria;
        private readonly PublicoAlvo _publicoAlvo;
        private readonly double _valor;
        private readonly string _descricao;
        private readonly Faker _faker;
        
        public CursoTest()
        {
            //bogus - dados aleatórios
            _faker = new Faker();

            _nome = _faker.Random.Word();
            _cargaHoraria = _faker.Random.Double(50, 1000);
            _publicoAlvo = PublicoAlvo.Estudante;
            _valor = _faker.Random.Double(100, 1000);
            _descricao = _faker.Lorem.Paragraph();
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
            Assert.Throws<ExcecaoDeDominio>(() => CursoBuilder.Novo().ComNome(nomeInvalido).Build())
                .ComMensagem(Resource.NomeInvalido);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveCursoTerUmaCargaHorariaInvalida(double carcaHorariaInvalida)
        {
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
            var nomeEsperado = _faker.Person.FullName;
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

        [Fact]
        public void DeveAlterarCargaHoraria()
        {
            var cargaHorariaEsperado = 20.5;
            var curso = CursoBuilder.Novo().Build();
            curso.AlterarCargaHoraria(cargaHorariaEsperado);
            Assert.Equal(cargaHorariaEsperado, curso.CargaHoraria);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveAlterarComCargaHorariaInvalido(double cargaHorariaInvalida)
        {
            var curso = CursoBuilder.Novo().Build();

            Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarCargaHoraria(cargaHorariaInvalida))
                .ComMensagem(Resource.CargaHorariaInvalida);
        }

        [Fact]
        public void DeveAlterarValor()
        {
            var valorEsperado = 2023.20;
            var curso = CursoBuilder.Novo().Build();
            curso.AlterarValor(valorEsperado);
            Assert.Equal(valorEsperado, curso.Valor);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void NaoDeveAlterarComValorInvalido(double valorInvalida)
        {
            var curso = CursoBuilder.Novo().Build();

            Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarValor(valorInvalida))
                .ComMensagem(Resource.ValorInvalido);
        }

    }
}
