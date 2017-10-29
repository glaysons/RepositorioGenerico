using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoSemValidacao : Entidade
	{
		[Chave]
		[AutoIncremento(Incremento.Identity)]
		[Coluna(Ordem = 1, NomeDoTipo = "int")]
		public int Codigo { get; set; }

		[Coluna(Ordem = 2, NomeDoTipo = "varchar")]
		[TamanhoMaximo(50)]
		public string Nome { get; set; }

	}
}
