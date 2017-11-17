using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoSemHerancaCorreta
	{
		[Chave]
		[AutoIncremento(Incremento.Nenhum)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int ChaveBase { get; set; }

		[Chave]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 1, NomeDoTipo = "int")]
		public int ChaveAutoIncremento { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 2, NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		public string Nome { get; set; }

	}
}
