using System;
using System.Data;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracaoParametro
	{
		IConfiguracao Valor(string valor, int tamanhoMaximo = 0);
		IConfiguracao Valor(int valor);
		IConfiguracao Valor(int? valor);
		IConfiguracao Valor(double valor);
		IConfiguracao Valor(double? valor);
		IConfiguracao Valor(decimal valor);
		IConfiguracao Valor(decimal? valor);
		IConfiguracao Valor(bool valor);
		IConfiguracao Valor(DateTime valor);
		IConfiguracao Valor(DateTime? valor);
		IConfiguracao Nulo();
		IConfiguracao Tipo(DbType tipo, object valor);
	}
}
