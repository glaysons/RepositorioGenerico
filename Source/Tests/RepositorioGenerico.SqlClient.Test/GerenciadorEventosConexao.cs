using FluentAssertions;

namespace RepositorioGenerico.SqlClient.Test
{
	public class GerenciadorEventosConexao
	{

		private int _sequencia;

		private int _sequenciaEventoAntesIniciar;
		private bool _executouEventoAntesIniciar;

		private int _sequenciaEventoDepoisIniciar;
		private bool _executouEventoDepoisIniciar;

		public GerenciadorEventosConexao(Conexao conexao)
		{
			Configurar(conexao);
		}

		private void Configurar(Conexao conexao)
		{

			conexao.AntesIniciarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoAntesIniciar = _sequencia;
				_executouEventoAntesIniciar = true;
			};

			conexao.DepoisIniciarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoDepoisIniciar = _sequencia;
				_executouEventoDepoisIniciar = true;
			};


		}

		public void ReiniciarValidacao()
		{
			_sequencia = 0;
			_sequenciaEventoAntesIniciar = 0;
			_sequenciaEventoDepoisIniciar = 0;

			_executouEventoAntesIniciar = false;
			_executouEventoDepoisIniciar = false;
		}

		public void Validar(bool executarEventoAntesIniciar, bool executarEventoDepoisIniciar)
		{
			_executouEventoAntesIniciar.Should().Be(executarEventoAntesIniciar);
			_executouEventoDepoisIniciar.Should().Be(executarEventoDepoisIniciar);
		}

		public void Validar(int sequenciaEventoAntesIniciar, int sequenciaEventoDepoisIniciar)
		{
			_sequenciaEventoAntesIniciar.Should().Be(sequenciaEventoAntesIniciar);
			_sequenciaEventoDepoisIniciar.Should().Be(sequenciaEventoDepoisIniciar);
		}

	}
}
