namespace RepositorioGenerico.Pattern
{

	public interface ITransacao
	{

		bool EmTransacao { get; }

		void IniciarTransacao();

		void ConfirmarTransacao();

		void CancelarTransacao();

	}

}
