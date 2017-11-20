using System.Data;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Builders;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;
using System;

namespace RepositorioGenerico.Fake.Contextos.Tables
{
	public class PersistenciaFake<TObjeto> : PersistenciaFakeBase<TObjeto, DataTable, DataRow>, IDisposable where TObjeto : IEntidade
	{
		private DataTable _dados;

		public override DataTable Dados
		{
			get { return _dados ?? (_dados = DataTableBuilder.CriarDataTable(Dicionario)); }
		}

		public override int Quantidade
		{
			get { return (_dados == null) ? 0 : _dados.Rows.Count; }
		}

		public PersistenciaFake(Dicionario dicionario)
			: base(dicionario)
		{

		}

		public override DataRow ConsultarUltimoRegistro()
		{
			var ultimo = Quantidade - 1;
			if (ultimo < 0)
				return null;
			return Dados.Rows[ultimo];
		}

		public override DataRow Criar()
		{
			var registro = Dados.NewRow();
			foreach (var item in Dicionario.Itens)
				if (item.ValorPadrao != null)
					registro[item.Nome] = item.ValorPadrao;
			return registro;
		}

		public override void Salvar(IConexao conexao, object registro)
		{
			var item = registro as DataRow;
			if (item == null)
				throw new TipoDeObjetoInvalidoException("DataRow");
			Atualizador.Salvar(conexao, item);
			item.AcceptChanges();
		}

		public void Dispose()
		{
			if (_dados != null)
				_dados.Dispose();
		}


	}
}
