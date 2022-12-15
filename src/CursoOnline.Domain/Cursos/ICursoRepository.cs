using CursoOnline.Domain.Cursos;

namespace CursoOnline.Domain.Cursos
{
    public interface ICursoRepository
    {
        void Adicionar(Curso curso);
        Curso ObterPeloNome(string nome);
    }
}