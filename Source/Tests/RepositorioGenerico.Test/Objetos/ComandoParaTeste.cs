using RepositorioGenerico.Pattern;
using System;
using RepositorioGenerico.Pattern.Buscadores;
using System.Data;
using System.Data.SqlClient;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.Test.Objetos
{
	public class ComandoParaTeste : IComando
	{

		private SqlConnection _conexao;

		public ComandoParaTeste()
		{
			_conexao = new SqlConnection(ConnectionStringHelper.Consultar());
		}

		public IDataReader ConsultarRegistro(IConfiguracao configuracao)
		{
			var config = configuracao as Configurador;
			config.Preparar();
			_conexao.Open();
			return config.Comando.ExecuteReader();
		}

		public IDataReader ConsultarRegistro<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			throw new NotImplementedException();
		}

		public DataTable ConsultarTabela(IConfiguracao configuracao)
		{
			var config = configuracao as Configurador;
			config.Preparar();
			var adapter = new SqlDataAdapter(config.Comando as SqlCommand);
			var tabela = new DataTable();
			adapter.Fill(tabela);
			return tabela;
		}

		public IDbCommand CriarComando()
		{
			return _conexao.CreateCommand();
		}

		public bool Existe(IConfiguracao configuracao)
		{
			var config = configuracao as Configurador;
			config.PrepararExistencia();
			_conexao.Open();
			return config.Comando.ExecuteReader(CommandBehavior.CloseConnection).Read();
		}

		public bool Existe<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			throw new NotImplementedException();
		}

		public int NonQuery(IConfiguracao configuracao)
		{
			var config = configuracao as Configurador;
			config.Preparar();
			_conexao.Open();
			try
			{
				return config.Comando.ExecuteNonQuery();
			}
			finally
			{
				_conexao.Close();
			}
		}

		public int NonQuery<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			throw new NotImplementedException();
		}

		public object Scalar(IConfiguracao configuracao)
		{
			var config = configuracao as Configurador;
			config.Preparar();
			_conexao.Open();
			try
			{
				return config.Comando.ExecuteScalar();
			}
			finally
			{
				_conexao.Close();
			}
		}

		public object Scalar<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			throw new NotImplementedException();
		}
	}
}
