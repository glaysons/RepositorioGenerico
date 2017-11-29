using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace Entities
{
	[Tabela("ContatosDosFilhos")]
	public class ContatoDoFilho : Entidade
	{

		#region Estrutura da Tabela

		[Chave, Coluna(Nome = "CodContatoFilho", NomeDoTipo = "int"), AutoIncremento(Incremento.Identity)]
		public int Id { get; set; }

		[Obrigatorio, Coluna(Nome = "CodFilho", NomeDoTipo = "int")]
		public int IdFilho { get; set; }

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

		[ChaveEstrangeira("IdFilho")]
		public virtual Filho Filho { get; set; }

		[ChaveEstrangeira("IdTipoContato")]
		public virtual TipoContato Tipo { get; set; }

		#endregion

	}
}
