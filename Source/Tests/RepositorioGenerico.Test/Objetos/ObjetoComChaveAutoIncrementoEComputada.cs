using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoComChaveAutoIncrementoEComputada : Entidade
	{

		[Chave]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int CodigoUm { get; set; }

		[Chave]
		[AutoIncremento(Incremento.Calculado)]
		[Coluna(Ordem = 1, NomeDoTipo = "int")]
		public int CodigoDois { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 2, NomeDoTipo = "varchar")]
		public string Nome { get; set; }

	}
}
