using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test
{
	public class FilhoMistoDoObjetoDeTestes : Entidade
	{

		[Chave]
		[Obrigatorio]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, Nome = "CodigoFilho", NomeDoTipo = "int")]
		public int AliasCodigoFilho { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 1, Nome = "NomeFilho", NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		public string AliasNomeFilho { get; set; }

		[Coluna(Ordem = 2, Nome = "CodigoPai", NomeDoTipo = "int")]
		public int AliasCodigoPai { get; set; }

		[ChaveEstrangeira("AliasCodigoPai")]
		public virtual ObjetoMistoDeTestes AliasPai { get; set; }

	}
}
