using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{

	[Tabela("Cidades")]
	public class Cidade: Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna(Nome = "CodCidade", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(5), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(2), TamanhoMaximo(2)]
		public string Estado { get; set; }

		#endregion

		#region Relacionamentos Descendentes

		[PropriedadeDeLigacaoEstrangeira("Cidade")]
		public virtual ICollection<Cliente> Clientes { get; set; }

		#endregion

	}
}
