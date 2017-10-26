using System;
using System.Collections.Generic;
using System.Data;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern.Contextos.Tables;

namespace RepositorioGenerico.SqlClient.Contextos.Tables
{

	public class Repositorio<TObjeto> : IRepositorio where TObjeto : class, IEntidade
    {

		private readonly ContextoBase _contexto;
		private readonly Persistencia<TObjeto> _persistencia;
		private bool _validarAoSalvar;

		public int Quantidade
		{
			get { return _persistencia.Quantidade; }
		}

		public Repositorio(ContextoBase contexto, Persistencia<TObjeto> persistencia)
		{
			_contexto = contexto;
			_persistencia = persistencia;
			_validarAoSalvar = true;
		}

		public void AtivarValidacoes()
		{
			_validarAoSalvar = true;
		}

		public void DesativarValidacoes()
		{
			_validarAoSalvar = false;
		}

		public IEnumerable<DataRow> Itens()
		{
			for (var registro = 0; registro < _persistencia.Dados.Rows.Count; registro++)
				yield return _persistencia.Dados.Rows[registro];
		}

		public DataRow Consultar(params object[] chave)
		{
			if (chave.Length == 0)
				throw new ValoresChavePreenchimentoObrigatorioException();
			if (chave.Length != _persistencia.Dicionario.QuantidadeCamposNaChave)
				throw new ValoresChavePreenchimentoObrigatorioException(_persistencia.Dicionario.QuantidadeCamposNaChave);
			var consulta = _contexto.Buscar().CriarQuery(_persistencia.Dicionario.Nome);
			var indice = 0;
			foreach (var campo in _persistencia.Dicionario.ConsultarCamposChave())
			{
				consulta.AdicionarCondicaoPersonalizada(campo.Nome + "=@p" + indice.ToString());
				consulta.DefinirParametro("p" + indice.ToString()).Tipo(campo.TipoBanco, chave[indice]);
				indice++;
			}
			return _contexto.Buscar().Um(consulta);
		}

		public DataRow Criar()
		{
			return _persistencia.Criar();
		}

		public void Validar(DataRow registro)
		{
			_persistencia.Dicionario.Validador.Validar(registro);
		}

		public IEnumerable<string> Valido(DataRow registro)
		{
			return _persistencia.Dicionario.Validador.Valido(registro);
		}

		public void Inserir(DataRow registro)
		{
			if (_validarAoSalvar)
				Validar(registro);
			_persistencia.Dados.Rows.Add(registro);
			_contexto.Transacoes.AdicionarTransacao(_persistencia, registro);
		}

		public void Inserir(DataTable registros)
		{
			foreach (DataRow registro in registros.Rows)
				Inserir(registro);
		}

		public void Atualizar(DataRow registro)
		{
			registro = AnexarRegistroNaMemoria(registro);
			if (_validarAoSalvar)
				Validar(registro);
			if (registro.RowState != DataRowState.Modified)
				registro.SetModified();
			_contexto.Transacoes.AdicionarTransacao(_persistencia, registro);
		}

		private DataRow AnexarRegistroNaMemoria(DataRow registro)
		{
			if (_persistencia.Dados.Equals(registro.Table))
				return registro;
			_persistencia.Dados.ImportRow(registro);
			var quantidade = _persistencia.Dados.Rows.Count;
			return _persistencia.Dados.Rows[quantidade - 1];
		}

		public void Atualizar(DataTable registros)
		{
			foreach (DataRow registro in registros.Rows)
				Atualizar(registro);
		}

		public void Excluir(DataRow registro)
		{
			registro = AnexarRegistroNaMemoria(registro);
			registro.Delete();
			_contexto.Transacoes.AdicionarTransacao(_persistencia, registro);
		}

		public void Importar(DataRow registro)
		{
			if (registro == null)
				throw new ArgumentException();
			switch (registro.RowState)
			{
				case DataRowState.Modified:
					Atualizar(registro);
					break;

				case DataRowState.Deleted:
					Excluir(registro);
					break;

				default:
					Inserir(registro);
					break;
			}
		}

	}

}
