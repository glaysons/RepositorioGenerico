using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace Entities
{
	[Tabela("ContatosDosClientes")]
	public class ContatoDoCliente : Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna(Nome = "CodContatoCliente", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(Nome = "CodCliente", NomeDoTipo = "int")]
		public int IdCliente { get; set; }

		[Obrigatorio, Coluna(Nome = "CodTipoContato", NomeDoTipo = "int")]
		public int IdTipoContato { get; set; }

		[Obrigatorio, Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(50)]
		public string Nome { get; set; }

		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(50)]
		public string Telefone { get; set; }

		[Coluna(NomeDoTipo = "varchar"), TamanhoMaximo(250)]
		public string Email { get; set; }

		#endregion

		#region Relacionamentos Ascendentes

		[ChaveEstrangeira("IdCliente")]
		public virtual Cliente Cliente { get; set; }

		[ChaveEstrangeira("IdTipoContato")]
		public virtual TipoContato Tipo { get; set; }

		#endregion

	}
}
