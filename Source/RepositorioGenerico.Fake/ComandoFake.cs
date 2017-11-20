using System;
using System.Data;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.Fake
{
	public class ComandoFake : IComando
	{

		private readonly ConexaoFake _conexao;

		public ComandoFake(ConexaoFake conexao)
		{
			_conexao = conexao;
		}

		public IDbCommand CriarComando()
		{
			return _conexao.CriarComando();
		}

		public DataTable ConsultarTabela(IConfiguracao configuracao)
		{
			return null;
		}

		private Configurador ConsultarConfiguradorBusca(Configurador configurador)
		{
			if (configurador == null)
				throw new ArgumentException();
			configurador.Preparar();
			return configurador;
		}

		public IDataReader ConsultarRegistro(IConfiguracao configuracao)
		{
			return ConsultarRegistro(configuracao as Configurador);
		}

		private IDataReader ConsultarRegistro(Configurador configuracao)
		{
			var configurador = ConsultarConfiguradorBusca(configuracao);
			configurador.Comando.Connection = _conexao.CriarConexaoSemTransacao();
			return configurador.Comando.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public IDataReader ConsultarRegistro<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return ConsultarRegistro(configuracao as Configurador);
		}

		public IDataReader ConsultarRegistro(IDbCommand comando)
		{
			comando.Connection = _conexao.CriarConexaoSemTransacao();
			return comando.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public object Scalar(IConfiguracao configuracao)
		{
			return Scalar(configuracao as Configurador);
		}

		private object Scalar(Configurador configuracao)
		{
			var configurador = ConsultarConfiguradorBusca(configuracao);
			using (var conexao = _conexao.CriarConexaoSemTransacao())
			{
				configurador.Comando.Connection = conexao;
				return configurador.Comando.ExecuteScalar();
			}
		}

		public object Scalar<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return Scalar(configuracao as Configurador);
		}

		public int NonQuery(IConfiguracao configuracao)
		{
			return NonQuery(configuracao as Configurador);
		}

		private int NonQuery(Configurador configuracao)
		{
			var configurador = ConsultarConfiguradorBusca(configuracao);
			using (var conexao = _conexao.CriarConexaoSemTransacao())
			{
				configurador.Comando.Connection = conexao;
				return configurador.Comando.ExecuteNonQuery();
			}
		}

		public int NonQuery<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return NonQuery(configuracao as Configurador);
		}

		public bool Existe(IConfiguracao configuracao)
		{
			return Existe(configuracao as Configurador);
		}

		private bool Existe(Configurador configurador)
		{
			if (configurador == null)
				throw new ArgumentException();
			configurador.PrepararExistencia();
			using (var conexao = _conexao.CriarConexaoSemTransacao())
			{
				configurador.Comando.Connection = conexao;
				var existe = configurador.Comando.ExecuteScalar();
				if ((existe == null) || (existe == DBNull.Value))
					return false;
				return (Convert.ToInt32(existe) == 1);
			}
		}

		public bool Existe<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return Existe(configuracao as Configurador);
		}

	}
}
