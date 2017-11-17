using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoApenasComCamposChave: Entidade
	{

		[Chave]
		[Obrigatorio]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int CodigoUm { get; set; }

		[Chave]
		[Obrigatorio]
		[Coluna(Ordem = 1, NomeDoTipo = "int")]
		public int CodigoDois { get; set; }

	}
}
