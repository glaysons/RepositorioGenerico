using System;
using System.Data;
using Moq;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Entities.Anotacoes;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.Fake
{
	internal class AdapterFake<TObjeto> : IDisposable where TObjeto : IEntidade
	{
		private readonly Dicionario _dicionario;

		private IDbCommand _insert;
		private IDbCommand _update;
		private IDbCommand _delete;
		private IDbCommand _autoInc;

		private int? _ultimoIncremento;

		private IDbCommand Insert
		{
			get { return _insert ?? (_insert = CriarComandoInsert()); }
		}

		private IDbCommand Update
		{
			get { return _update ?? (_update = CriarComandoUpdate()); }
		}

		private IDbCommand Delete
		{
			get { return _delete ?? (_delete = CriarComandoDelete()); }
		}

		private IDbCommand AutoIncremento
		{
			get { return _autoInc ?? (_autoInc = CriarComandoAutoIncremento()); }
		}

		public AdapterFake(Dicionario dicionario)
		{
			_dicionario = dicionario;
		}

		private IDbCommand CriarComandoInsert()
		{
			return new Mock<IDbCommand>().Object;
		}

		private IDbCommand CriarComandoUpdate()
		{
			return new Mock<IDbCommand>().Object;
		}

		private IDbCommand CriarComandoDelete()
		{
			return new Mock<IDbCommand>().Object;
		}

		private IDbCommand CriarComandoAutoIncremento()
		{
			return new Mock<IDbCommand>().Object;
		}

		public void Salvar(IConexao conexao, TObjeto registro)
		{
			IDbCommand comando = null;

			if ((registro.EstadoEntidade == EstadosEntidade.Novo) || (registro.EstadoEntidade == EstadosEntidade.Modificado))
			{
				comando = (registro.EstadoEntidade == EstadosEntidade.Novo)
					? Insert
					: Update;
				if (_dicionario.AutoIncremento == OpcoesAutoIncremento.Calculado)
					CalcularAutoIncremento(conexao, registro);
			}
			else if (registro.EstadoEntidade == EstadosEntidade.Excluido)
				comando = Delete;

			if ((registro.EstadoEntidade == EstadosEntidade.Novo) && (_dicionario.AutoIncremento == OpcoesAutoIncremento.Identity))
			{
				var autoIncremento = ExecutarComandoInsertComIdentity(conexao, comando);
				AtribuirValorAutoIncremento(Incremento.Identity, item => item.Propriedade.SetValue(registro, autoIncremento, null));
			}
			else
				ExecutarComando(conexao, comando);
		}

		private void CalcularAutoIncremento(IConexao conexao, TObjeto registro)
		{
			var valor = _ultimoIncremento;

			if (!valor.HasValue)
			{
				var comando = AutoIncremento;
				conexao.DefinirConexao(comando);
				valor = Convert.ToInt32(comando.ExecuteScalar());
			}

			AtribuirValorAutoIncremento(Incremento.Calculado, item => item.Propriedade.SetValue(registro, valor, null));

			if (_dicionario.QuantidadeCamposNaChave == 1)
				_ultimoIncremento = valor + 1;
		}

		private int ExecutarComandoInsertComIdentity(IConexao conexao, IDbCommand comando)
		{
			conexao.DefinirConexao(comando);
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
			conexao.DefinirConexao(comando);
			var afetados = comando.ExecuteNonQuery();
			if (afetados == 0)
				throw new DBConcurrencyException();
		}

		public void Salvar(IConexao conexao, DataRow registro)
		{
			IDbCommand comando = null;

			if ((registro.RowState == DataRowState.Added) || (registro.RowState == DataRowState.Modified))
			{
				comando = (registro.RowState == DataRowState.Added)
					? Insert
					: Update;
				if (_dicionario.AutoIncremento == OpcoesAutoIncremento.Calculado)
					CalcularAutoIncremento(conexao, registro);
			}
			else if (registro.RowState == DataRowState.Deleted)
				comando = Delete;

			if (_dicionario.AutoIncremento == OpcoesAutoIncremento.Identity)
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
				conexao.DefinirConexao(AutoIncremento);
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

		public void Dispose(IDbCommand comando)
		{
			if (comando != null)
				comando.Dispose();
		}

	}
}
