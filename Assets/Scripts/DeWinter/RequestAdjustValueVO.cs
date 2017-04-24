using System.Collections;

namespace DeWinter
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