using CursoOnline.Domain.Cursos;

namespace CursoOnline.Domain.Test.Cursos
{
    public interface ICursoRepository
    {
        void Adicionar(Curso curso);
    }
}