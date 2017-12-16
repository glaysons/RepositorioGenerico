using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.SqlClient.Builders;
using RepositorioGenerico.SqlClient.Scripts;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.SqlClient
{
	internal class Adapter<TObjeto> : IDisposable where TObjeto : IEntidade
	{

		private readonly Dicionario _dicionario;

		private Script _scripts;
		private SqlCommand _insert;
		private SqlCommand _update;
		private SqlCommand _delete;
		private SqlCommand _autoInc;

		private int? _ultimoIncremento;

		private Script Scripts
		{
			get { return _scripts ?? (_scripts = Cache.Consultar(_dicionario)); }
		}

		private SqlCommand Insert
		{
			get { return _insert ?? (_insert = CriarComandoInsert()); }
		}

		private SqlCommand Update
		{
			get { return _update ?? (_update = CriarComandoUpdate()); }
		}

		private SqlCommand Delete
		{
			get { return _delete ?? (_delete = CriarComandoDelete()); }
		}

		private SqlCommand AutoIncremento
		{
			get { return _autoInc ?? (_autoInc = CriarComandoAutoIncremento()); }
		}

		public Adapter(Dicionario dicionario)
		{
			_dicionario = dicionario;
		}

		private SqlCommand CriarComandoInsert()
		{
			var comando = new SqlCommand { CommandText = Scripts.Insert };
			CommandBuilder.DefinirParametrosParaTodosOsCampos(_dicionario, comando);
			return comando;
		}

		private SqlCommand CriarComandoUpdate()
		{
			var comando = new SqlCommand { CommandText = Scripts.Update };
			CommandBuilder.DefinirParametrosParaTodosOsCampos(_dicionario, comando);
			return comando;
		}

		private SqlCommand CriarComandoDelete()
		{
			var comando = new SqlCommand { CommandText = Scripts.Delete };
			CommandBuilder.DefinirParametrosParaTodosOsCamposDaChave(_dicionario, comando);
			return comando;
		}

		private SqlCommand CriarComandoAutoIncremento()
		{
			var comando = new SqlCommand { CommandText = Scripts.AutoIncremento };
			CommandBuilder.DefinirParametrosParaCamposDaChaveQueNaoSaoAutoIncremento(_dicionario, comando);
			return comando;
		}

		public void Salvar(IConexao conexao, TObjeto registro)
		{
			SqlCommand comando = null;

			var novo = (registro.EstadoEntidade == EstadosEntidade.Novo);
			if ((novo) || (registro.EstadoEntidade == EstadosEntidade.Modificado))
			{
				comando = (novo)
					? Insert
					: Update;
				if ((novo) && (_dicionario.AutoIncremento == OpcoesAutoIncremento.Calculado))
					CalcularAutoIncremento(conexao, registro);
				CommandBuilder.SincronizarParametrosDeTodosOsCampos(_dicionario, comando, registro);
			}
			else if (registro.EstadoEntidade == EstadosEntidade.Excluido)
			{
				comando = Delete;
				CommandBuilder.SincronizarParametrosDosCamposChave(_dicionario, comando, registro);
			}

			if ((novo) && (_dicionario.AutoIncremento == OpcoesAutoIncremento.Identity))
			{
				var autoIncremento = ExecutarComandoInsertComIdentity(conexao, comando);
				AtribuirValorAutoIncremento(Incremento.Identity, item => item.Propriedade.SetValue(registro, autoIncremento, null));
			}
			else
				ExecutarComando(conexao, comando);

			SincronizarChaveDoPaiComOsFilhos(registro);
		}

		private void CalcularAutoIncremento(IConexao conexao, TObjeto registro)
		{
			var valor = _ultimoIncremento;

			if (!valor.HasValue)
			{
				var comando = AutoIncremento;
				if (string.IsNullOrEmpty(comando.CommandText))
					throw new NaoFoiPossivelLocalizarScriptParaCalculoDoAutoIncrementoException(_dicionario.Nome);
				CommandBuilder.SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento(_dicionario, comando, registro);
				conexao.DefinirConexaoTransacionada(comando);
				valor = Convert.ToInt32(comando.ExecuteScalar());
			}

			AtribuirValorAutoIncremento(Incremento.Calculado, item => item.Propriedade.SetValue(registro, valor, null));

			if (_dicionario.QuantidadeCamposNaChave == 1)
				_ultimoIncremento = valor + 1;
		}

		private int ExecutarComandoInsertComIdentity(IConexao conexao, IDbCommand comando)
		{
			conexao.DefinirConexaoTransacionada(comando);
			return Convert.ToInt32(comando.ExecuteScalar());
		}

		private void AtribuirValorAutoIncremento(Incremento opcao, Action<ItemDicionario> atribuir)
		{
			foreach (var item in _dicionario.Itens)
				if (item.OpcaoGeracao == opcao)
				{
					atribuir(item);
					return;
				}
		}

		private void ExecutarComando(IConexao conexao, IDbCommand comando)
		{
			if (comando == null)
				return;
			conexao.DefinirConexaoTransacionada(comando);
			var afetados = comando.ExecuteNonQuery();
			if (afetados == 0)
				throw new DBConcurrencyException();
		}

		private void SincronizarChaveDoPaiComOsFilhos(TObjeto registro)
		{
			if (!((registro.EstadoEntidade == EstadosEntidade.Novo) || (_dicionario.AutoIncremento != OpcoesAutoIncremento.Nenhum) || (_dicionario.PossuiCamposFilho)))
				return;

			var chave = _dicionario.ConsultarValoresDaChave(registro);
			foreach (var filho in _dicionario.ConsultarCamposFilho())
			{
				var itens = (ICollection)filho.Propriedade.GetValue(registro, null);
				if (itens == null)
					continue;
				foreach (var item in itens)
					filho.Ligacao.AplicarChaveAscendente(chave, item);
			}
		}

		public void Salvar(IConexao conexao, DataRow registro)
		{
			SqlCommand comando = null;

			var novo = (registro.RowState == DataRowState.Added);
			if ((novo) || (registro.RowState == DataRowState.Modified))
			{
				comando = (novo)
					? Insert
					: Update;
				if ((novo) && (_dicionario.AutoIncremento == OpcoesAutoIncremento.Calculado))
					CalcularAutoIncremento(conexao, registro);
				CommandBuilder.SincronizarParametrosDeTodosOsCampos(_dicionario, comando, registro);
			}
			else if (registro.RowState == DataRowState.Deleted)
			{
				comando = Delete;
				registro.RejectChanges();
				CommandBuilder.SincronizarParametrosDosCamposChave(_dicionario, comando, registro);
			}

			if ((novo) && (_dicionario.AutoIncremento == OpcoesAutoIncremento.Identity))
			{
				var autoIncremento = ExecutarComandoInsertComIdentity(conexao, comando);
				AtribuirValorAutoIncremento(Incremento.Identity, item => registro[item.Nome] = autoIncremento);
			}
			else
				ExecutarComando(conexao, comando);
		}

		private void CalcularAutoIncremento(IConexao conexao, DataRow registro)
		{
			var valor = _ultimoIncremento;

			if (!valor.HasValue)
			{
				CommandBuilder.SincronizarParametrosDosCamposChaveQueNaoSaoAutoIncremento(_dicionario, AutoIncremento, registro);
				conexao.DefinirConexaoTransacionada(AutoIncremento);
				valor = Convert.ToInt32(AutoIncremento.ExecuteScalar());
			}

			AtribuirValorAutoIncremento(Incremento.Calculado, item => registro[item.Nome] = valor);

			if (_dicionario.QuantidadeCamposNaChave == 1)
				_ultimoIncremento = valor + 1;
		}

		public void Dispose()
		{
			Dispose(_autoInc);
			Dispose(_insert);
			Dispose(_update);
			Dispose(_delete);
		}

		private void Dispose(SqlCommand comando)
		{
			if (comando != null)
				comando.Dispose();
		}
	}
}
