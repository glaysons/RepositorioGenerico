using System.Collections.Generic;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	[Tabela("ObjetoVirtualFilho")]
	public class FilhoDoObjetoDeTestes : Entidade
	{

		[Chave, Obrigatorio, AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int CodigoFilho { get; set; }

		[Obrigatorio, TamanhoMaximo(50)]
		[Coluna(Ordem = 1, NomeDoTipo = "varchar")]
		public string NomeFilho { get; set; }

		[Coluna(Ordem = 2, NomeDoTipo = "int")]
		public int CodigoPai { get; set; }

		[ChaveEstrangeira("CodigoPai")]
		public virtual ObjetoDeTestes Pai { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Filho")]
		public virtual ICollection<NetoDoObjetoDeTestes> Netos { get; set; }

	}
}
