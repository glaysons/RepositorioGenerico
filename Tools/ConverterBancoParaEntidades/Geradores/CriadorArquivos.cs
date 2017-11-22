using System;
using System.IO;
using System.Text;

namespace ConverterBancoParaEntidades.Geradores
{
	public static class CriadorArquivos
	{

		public static void Salvar(string destino, string arquivo, Action<StreamWriter> salvarArquivo)
		{
			if (!Directory.Exists(destino))
				Directory.CreateDirectory(destino);

			var nomeArquivo = (destino.EndsWith("\\"))
				? destino + arquivo
				: destino + "\\" + arquivo;

			if (File.Exists(nomeArquivo))
			{
				File.SetAttributes(nomeArquivo, FileAttributes.Temporary);
				File.Delete(nomeArquivo);
			}

			using (var conteudoArquivo = new FileStream(nomeArquivo, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (var writer = new StreamWriter(conteudoArquivo, Encoding.UTF8))
					salvarArquivo(writer);
				conteudoArquivo.Close();
			}
		}

	}
}
