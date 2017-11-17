using System;
using System.Collections.Generic;
using System.Linq;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Mapas.Patterns;
using RepositorioGenerico.Entities.Mapas;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.Dictionary.Mapas
{
	public static class MapaItemDicionarioBuilder
	{

		public static IMapa CriarMapaBuilder(Type tipo)
		{
			var builder = CriarModelBuilder(tipo);
			ReferenciarMapa(tipo, builder);
			return builder;
		}

		private static IMapa CriarModelBuilder(Type tipo)
		{
			var tipoBuilder = typeof(Mapa<>);
			var builder = tipoBuilder.MakeGenericType(tipo);
			return (IMapa)Activator.CreateInstance(builder);
		}

		private static void ReferenciarMapa(Type tipo, IMapa builder)
		{
			var tipoMapa = typeof(IMapaEntidade<>);
			var tipoGenerico = tipoMapa.MakeGenericType(tipo);
			var mapaPersonalizado = tipo.Assembly.GetTypes().FirstOrDefault(t => tipoGenerico.IsAssignableFrom(t));
			if (mapaPersonalizado == null)
				throw new NaoFoiPossivelLocalizarMapaRelacionadoException(tipo.Name);
			var mapa = Activator.CreateInstance(mapaPersonalizado);
			if (mapa == null)
				throw new NaoFoiPossivelCriarMapaRelacionadoException(tipo.Name);
			var referenciar = mapaPersonalizado.GetMethod("Referenciar");
			referenciar.Invoke(mapa, new object[] { builder });
		}

		public static IList<ItemDicionario> ConsultarItensDicionario(IMapa builder, Type tipo)
		{
			var id = 0;
			var propriedades = tipo.GetProperties();
			var itens = from propriedade in propriedades
				let propriedadeReferenciada = builder.ConsultarPropriedadeDaTabela(propriedade)
				select ItemDicionarioFactory.CriarItemDicionario(id++, propriedade, propriedadeReferenciada);
			return itens.ToList();
		}

	}
}
