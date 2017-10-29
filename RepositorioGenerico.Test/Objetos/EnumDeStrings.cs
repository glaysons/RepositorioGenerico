using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Test.Objetos
{
	public enum EnumDeStrings
	{

		[Descricao("Primeira Opção"), ValorPadrao("A")]
		OpcaoA = 1,

		[Descricao("Segunda Opção"), ValorPadrao("B")]
		OpcaoB = 2,

		[Descricao("Terceira Opção"), ValorPadrao("C")]
		OpcaoC = 3,

		SemOpcao
	}
}
