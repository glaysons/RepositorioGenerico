using System;
using System.Data;
using System.Data.SqlClient;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.SqlClient
{
	public class Comando : IComando
	{

		private readonly IConexao _conexao;

		public Comando(IConexao conexao)
		{
			_conexao = conexao;
		}

		public IDbCommand CriarComando()
		{
			return _conexao.CriarComando();
		}

		public DataTable ConsultarTabela(IConfiguracao configuracao)
		{
			var configurador = ConsultarConfiguradorBusca(configuracao as Configurador);

			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return ConsultarTabelaComTransacaoExistente(configurador);

			return ConsultarTabelaComNovaTransacao(configurador);
		}

		private DataTable ConsultarTabelaComTransacaoExistente(Configurador configurador)
		{
			var tabela = new DataTable();
			using (var adapter = new SqlDataAdapter(configurador.Comando as SqlCommand))
			{
				_conexao.DefinirConexaoTransacionada(configurador.Comando);
				adapter.Fill(tabela);
			}
			return tabela;
		}

		private DataTable ConsultarTabelaComNovaTransacao(Configurador configurador)
		{
			var tabela = new DataTable();
			using (var conexao = _conexao.CriarConexaoSemTransacao())
			using (var adapter = new SqlDataAdapter(configurador.Comando as SqlCommand))
			{
				configurador.Comando.Connection = conexao;
				adapter.Fill(tabela);
			}
			return tabela;
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

			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return ConsultarRegistroComTransacaoExistente(configurador);

			return ConsultarRegistroComNovaTransacao(configurador);
		}

		private IDataReader ConsultarRegistroComTransacaoExistente(Configurador configurador)
		{
			_conexao.DefinirConexaoTransacionada(configurador.Comando);
			return configurador.Comando.ExecuteReader();
		}

		private IDataReader ConsultarRegistroComNovaTransacao(Configurador configurador)
		{
			configurador.Comando.Connection = _conexao.CriarConexaoSemTransacao();
			return configurador.Comando.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public IDataReader ConsultarRegistro<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return ConsultarRegistro(configuracao as Configurador);
		}

		public IDataReader ConsultarRegistro(IDbCommand comando)
		{
			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return ConsultarRegistroComTransacaoExistente(comando);

			return ConsultarRegistroComNovaTransacao(comando);
		}

		private IDataReader ConsultarRegistroComTransacaoExistente(IDbCommand comando)
		{
			_conexao.DefinirConexaoTransacionada(comando);
			return comando.ExecuteReader();
		}

		private IDataReader ConsultarRegistroComNovaTransacao(IDbCommand comando)
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

			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return ScalarComTransacaoExistente(configuracao, configurador);

			return ScalarComNovaTransacao(configurador);
		}

		private object ScalarComTransacaoExistente(Configurador configuracao, Configurador configurador)
		{
			_conexao.DefinirConexaoTransacionada(configuracao.Comando);
			return configurador.Comando.ExecuteScalar();
		}

		private object ScalarComNovaTransacao(Configurador configurador)
		{
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

			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return NonQueryComTransacaoExistente(configurador);

			return NonQueryComNovaTransacao(configurador);
		}

		private int NonQueryComTransacaoExistente(Configurador configurador)
		{
			_conexao.DefinirConexaoTransacionada(configurador.Comando);
			return configurador.Comando.ExecuteNonQuery();
		}

		private int NonQueryComNovaTransacao(Configurador configurador)
		{
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

			if (_conexao.EmTransacao && _conexao.RealizaConsultasUtilizandoConexaoTransacionada)
				return ExisteComTransacaoExistente(configurador);

			return ExisteComNovaTransacao(configurador);
		}

		private bool ExisteComTransacaoExistente(Configurador configurador)
		{
			_conexao.DefinirConexaoTransacionada(configurador.Comando);
			return ExisteInformacao(configurador);
		}

		private bool ExisteComNovaTransacao(Configurador configurador)
		{
			using (var conexao = _conexao.CriarConexaoSemTransacao())
			{
				configurador.Comando.Connection = conexao;
				return ExisteInformacao(configurador);
			}
		}

		private static bool ExisteInformacao(Configurador configurador)
		{
			var existe = configurador.Comando.ExecuteScalar();
			if ((existe == null) || (existe == DBNull.Value))
				return false;
			return (Convert.ToInt32(existe) == 1);
		}

		public bool Existe<TObjeto>(IConfiguracao<TObjeto> configuracao)
		{
			return Existe(configuracao as Configurador);
		}
	}
}
