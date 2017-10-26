using System;
using System.Data;
using System.Linq.Expressions;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Framework.Helpers;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class Configurador<TObjeto> : Configurador, IConfiguracao<TObjeto>
	{

		private readonly IDicionario _dicionario;

		public Configurador(IDbCommand comando, IDicionario dicionario)
			: base(comando)
		{
			_dicionario = dicionario;
		}

		public IConfiguracaoParametro<TObjeto> DefinirParametro(Expression<Func<TObjeto, object>> nome)
		{
			return new ConfiguradorParametro<TObjeto>(this, ConsultarNomeDaExpressao(nome));
		}

		protected string ConsultarNomeDaExpressao(Expression<Func<TObjeto, object>> campo)
		{
			if (campo == null)
				return null;
			var propriedade = ExpressionHelper.PropriedadeDaExpressao(campo);
			var info = _dicionario.ConsultarPorPropriedade(propriedade.Name);
			return (info == null)
				? propriedade.Name
				: info.Nome;
		}

	}
}
