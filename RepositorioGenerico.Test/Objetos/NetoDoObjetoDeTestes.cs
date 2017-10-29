using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{

	[Tabela("TabelaNetos")]
	public class NetoDoObjetoDeTestes : Entidade
	{

		[Chave]
		[Obrigatorio]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int CodigoNeto { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 1, NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		public string NomeNeto { get; set; }

		[Coluna(Ordem = 2, NomeDoTipo = "int")]
		public int CodigoFilho { get; set; }

		[Coluna(Ordem = 3, Nome = "CampoComOpcoesInteiras", NomeDoTipo = "int")]
		public EnumDeTestes Opcao { get; set; }

		[Coluna(Ordem = 4, Nome = "CampoComOpcoesString", NomeDoTipo = "varchar"), TamanhoMaximo(1)]
		public EnumDeStrings Letra { get; set; }

		[ChaveEstrangeira("CodigoFilho")]
		public virtual FilhoDoObjetoDeTestes Filho { get; set; }

	}
}
