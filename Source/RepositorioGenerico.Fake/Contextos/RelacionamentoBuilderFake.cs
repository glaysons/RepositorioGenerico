using RepositorioGenerico.Search;
using System.Collections.Generic;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Relacionamentos;
using System.Text;

namespace RepositorioGenerico.Fake.Contextos
{
	public class RelacionamentoBuilderFake : IRelacionamentoBuilder
	{

		public string CriarScriptConsultaRelacionamentoAscendente(Relacionamento relacionamento, string condicao)
		{
			return string.Concat("asc|", 
				relacionamento.Dicionario.Nome, "|",
				ConsultarCamposEstrangeirosDoRelacionamento(relacionamento), "|", 
				condicao);
		}

		private static string ConsultarCamposEstrangeirosDoRelacionamento(Relacionamento relacionamento)
		{
			var campos = new StringBuilder();
			foreach (var campo in relacionamento.ChaveEstrangeira)
			{
				var item = relacionamento.Dicionario.ConsultarPorPropriedade(campo);
				campos.Append(item.Nome);
				campos.Append(",");
			}
			campos.Length -= 1;
			return campos.ToString();
		}

		public string CriarScriptConsultaRelacionamentoDescendente(Relacionamento relacionamento, string condicao, IEnumerable<ItemDicionario> camposChave)
		{
			return string.Concat("desc|", 
				relacionamento.Dicionario.Nome, "|",
				ConsultarCamposEstrangeirosDoRelacionamento(relacionamento), "|",
				condicao);
		}

	}
}
