using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class DatabaseGeneratedAttribute : AutoIncrementoAttribute
	{

		public DatabaseGeneratedOption DatabaseGeneratedOption { get; set; }

		public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption)
			: base(ConverterDatabaseGeneratedOptionParaIncremento(databaseGeneratedOption))
		{

		}

		private static Incremento ConverterDatabaseGeneratedOptionParaIncremento(DatabaseGeneratedOption databaseGeneratedOption)
		{
			if (databaseGeneratedOption == DatabaseGeneratedOption.None)
				return Incremento.Nenhum;

			if (databaseGeneratedOption == DatabaseGeneratedOption.Identity)
				return Incremento.Identity;

			return Incremento.Calculado;
		}
	}

}
