using System.Data;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos.Tables;

namespace RepositorioGenerico.Fake.Contextos.Tables
{
	public interface IContextoFake : IContexto
	{

		void AdicionarRegistros<TObjeto>(DataTable registros) where TObjeto : IEntidade;

		void AdicionarRegistro<TObjeto>(DataRow registro) where TObjeto : IEntidade;


	}
}
