using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search.Extensoes
{
	public static class ConfiguradorExtension
	{

		public static void PersonalizarScript(this IConfiguracao sender, string script)
		{
			if (sender == null)
				return;
			if (sender is Configurador)
				((Configurador)sender).PersonalizarScript(script);
			else
				throw new EsteTipoDeConfiguracaoNaoPermitePersonalizacaoDeScript(sender.GetType().Name);
		}

	}
}
