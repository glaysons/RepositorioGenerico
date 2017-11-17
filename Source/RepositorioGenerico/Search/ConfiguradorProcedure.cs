using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
    public class ConfiguradorProcedure : Configurador, IConfiguracaoProcedure
    {

        private string _nomeProcedure;

        public ConfiguradorProcedure(IDbCommand comando) : base(comando)
        {

        }

        public IConfiguracaoProcedure DefinirProcedure(string nome)
        {
            _nomeProcedure = nome;
            return this;
        }

        public override void Preparar()
        {
            Comando.CommandType = CommandType.StoredProcedure;
            Comando.CommandText = _nomeProcedure;
            Comando.CommandTimeout = 0;
        }

    }
}
