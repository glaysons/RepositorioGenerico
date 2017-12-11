using RepositorioGenerico.Dictionary.Relacionamentos;
using System;

namespace RepositorioGenerico.Fake
{
	internal class DadosRelacionamento
	{

		public TiposRelacionamento Tipo { get; private set; }

		public string Tabela { get; private set; }

		public string CamposEstrangeiros { get; private set; }

		public string Consulta { get; private set; }

		public DadosRelacionamento(string commandText)
		{
			var dados = commandText.Split('|');

			if (dados.Length < 4)
				throw new Exception("Dados de relacionamento inválido!");

			Tipo = (dados[0] == "asc")
				? TiposRelacionamento.Ascendente
				: TiposRelacionamento.Descendente;
			Tabela = dados[1];
			CamposEstrangeiros = dados[2];
			Consulta = string.Join("|", dados, 3, dados.Length - 3);
		}

	}
}
