using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RepositorioGenerico.Search.Conversores
{
	public class Conversor
	{

		private static readonly ConcurrentDictionary<string, Conversor> _conversores = new ConcurrentDictionary<string, Conversor>();

		public static IEnumerable<TObjeto> ConverterDataReaderParaObjeto<TObjeto>(IDataReader reader)
		{
			var tipo = typeof (TObjeto);
			var conversor = ConsultarConversorAtual(tipo);
			return 
				from object registro in conversor.ConverterDataReader(tipo, reader) 
				select (TObjeto) registro;
		}

		private static Conversor ConsultarConversorAtual(Type tipo)
		{
			var chave = tipo.GUID.ToString();
			return _conversores.GetOrAdd(chave, s => new Conversor());
		}

		private IEnumerable ConverterDataReader(Type tipo, IDataReader reader)
		{
			var tipoGenerico = typeof (Conversor<>).MakeGenericType(tipo);
			var binder = new Builder().CriarBinderDoObjeto(tipo);
			return (IEnumerable)Activator.CreateInstance(tipoGenerico, reader, binder);
		}

		public static IEnumerable ConverterDataReaderParaObjeto(Type tipo, IDataReader reader)
		{
			var conversor = ConsultarConversorAtual(tipo);
			return conversor.ConverterDataReader(tipo, reader);
		}
	}
}
