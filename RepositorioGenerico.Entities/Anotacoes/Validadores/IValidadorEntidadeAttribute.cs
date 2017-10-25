using System.Runtime.InteropServices;

namespace RepositorioGenerico.Entities.Anotacoes.Validadores
{
	public interface IValidadorEntidadeAttribute : _Attribute
	{

		void Validar(object objeto);

	}
}
