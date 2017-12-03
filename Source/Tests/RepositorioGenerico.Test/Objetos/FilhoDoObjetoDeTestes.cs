using System.Collections.Generic;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	[Tabela("ObjetoVirtualFilho")]
	public class FilhoDoObjetoDeTestes : Entidade
	{

		[Chave, Obrigatorio, AutoIncremento(Incremento.Identity)]
		[Coluna(Nome = "CodigoFilho", Ordem = 0, NomeDoTipo = "int")]
		public int Id { get; set; }

		[Obrigatorio, TamanhoMaximo(50)]
		[Coluna(Nome = "NomeFilho", Ordem = 1, NomeDoTipo = "varchar")]
		public string Nome { get; set; }

		[Coluna(Nome = "CodigoPai", Ordem = 2, NomeDoTipo = "int")]
		public int IdPai { get; set; }

		[ChaveEstrangeira("IdPai")]
		public virtual ObjetoDeTestes Pai { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Filho")]
		public virtual ICollection<NetoDoObjetoDeTestes> Netos { get; set; }

	}
}
