using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace RepositorioGenerico.Fake
{
	public class DataParametersCollectionFake : Collection<IDataParameter>, IDataParameterCollection
	{

		public bool Contains(string parameterName)
		{
			return this.Any(item => string.Equals(item.ParameterName, parameterName));
		}

		public int IndexOf(string parameterName)
		{
			for (var i = 0; i < Count; i++)
				if (string.Equals(this[i].ParameterName, parameterName))
					return i;
			return -1;
		}

		public void RemoveAt(string parameterName)
		{
			for (var i = 0; i < Count; i++)
				if (string.Equals(this[i].ParameterName, parameterName))
				{
					RemoveAt(i);
					return;
				}
		}

		public object this[string parameterName]
		{
			get
			{
				var n = IndexOf(parameterName);
				return (n > -1) 
					? this[n] 
					: null;
			}
			set
			{
				var parametro = value as IDataParameter;
				var n = IndexOf(parameterName);
				if (n > -1)
					this[n] = parametro;
				else
					Add(parametro);
			}
		}
	}
}
