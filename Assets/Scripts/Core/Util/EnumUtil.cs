using System;
namespace Util
{
    public static class EnumUtil
    {
        public static T[] GetList<T>() where T:IConvertible
        {
            Type t = typeof(T);
#if DEBUG
            if (!t.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
#endif
            return (T[])(Enum.GetValues(t));
        }
    }
}
