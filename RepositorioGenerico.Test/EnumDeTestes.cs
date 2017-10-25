using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test
{
	public enum EnumDeTestes
	{

		[Descricao("Primeira Opção")]
		Opcao1 = 1,

		[Descricao("Segunda Opção")]
		Opcao2 = 2,

		[Descricao("Terceira Opção")]
		Opcao3 = 3,

		SemOpcao

	}
}
