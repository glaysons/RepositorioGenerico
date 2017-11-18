using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace Entities
{
	[Tabela("ContatosDosClientes")]
	public class ContatoDoCliente : Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodContatoCliente", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(Nome = "CodCliente", NomeDoTipo = "int")]
		public int IdCliente { get; set; }

		[Obrigatorio, Coluna(Nome = "CodTipoContato", NomeDoTipo = "int")]
		public int IdTipoContato { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar")]
		public string Nome { get; set; }

		[Coluna(NomeDoTipo = "varchar")]
		public string Telefone { get; set; }

		[Coluna(NomeDoTipo = "varchar")]
		public string Email { get; set; }

		/// <summary>
		/// Relacionamento Ascendente
		/// </summary>

		[ChaveEstrangeira("IdCliente")]
		public virtual Cliente Cliente { get; set; }

		[ChaveEstrangeira("IdTipoContato")]
		public virtual TipoContato Tipo { get; set; }

	}
}
