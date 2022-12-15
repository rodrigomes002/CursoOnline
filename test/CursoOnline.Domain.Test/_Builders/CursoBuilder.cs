using CursoOnline.Domain.Cursos;

namespace CursoOnline.Domain.Test._Builders
{
    //design pattern builder
    public class CursoBuilder
    {
        private string _nome = "Informática básica";
        private double _cargaHoraria = 80;
        private PublicoAlvo _publicoAlvo = PublicoAlvo.Estudante;
        private double _valor = 950;
        private string _descricao = "Uma descrição";

        public static CursoBuilder Novo()
        {
            return new CursoBuilder();
        } 

        public CursoBuilder ComNome(string nome)
        {
            this._nome = nome;
            return this;
        }

        public CursoBuilder ComDescricao(string descricao)
        {
            this._descricao = descricao;
            return this;
        }

        public CursoBuilder ComCargaHoraria(double cargaHoraria)
        {
            this._cargaHoraria = cargaHoraria;
            return this;
        }

        public CursoBuilder ComValor(double valor)
        {
            this._valor = valor;
            return this;
        }

        public CursoBuilder ComPublicoAlvo(PublicoAlvo publicoAlvo)
        {
            this._publicoAlvo = publicoAlvo;
            return this;
        }

        public Curso Build()
        {
            return new Curso(_nome, _cargaHoraria, _publicoAlvo, _valor, _descricao);
        }
    }
}
