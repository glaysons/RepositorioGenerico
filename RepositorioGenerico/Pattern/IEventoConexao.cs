namespace RepositorioGenerico.Pattern
{
	public interface IEventoConexao
	{

		EventoConexaoDelegate AntesIniciarTransacao { get; set; }

		EventoConexaoDelegate DepoisIniciarTransacao { get; set; }

		EventoConexaoDelegate AntesConfirmarTransacao { get; set; }

		EventoConexaoDelegate DepoisConfirmarTransacao { get; set; }

		EventoConexaoDelegate AntesCancelarTransacao { get; set; }

		EventoConexaoDelegate DepoisCancelarTransacao { get; set; }

	}
}
