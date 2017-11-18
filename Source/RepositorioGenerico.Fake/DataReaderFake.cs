using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace RepositorioGenerico.Fake
{
	public class DataReaderFake : DbDataReader
	{

		private readonly DataView _dados;
		private readonly int _top;

		private bool _podeLer;
		private int _indiceRegistroAtual;
		private DataRow _registroAtual;

		public DataReaderFake(DataView dados) : this(dados, 0)
		{

		}

		public DataReaderFake(DataView dados, int top)
		{
			_podeLer = false;
			_indiceRegistroAtual = 0;
			_registroAtual = null;
			_dados = dados;
			_top = top;
		}

		public override string GetName(int i)
		{
			if ((i >= 0) && (i < _dados.Table.Columns.Count))
				return _dados.Table.Columns[i].ColumnName;
			throw new IndexOutOfRangeException();
		}

		public override string GetDataTypeName(int i)
		{
			throw new NotImplementedException();
		}

		public override IEnumerator GetEnumerator()
		{
			return _dados.GetEnumerator();
		}

		public override Type GetFieldType(int i)
		{
			if ((i >= 0) && (i < _dados.Table.Columns.Count))
			{
				var tipo = _dados.Table.Columns[i].DataType;
				if ((tipo != typeof(string)) && _dados.Table.Columns[i].AllowDBNull)
					return typeof(Nullable<>).MakeGenericType(tipo);
				return tipo;
			}
			throw new IndexOutOfRangeException();
		}

		public override object GetValue(int i)
		{
			if ((i >= 0) && (i < _dados.Table.Columns.Count))
				return (_registroAtual[i] == DBNull.Value)
					? null
					: _registroAtual[i];
			throw new IndexOutOfRangeException();
		}

		public override int GetValues(object[] values)
		{
			var maximo = (values.Length > FieldCount) ? FieldCount : values.Length;
			for (var indice = 0; indice < maximo; indice++)
				values[indice] = GetValue(indice);
			return maximo;
		}

		public override int GetOrdinal(string name)
		{
			name = name.ToLower();
			for (var indice = 0; indice < _dados.Table.Columns.Count; indice++)
				if (string.Equals(_dados.Table.Columns[indice].ColumnName.ToLower(), name))
					return indice;
			throw new IndexOutOfRangeException();
		}

		public override bool GetBoolean(int i)
		{
			return (bool)GetValue(i);
		}

		public override byte GetByte(int i)
		{
			return (byte)GetValue(i);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public override char GetChar(int i)
		{
			return (char)GetValue(i);
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public override Guid GetGuid(int i)
		{
			return (Guid)GetValue(i);
		}

		public override short GetInt16(int i)
		{
			return (short)GetValue(i);
		}

		public override int GetInt32(int i)
		{
			return (int)GetValue(i);
		}

		public override long GetInt64(int i)
		{
			return (long)GetValue(i);
		}

		public override float GetFloat(int i)
		{
			return (float)GetValue(i);
		}

		public override double GetDouble(int i)
		{
			return (double)GetValue(i);
		}

		public override string GetString(int i)
		{
			return (string)GetValue(i);
		}

		public override decimal GetDecimal(int i)
		{
			return (decimal)GetValue(i);
		}

		public override DateTime GetDateTime(int i)
		{
			return (DateTime)GetValue(i);
		}

		public override bool IsDBNull(int i)
		{
			return (GetValue(i) == null);
		}

		public override int FieldCount
		{
			get { return _dados.Table.Columns.Count; }
		}

		public override bool HasRows
		{
			get { return (_dados.Count > 0); }
		}

		public override object this[int i]
		{
			get { return GetValue(i); }
		}

		public override object this[string name]
		{
			get { return GetValue(GetOrdinal(name)); }
		}

		public override void Close()
		{
			_podeLer = false;
		}

		public override DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public override bool NextResult()
		{
			throw new NotImplementedException();
		}

		public override bool Read()
		{
			var registros = _dados.Count;
			_podeLer = ((registros > 0) && (_indiceRegistroAtual < _dados.Count) && (_top < 1 || _indiceRegistroAtual < _top));
			if (!_podeLer)
				return false;
			_registroAtual = _dados[_indiceRegistroAtual].Row;
			_indiceRegistroAtual++;
			return true;
		}

		public override int Depth
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsClosed
		{
			get { return (_indiceRegistroAtual >= _dados.Count); }
		}

		public override int RecordsAffected
		{
			get { return _dados.Count; }
		}

	}
}
