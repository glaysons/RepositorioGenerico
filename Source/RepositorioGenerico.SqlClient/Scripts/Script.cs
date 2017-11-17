namespace RepositorioGenerico.SqlClient.Scripts
{
	internal class Script
	{

		public string Insert { get; private set; }
		public string Update { get; private set; }
		public string Delete { get; private set; }
		public string AutoIncremento { get; private set; }

		public Script(string insert, string update, string delete, string autoIncremento)
		{
			Insert = insert;
			Update = update;
			Delete = delete;
			AutoIncremento = autoIncremento;
		}
	}
}
