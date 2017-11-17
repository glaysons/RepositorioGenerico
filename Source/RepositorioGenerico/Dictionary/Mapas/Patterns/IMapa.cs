using System;
using System.Reflection;

namespace RepositorioGenerico.Dictionary.Mapas.Patterns
{
	public interface IMapa
	{

		Type TipoDaTabela { get; }

		string NomeDaTabela { get; }

		PropertyInfo ConsultarPropriedadeDaTabela(PropertyInfo propriedadeDoModel);

	}
}
