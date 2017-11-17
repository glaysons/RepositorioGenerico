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

		private int _sequenciaEventoAntesConfirmar;
		private bool _executouEventoAntesConfirmar;

		private int _sequenciaEventoAoConfirmar;
		private bool _executouEventoAoConfirmar;

		private int _sequenciaEventoAntesCancelar;
		private bool _executouEventoAntesCancelar;

		private int _sequenciaEventoAoCancelar;
		private bool _executouEventoAoCancelar;

		public GerenciadorEventosConexao(Conexao conexao)
		{
			Configurar(conexao);
		}

		private void Configurar(Conexao conexao)
		{

			conexao.AntesIniciarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoAntesIniciar = _sequencia;
				_executouEventoAntesIniciar = true;
			};

			conexao.DepoisIniciarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoDepoisIniciar = _sequencia;
				_executouEventoDepoisIniciar = true;
			};

			conexao.AntesConfirmarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoAntesConfirmar = _sequencia;
				_executouEventoAntesConfirmar = true;
			};

			conexao.DepoisConfirmarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoAoConfirmar = _sequencia;
				_executouEventoAoConfirmar = true;
			};

			conexao.AntesCancelarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoAntesCancelar = _sequencia;
				_executouEventoAntesCancelar = true;
			};

			conexao.DepoisCancelarTransacao = () =>
			{
				_sequencia++;
				_sequenciaEventoAoCancelar = _sequencia;
				_executouEventoAoCancelar = true;
			};
		}

		public void ReiniciarValidacao()
		{
			_sequencia = 0;
			_sequenciaEventoAntesIniciar = 0;
			_sequenciaEventoDepoisIniciar = 0;
			_sequenciaEventoAntesConfirmar = 0;
			_sequenciaEventoAoConfirmar = 0;
			_sequenciaEventoAntesCancelar = 0;
			_sequenciaEventoAoCancelar = 0;

			_executouEventoAntesIniciar = false;
			_executouEventoDepoisIniciar = false;
			_executouEventoAntesConfirmar = false;
			_executouEventoAoConfirmar = false;
			_executouEventoAntesCancelar = false;
			_executouEventoAoCancelar = false;
		}

		public void Validar(bool executarEventoAntesIniciar, bool executarEventoDepoisIniciar, 
			bool executarEventoAntesConfirmar, bool executarEventoAoConfirmar, 
			bool executarEventoAntesCancelar, bool executarEventoAoCancelar)
		{
			_executouEventoAntesIniciar.Should().Be(executarEventoAntesIniciar);
			_executouEventoDepoisIniciar.Should().Be(executarEventoDepoisIniciar);
			_executouEventoAntesConfirmar.Should().Be(executarEventoAntesConfirmar);
			_executouEventoAoConfirmar.Should().Be(executarEventoAoConfirmar);
			_executouEventoAntesCancelar.Should().Be(executarEventoAntesCancelar);
			_executouEventoAoCancelar.Should().Be(executarEventoAoCancelar);
		}

		public void Validar(int sequenciaEventoAntesIniciar, int sequenciaEventoDepoisIniciar,
			int sequenciaEventoAntesConfirmar, int sequenciaEventoAoConfirmar,
			int sequenciaEventoAntesCancelar, int sequenciaEventoAoCancelar)
		{
			_sequenciaEventoAntesIniciar.Should().Be(sequenciaEventoAntesIniciar);
			_sequenciaEventoDepoisIniciar.Should().Be(sequenciaEventoDepoisIniciar);
			_sequenciaEventoAntesConfirmar.Should().Be(sequenciaEventoAntesConfirmar);
			_sequenciaEventoAoConfirmar.Should().Be(sequenciaEventoAoConfirmar);
			_sequenciaEventoAntesCancelar.Should().Be(sequenciaEventoAntesCancelar);
			_sequenciaEventoAoCancelar.Should().Be(sequenciaEventoAoCancelar);
		}

	}
}
