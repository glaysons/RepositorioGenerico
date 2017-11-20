using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Fake.Contextos
{
	[Tabela("RepositorioGenerico.Fake.Contextos.Procedure")]
	internal class Procedure : Entidade
	{

		[Coluna]
		public object Valor { get; set; }

		[Chave, Coluna]
		public string Nome { get; set; }

		[Coluna]
		public int RegistrosAfetados { get; set; }

	}
}
