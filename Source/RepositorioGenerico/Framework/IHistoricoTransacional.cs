using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Framework
{
	public interface IHistoricoTransacional
	{
		int Quantidade { get; }
		ItemHistoricoTransacional this[int indice] { get; }
		void AdicionarTransacao(IPersistencia persistencia, object registro);
		void Salvar();
		void Limpar();
	}
}