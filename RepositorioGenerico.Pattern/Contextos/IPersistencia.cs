namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IPersistencia
	{

		void Salvar(IConexao conexao, object registro);

	}
}
