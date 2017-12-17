using FluentAssertions;

namespace RepositorioGenerico.SqlClient.Test
{
	public class GerenciadorEventosTransacao
	{

		private int _sequencia;

		private int _sequenciaEventoAntesConfirmar;
		private bool _executouEventoAntesConfirmar;

		private int _sequenciaEventoAoConfirmar;
		private bool _executouEventoAoConfirmar;

		private int _sequenciaEventoAntesCancelar;
		private bool _executouEventoAntesCancelar;

		private int _sequenciaEventoAoCancelar;
		private bool _executouEventoAoCancelar;

		public GerenciadorEventosTransacao(Transacao transacao)
		{
			Configurar(transacao);
		}

		private void Configurar(Transacao transacao)
		{
			transacao.AntesConfirmarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoAntesConfirmar = _sequencia;
				_executouEventoAntesConfirmar = true;
			};

			transacao.DepoisConfirmarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoAoConfirmar = _sequencia;
				_executouEventoAoConfirmar = true;
			};

			transacao.AntesCancelarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoAntesCancelar = _sequencia;
				_executouEventoAntesCancelar = true;
			};

			transacao.DepoisCancelarTransacao = (sender) =>
			{
				_sequencia++;
				_sequenciaEventoAoCancelar = _sequencia;
				_executouEventoAoCancelar = true;
			};
		}

		public void ReiniciarValidacao()
		{
			_sequencia = 0;
			_sequenciaEventoAntesConfirmar = 0;
			_sequenciaEventoAoConfirmar = 0;
			_sequenciaEventoAntesCancelar = 0;
			_sequenciaEventoAoCancelar = 0;

			_executouEventoAntesConfirmar = false;
			_executouEventoAoConfirmar = false;
			_executouEventoAntesCancelar = false;
			_executouEventoAoCancelar = false;
		}

		public void Validar(bool executarEventoAntesConfirmar, bool executarEventoAoConfirmar, 
			bool executarEventoAntesCancelar, bool executarEventoAoCancelar)
		{
			_executouEventoAntesConfirmar.Should().Be(executarEventoAntesConfirmar);
			_executouEventoAoConfirmar.Should().Be(executarEventoAoConfirmar);
			_executouEventoAntesCancelar.Should().Be(executarEventoAntesCancelar);
			_executouEventoAoCancelar.Should().Be(executarEventoAoCancelar);
		}

		public void Validar(int sequenciaEventoAntesConfirmar, int sequenciaEventoAoConfirmar,
			int sequenciaEventoAntesCancelar, int sequenciaEventoAoCancelar)
		{
			_sequenciaEventoAntesConfirmar.Should().Be(sequenciaEventoAntesConfirmar);
			_sequenciaEventoAoConfirmar.Should().Be(sequenciaEventoAoConfirmar);
			_sequenciaEventoAntesCancelar.Should().Be(sequenciaEventoAntesCancelar);
			_sequenciaEventoAoCancelar.Should().Be(sequenciaEventoAoCancelar);
		}

	}
}
