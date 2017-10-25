using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test
{
	public class ObjetoSemChaveDefinida: Entidade
	{

		[Obrigatorio]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int Codigo { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 1, NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		public string Nome { get; set; }

	}
}
