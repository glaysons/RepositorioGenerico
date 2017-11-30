using System.Reflection;
using System.Runtime.InteropServices;

namespace RepositorioGenerico.Entities.Anotacoes.Validadores
{
	public interface IValidadorPropriedadeAttribute : _Attribute
	{

		void Validar(PropertyInfo propriedade, object valorDaPropriedade);

	}
}
