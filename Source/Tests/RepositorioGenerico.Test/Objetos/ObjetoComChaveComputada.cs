using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoComChaveComputada: Entidade
	{

		[Chave]
		[AutoIncremento(Incremento.Calculado)]
		[Coluna(Ordem = 0, NomeDoTipo = "int")]
		public int Codigo { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 1, NomeDoTipo = "varchar")]
		public string Nome { get; set; }

	}
}
