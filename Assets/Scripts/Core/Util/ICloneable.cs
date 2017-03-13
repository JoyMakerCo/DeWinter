using System;

namespace Util
{
	public interface ICloneable<T>
	{
		T Clone();
	}
}