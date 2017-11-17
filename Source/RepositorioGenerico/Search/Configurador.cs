using System;
using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class Configurador : IConfiguracao
	{

		public IDbCommand Comando { get; private set; }

		protected string ScriptPersonalizado { get; set; }

		public Configurador(IDbCommand comando)
		{
			Comando = comando;
		}

		public IConfiguracaoParametro DefinirParametro(string nome)
		{
			return new ConfiguradorParametro(this, nome);
		}

		public virtual void Preparar()
		{
			throw new NotImplementedException();
		}

		public virtual void PrepararExistencia()
		{
			throw new NotImplementedException();
		}

		public string ConsultarScript()
		{
			return Comando.CommandText;
		}

		public void PersonalizarScript(string script)
		{
			ScriptPersonalizado = script;
		}

	}
}
