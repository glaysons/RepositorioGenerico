using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Framework
{
	public class ItemHistoricoTransacional
	{

		public IPersistencia Persistencia { get; private set; }

		public object Registro { get; private set; }

		public ItemHistoricoTransacional(IPersistencia persistencia, object registro)
		{
			Persistencia = persistencia;
			Registro = registro;
		}
	}
}
