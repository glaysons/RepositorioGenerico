using System.Reflection;

namespace RepositorioGenerico.Dictionary.Mapas.Patterns
{
	internal interface IConsultadorCamposDaTabela
	{
		PropertyInfo ConsultarPropriedadeDaTabelaRelacionadaComModel(PropertyInfo propriedadeDoModel);
	}
}
