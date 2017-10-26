using System;
using System.Collections.Generic;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.Pattern;

namespace RepositorioGenerico.Fake.Contextos
{
	public class PersistenciaFake<TObjeto> : PersistenciaFakeBase<TObjeto, IList<TObjeto>, TObjeto> where TObjeto : IEntidade
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

		public PersistenciaFake(Dicionario dicionario)
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

		public override TObjeto Criar()
		{
			var objeto = (TObjeto)Activator.CreateInstance(typeof(TObjeto));
			foreach (var item in Dicionario.Itens)
				if (item.ValorPadrao != null)
					item.Propriedade.SetValue(objeto, item.ValorPadrao, null);
			return objeto;
		}

		public override void Salvar(IConexao conexao, object registro)
		{
			var item = registro as IEntidade;
			if (item == null)
				throw new TipoDeObjetoInvalidoException(typeof(TObjeto).Name);
			Atualizador.Salvar(conexao, (TObjeto)item);
			item.EstadoEntidade = EstadosEntidade.NaoModificado;
		}

	}
}
