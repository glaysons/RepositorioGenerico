using System;
using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public class Persistencia<TObjeto> : PersistenciaBase<TObjeto, IList<TObjeto>, TObjeto> where TObjeto : IEntidade
	{

		private IList<TObjeto> _dados;

		public override IList<TObjeto> Dados
		{
			get { return _dados ?? (_dados = new List<TObjeto>()); }
		}

		public override int Quantidade
		{
			get { return (_dados == null) ? 0 : _dados.Count; }
		}

		public Persistencia(Dicionario dicionario)
			: base(dicionario)
		{

		}

		public override TObjeto ConsultarUltimoRegistro()
		{
			var ultimo = Quantidade - 1;
			if (ultimo < 0)
				return default(TObjeto);
			return Dados[ultimo];
		}

		public override void Salvar(IConexao conexao, object registro)
		{
			if (registro == null)
				throw new ArgumentNullException();
			var item = registro as IEntidade;
			if (item == null)
				throw new TipoDeObjetoInvalidoException(typeof(TObjeto).Name);
			Atualizador.Salvar(conexao, (TObjeto)item);
			item.EstadoEntidade = EstadosEntidade.NaoModificado;
		}

	}
}
