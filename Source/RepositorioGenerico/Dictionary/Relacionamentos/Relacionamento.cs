namespace RepositorioGenerico.Dictionary.Relacionamentos
{
	public class Relacionamento
	{

		private readonly TiposRelacionamento _tipo;
		private readonly Dicionario _dicionario;
		private readonly string[] _chaveEstrangeira;

		public TiposRelacionamento Tipo
		{
			get { return _tipo; }
		}

		public Dicionario Dicionario
		{
			get { return _dicionario; }
		}

		public string[] ChaveEstrangeira { get { return _chaveEstrangeira;} }

		public Relacionamento(TiposRelacionamento tipo, Dicionario dicionario, string chaveEstrangeira)
		{
			_tipo = tipo;
			_dicionario = dicionario;
			_chaveEstrangeira = chaveEstrangeira.Split(',');
		}

		public void AplicarChaveAscendente(object[] chaveAscendente, object objeto)
		{
			var n = 0;
			foreach (var campoEstrangeiro in ChaveEstrangeira)
			{
				var campo = Dicionario.ConsultarPorCampo(campoEstrangeiro);
				campo.Propriedade.SetValue(objeto, chaveAscendente[n], null);
				n++;
			}
		}

		public bool PossuiChaveAscendente(object[] chaveAscendente, object objeto)
		{
			var n = 0;
			foreach (var campoEstrangeiro in ChaveEstrangeira)
			{
				var campo = Dicionario.ConsultarPorCampo(campoEstrangeiro);
				var valor = campo.Propriedade.GetValue(objeto, null);
				if (!Equals(chaveAscendente[n], valor))
					return false;
				n++;
			}
			return (n > 0);
		}
	}
}
