using ConverterBancoParaEntidades.Constantes;

namespace ConverterBancoParaEntidades.Interfaces
{
	public interface IConfiguracao
	{

		string Conexao { get; }

		string ScriptConsultaTabelas { get; }

		string PastaDeDestino { get; }

		Linguagem Linguagem { get; }

		string[] Usings { get; }

		string Namespace { get; }

		string HerancaPadrao { get; }

		string[] Tabelas { get; }

		void AdicionarLog(params string[] mensagem);

	}
}
