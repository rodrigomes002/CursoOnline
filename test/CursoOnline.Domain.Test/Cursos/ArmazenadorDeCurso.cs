using CursoOnline.Domain.Cursos;

namespace CursoOnline.Domain.Test.Cursos
{
    public class ArmazenadorDeCurso
    {
        private readonly ICursoRepository _cursoRepository;
        public ArmazenadorDeCurso(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public void Armazenar(CursoDTO cursoDTO)
        {
            Enum.TryParse(typeof(PublicoAlvo), cursoDTO.PublicoAlvo, out var publicoAlvo);

            if (publicoAlvo == null) throw new ArgumentException("Publico Alvo inválido");

            var curso = new Curso(cursoDTO.Nome, cursoDTO.CargaHoraria, (PublicoAlvo)publicoAlvo, cursoDTO.Valor, cursoDTO.Descricao);
            _cursoRepository.Adicionar(curso);
        }
    }
}
