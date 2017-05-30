using System.Collections;

namespace Ambition
{
	public class RequestAdjustValueVO<T>
	{
		public string Type;
		public T Value;

		public RequestAdjustValueVO(string type, T value)
		{
			Type = type;
			Value = value;
		}
	}
}