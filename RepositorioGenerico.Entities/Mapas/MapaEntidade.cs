namespace RepositorioGenerico.Entities.Mapas
{
	public abstract class MapaEntidade<TObjeto> : IMapaEntidade<TObjeto> where TObjeto : class, IEntidade
	{

		public abstract void Referenciar(IMapa<TObjeto> builder);

	}
}
