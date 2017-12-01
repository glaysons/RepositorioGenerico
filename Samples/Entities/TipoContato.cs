using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{
	[Tabela("TiposDosContatos")]
	public class TipoContato : Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna(Nome = "CodTipoContato", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(50)]
		public string Nome { get; set; }

		#endregion

		#region Relacionamentos Descendentes

		[PropriedadeDeLigacaoEstrangeira("Tipo")]
		public virtual ICollection<ContatoDoCliente> ContatosDosClientes { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Tipo")]
		public virtual ICollection<ContatoDoFilho> ContatosDosFilhos { get; set; }

		#endregion

	}
}
