using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace Entities
{
	[Tabela("ContatosDosFilhos")]
	public class ContatoDoFilho : Entidade
	{

		/// <summary>
		/// Estrutura da Tabela
		/// </summary>

		[Chave, Coluna(Nome = "CodContatoFilho", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(Nome = "CodFilho", NomeDoTipo = "int")]
		public int IdFilho { get; set; }

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

		[ChaveEstrangeira("IdFilho")]
		public virtual Filho Filho { get; set; }

		[ChaveEstrangeira("IdTipoContato")]
		public virtual TipoContato Tipo { get; set; }

	}
}
