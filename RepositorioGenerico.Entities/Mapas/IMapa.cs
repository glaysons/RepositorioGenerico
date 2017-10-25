namespace RepositorioGenerico.Entities.Mapas
{
	public interface IMapa<TModel> where TModel : class, IEntidade
	{

		ITabelaMapaDicionario<TModel, TTabela> Tabela<TTabela>()
			where TTabela : class, IEntidade;

	}
}
