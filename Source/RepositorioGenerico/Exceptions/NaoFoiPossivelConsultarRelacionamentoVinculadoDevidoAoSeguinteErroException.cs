using System;

namespace RepositorioGenerico.Exceptions
{
	public class NaoFoiPossivelConsultarRelacionamentoVinculadoDevidoAoSeguinteErroException : Exception
	{
		public NaoFoiPossivelConsultarRelacionamentoVinculadoDevidoAoSeguinteErroException(string origem, string tipo, string nome, string mensagem)
			: base(string.Concat("Não foi possível consultar na tabela [", origem, "] o relacionamento [", tipo, "] com a tabela [", nome, 
				"] devido ao seguinte erro: ", mensagem))
		{

		}
	}
}
