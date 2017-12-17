namespace RepositorioGenerico.Pattern
{
	public interface ITransacional
	{
		bool EmTransacao { get; }

		EventoDelegate AntesIniciarTransacao { get; set; }

		EventoDelegate DepoisIniciarTransacao { get; set; }

		ITransacao IniciarTransacao();
	}
}
