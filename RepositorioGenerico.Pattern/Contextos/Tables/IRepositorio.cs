using System.Data;

namespace RepositorioGenerico.Pattern.Contextos.Tables
{
	public interface IRepositorio : IRepositorioBase<DataTable, DataRow>
	{

		void Importar(DataRow registro);

	}
}
