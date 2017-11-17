using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Search.Conversores
{
	internal class Conversor<TObjeto> : IEnumerable<TObjeto>
	{

		private readonly IDataReader _reader;

		private readonly Func<DbDataReader, TObjeto> _binder;

		public Conversor(IDataReader reader, Delegate binder)
		{
			_reader = reader;
			_binder = (Func<DbDataReader, TObjeto>)binder;
		}

		public IEnumerator<TObjeto> GetEnumerator()
		{
			while (_reader.Read())
			{
				var registro = _binder(_reader as DbDataReader);
				AtualizarEstadoDoObjeto(registro as IEntidade);
				yield return registro;
			}
		}

		private static void AtualizarEstadoDoObjeto(IEntidade estado)
		{
			if (estado != null)
				estado.EstadoEntidade = EstadosEntidade.NaoModificado;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}