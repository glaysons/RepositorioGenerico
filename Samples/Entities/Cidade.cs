using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using System.Collections.Generic;

namespace Entities
{

	[Tabela("Cidades")]
	public class Cidade: Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodCidade", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(10), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMinimo(2), TamanhoMaximo(2)]
		public string Estado { get; set; }

		/// <summary>
		/// Relacionamento Descendente
		/// </summary>

		[PropriedadeDeLigacaoEstrangeira("Cidade")]
		public ICollection<Cliente> Clientes { get; set; }


	}
}
