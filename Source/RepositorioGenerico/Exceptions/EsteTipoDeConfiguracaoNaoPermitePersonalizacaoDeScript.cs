using System;

namespace RepositorioGenerico.Exceptions
{
	public class EsteTipoDeConfiguracaoNaoPermitePersonalizacaoDeScript : Exception
	{
		public EsteTipoDeConfiguracaoNaoPermitePersonalizacaoDeScript(string tipo)
			: base(string.Concat("Este tipo de configuração [", tipo, "] não permite personalização de script!"))
		{

		}
	}
}
