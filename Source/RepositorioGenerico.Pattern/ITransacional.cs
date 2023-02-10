using System.Data;

namespace RepositorioGenerico.Pattern
{
	public interface ITransacional
	{

		void DefinirUmaTransacaoEspecifica(IDbTransaction transacao);

		bool EmTransacao { get; }

		EventoDelegate AntesIniciarTransacao { get; set; }

		EventoDelegate DepoisIniciarTransacao { get; set; }

		ITransacao IniciarTransacao();
	}
}
