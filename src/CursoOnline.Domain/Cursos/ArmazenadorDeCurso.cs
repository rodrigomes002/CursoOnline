namespace CursoOnline.Domain.Cursos
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
            var cursoJaSalvo = _cursoRepository.ObterPeloNome(cursoDTO.Nome);

            if (cursoJaSalvo != null) throw new ArgumentException("Nome do curso já consta no banco de dados");

            if(!Enum.TryParse<PublicoAlvo>(cursoDTO.PublicoAlvo, out var publicoAlvo))
                throw new ArgumentException("Publico Alvo inválido");

            var curso = new Curso(cursoDTO.Nome, cursoDTO.CargaHoraria, publicoAlvo, cursoDTO.Valor, cursoDTO.Descricao);
            _cursoRepository.Adicionar(curso);
        }
    }
}
