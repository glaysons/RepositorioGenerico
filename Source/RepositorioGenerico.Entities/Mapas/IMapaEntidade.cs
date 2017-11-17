namespace RepositorioGenerico.Entities.Mapas
{
	public interface IMapaEntidade<TTabela>
		where TTabela : class, IEntidade
	{

		void Referenciar(IMapa<TTabela> builder);

	}
}
