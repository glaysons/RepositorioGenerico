using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public abstract class PersistenciaBase<TObjeto, TLista, TItem> : IPersistencia, IDisposable where TObjeto : IEntidade
	{

		private readonly Dicionario _dicionario;
		private Adapter<TObjeto> _atualizador;

		public abstract TLista Dados { get; }

		internal Adapter<TObjeto> Atualizador
		{
			get { return _atualizador ?? (_atualizador = new Adapter<TObjeto>(_dicionario)); }
		}

		public Dicionario Dicionario { get { return _dicionario; } }

		protected PersistenciaBase(Dicionario dicionario)
		{
			_dicionario = dicionario;
		}

		public abstract int Quantidade { get; }

		public abstract TItem ConsultarUltimoRegistro();

		public abstract void Salvar(IConexao conexao, object registro);

		public virtual void Dispose()
		{
			if (_atualizador != null)
				_atualizador.Dispose();
		}

	}
}
