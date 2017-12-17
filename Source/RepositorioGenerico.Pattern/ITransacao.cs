using System;

namespace RepositorioGenerico.Pattern
{

	public interface ITransacao : IDisposable
	{

		EventoDelegate AntesConfirmarTransacao { get; set; }

		EventoDelegate DepoisConfirmarTransacao { get; set; }

		EventoDelegate AntesCancelarTransacao { get; set; }

		EventoDelegate DepoisCancelarTransacao { get; set; }

		void ConfirmarTransacao();

		void CancelarTransacao();

	}

}
