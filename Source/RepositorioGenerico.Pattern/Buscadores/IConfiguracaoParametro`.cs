using System;
using System.Data;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoParametro<TObjeto>
	{
		IConfiguracao<TObjeto> Valor(string valor, int tamanhoMaximo = 0);
		IConfiguracao<TObjeto> Valor(int valor);
		IConfiguracao<TObjeto> Valor(int? valor);
		IConfiguracao<TObjeto> Valor(double valor);
		IConfiguracao<TObjeto> Valor(double? valor);
		IConfiguracao<TObjeto> Valor(decimal valor);
		IConfiguracao<TObjeto> Valor(decimal? valor);
		IConfiguracao<TObjeto> Valor(bool valor);
		IConfiguracao<TObjeto> Valor(DateTime valor);
		IConfiguracao<TObjeto> Valor(DateTime? valor);
		IConfiguracao<TObjeto> Nulo();
		IConfiguracao<TObjeto> Tipo(DbType tipo, object valor);
	}
}
