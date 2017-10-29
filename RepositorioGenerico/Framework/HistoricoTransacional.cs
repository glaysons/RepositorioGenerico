using System;
using System.Collections.Generic;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Framework
{
	public class HistoricoTransacional : IHistoricoTransacional
	{

		private readonly IConexao _conexao;
		private IList<ItemHistoricoTransacional> _log;

		private IList<ItemHistoricoTransacional> Log
		{
			get { return _log ?? (_log = new List<ItemHistoricoTransacional>()); }
		}

		public int Quantidade { get { return Log.Count; } }

		public ItemHistoricoTransacional this[int indice]
		{
			get
			{
				if (!(indice < 0 || indice >= Log.Count))
					return Log[indice];
				throw new IndexOutOfRangeException();
			}
		}

		public HistoricoTransacional(IConexao conexao)
		{
			_conexao = conexao;
		}

		public void AdicionarTransacao(IPersistencia persistencia, object registro)
		{
			Log.Add(new ItemHistoricoTransacional(persistencia, registro));
		}

		public void Salvar()
		{
			foreach (var item in Log)
				item.Persistencia.Salvar(_conexao, item.Registro);
			Log.Clear();
		}

	}
}
