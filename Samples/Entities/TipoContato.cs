using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{
	[Tabela("TiposDosContatos")]
	public class TipoContato : Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodFilho", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar")]
		public string Nome { get; set; }

		/// <summary>
		/// Relacionamento Descendente
		/// </summary>

		[PropriedadeDeLigacaoEstrangeira("Tipo")]
		public virtual ICollection<ContatoDoCliente> ContatosDosClientes { get; set; }

		[PropriedadeDeLigacaoEstrangeira("Tipo")]
		public virtual ICollection<ContatoDoFilho> ContatosDosFilhos { get; set; }

	}
}
