using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoComChaveTexto : Entidade
	{

		[Chave, Coluna(Ordem = 0, NomeDoTipo = "varchar"), TamanhoMaximo(50), Requerido]
		public string ChaveTexto { get; set; }

		[Obrigatorio]
		[Coluna(Ordem = 1, NomeDoTipo = "varchar"), TamanhoMaximo(50)]
		public string Nome { get; set; }

	}
}
