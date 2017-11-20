using System;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Fake.Contextos
{
	public abstract class PersistenciaFakeBase<TObjeto, TLista, TItem> : IPersistencia where TObjeto : IEntidade
	{

		private readonly Dicionario _dicionario;
		private AdapterFake<TObjeto> _atualizador;

		public abstract TLista Dados { get; }

		internal AdapterFake<TObjeto> Atualizador
		{
			get { return _atualizador ?? (_atualizador = new AdapterFake<TObjeto>(_dicionario)); }
		}

		public Dicionario Dicionario { get { return _dicionario; } }

		protected PersistenciaFakeBase(Dicionario dicionario)
		{
			_dicionario = dicionario;
		}

		public abstract int Quantidade { get; }

		public abstract TItem ConsultarUltimoRegistro();

		public abstract TItem Criar();

		public abstract void Salvar(IConexao conexao, object registro);

	}
}
