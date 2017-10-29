using System;
using System.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test
{
	[TestClass]
	public class AdapterUnitTest
	{

		[TestMethod]
		public void SeCadastrarNovoObjetoDeTestesDeveSalvarCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();

			var novoRegistro = new ObjetoDeTestes()
			{
				Nome = "Teste(" + guid.ToString() + ")",
				DataHora = DateTime.Now,
				Decimal = 123.45M,
				Duplo = 234.56
			};

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, novoRegistro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			ValidarExclusaoDoRegistroTemporario(conexao, guid);

			adapter.Dispose();
		}

		private static void ValidarExistenciaRegistroTemporario(Conexao conexao, Guid guid, int deveExistirQuantidade = 1)
		{
			using (var connection = conexao.CriarConexaoSemTransacao())
				using (var command = conexao.CriarComando())
				{
					command.Connection = connection;
					command.CommandText = "select(count(*))from[ObjetoVirtual]where[Nome]like'%(" + guid.ToString() + ")%'and[Decimal]=(123.45)and[Duplo]=(234.56)";
					((int) command.ExecuteScalar())
						.Should()
						.Be(deveExistirQuantidade, "porque so deve existir [" + deveExistirQuantidade.ToString() + "] registro com a Guid [" + guid.ToString() + "]!");
				}
		}

		private static void ValidarExclusaoDoRegistroTemporario(Conexao conexao, Guid guid)
		{
			using (var connection = conexao.CriarConexaoSemTransacao())
				using (var command = conexao.CriarComando())
				{
					command.Connection = connection;
					command.CommandText = "delete[ObjetoVirtual]where[Nome]like'%(" + guid.ToString() + ")%'";
					command.ExecuteNonQuery()
						.Should()
						.Be(1, "porque deve ser excluido um registro com a Guid [" + guid.ToString() + "]!");
				}
		}

		private static Adapter<ObjetoDeTestes> CriarAdapterDoObjetoDeTestes()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));
			return new Adapter<ObjetoDeTestes>(dicionario);
		}

		[TestMethod]
		public void SeCadastrarEDepoisAtualizarObjetoDeTestesDeveSerAtualizadoCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();

			var novoRegistro = new ObjetoDeTestes()
			{
				Nome = "Teste(" + guid.ToString() + ")",
				DataHora = DateTime.Now,
				Decimal = 123.45M,
				Duplo = 234.56
			};

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, novoRegistro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			var registroExistente = (ObjetoDeTestes)novoRegistro.Clone();
			guid = Guid.NewGuid();

			registroExistente.Nome = "Novo(" + guid.ToString() + ")";
			registroExistente.EstadoEntidade = EstadosEntidade.Modificado;

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registroExistente);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			ValidarExclusaoDoRegistroTemporario(conexao, guid);

			adapter.Dispose();
		}

		[TestMethod]
		public void SeCadastrarEDepoisExcluirObjetoDeTestesDeveSerExcluidoCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();

			var registro = new ObjetoDeTestes()
			{
				Nome = "Teste(" + guid.ToString() + ")",
				DataHora = DateTime.Now,
				Decimal = 123.45M,
				Duplo = 234.56
			};

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			registro.EstadoEntidade = EstadosEntidade.Excluido;

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid, deveExistirQuantidade: 0);

			adapter.Dispose();
		}

		[TestMethod]
		public void SeCadastrarNovoRegistroDoObjetoDeTestesDeveSalvarCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();
			var tabela = CriarTabelaDoObjetoDeTestes();

			var novoRegistro = tabela.NewRow();

			novoRegistro["Codigo"] = -1;
			novoRegistro["Nome"] = "Teste(" + guid.ToString() + ")";
			novoRegistro["DataHora"] = DateTime.Now;
			novoRegistro["Decimal"] = 123.45M;
			novoRegistro["Duplo"] = 234.56;
			novoRegistro["Logico"] = true;

			tabela.Rows.Add(novoRegistro);

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, novoRegistro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			ValidarExclusaoDoRegistroTemporario(conexao, guid);

		}

		private DataTable CriarTabelaDoObjetoDeTestes()
		{
			var dicionario = new Dicionario(typeof (ObjetoDeTestes));
			return DataTableBuilder.CriarDataTable(dicionario);
		}

		[TestMethod]
		public void SeCadastrarEDepoisAtualizarRegistroDoObjetoDeTestesDeveSerAtualizadoCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();
			var tabela = CriarTabelaDoObjetoDeTestes();

			var novoRegistro = tabela.NewRow();

			novoRegistro["Codigo"] = -1;
			novoRegistro["Nome"] = "Teste(" + guid.ToString() + ")";
			novoRegistro["DataHora"] = DateTime.Now;
			novoRegistro["Decimal"] = 123.45M;
			novoRegistro["Duplo"] = 234.56;
			novoRegistro["Logico"] = true;

			tabela.Rows.Add(novoRegistro);

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, novoRegistro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			tabela.AcceptChanges();

			var registroExistente = tabela.Rows[0];
			guid = Guid.NewGuid();

			registroExistente["Nome"] = "Novo(" + guid.ToString() + ")";

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registroExistente);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid);

			ValidarExclusaoDoRegistroTemporario(conexao, guid);

			adapter.Dispose();
		}

		[TestMethod]
		public void SeCadastrarEDepoisExcluirRegsitroDoObjetoDeTestesDeveSerExcluidoCorretamente()
		{
			var adapter = CriarAdapterDoObjetoDeTestes();
			var conexao = new Conexao(ConnectionStringHelper.Consultar());
			var guid = Guid.NewGuid();
			var tabela = CriarTabelaDoObjetoDeTestes();

			var registro = tabela.NewRow();

			registro["Codigo"] = -1;
			registro["Nome"] = "Teste(" + guid.ToString() + ")";
			registro["DataHora"] = DateTime.Now;
			registro["Decimal"] = 123.45M;
			registro["Duplo"] = 234.56;
			registro["Logico"] = true;

			tabela.Rows.Add(registro);

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			tabela.AcceptChanges();

			ValidarExistenciaRegistroTemporario(conexao, guid);

			registro.Delete();

			conexao.IniciarTransacao();
			try
			{
				adapter.Salvar(conexao, registro);
				conexao.ConfirmarTransacao();
			}
			catch
			{
				conexao.CancelarTransacao();
				throw;
			}

			ValidarExistenciaRegistroTemporario(conexao, guid, deveExistirQuantidade: 0);

			adapter.Dispose();
		}

	}
}
