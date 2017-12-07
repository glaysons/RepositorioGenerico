using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;

namespace Business.Mvc
{
	public static class HtmlHelperExtensions
	{

		public static HtmlString ToJson(this HtmlHelper sender, object value)
		{
			return new HtmlString(JsonConvert.SerializeObject(value));
		}

		public static HtmlString ToJson(this HtmlHelper sender, object value, Formatting formatting)
		{
			return new HtmlString(JsonConvert.SerializeObject(value, formatting));
		}

	}
}