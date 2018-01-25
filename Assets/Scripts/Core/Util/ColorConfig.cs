using System;
using UnityEngine;

#if (UNITY_EDITOR)
	using UnityEditor;
#endif

namespace Util
{
	[Serializable]
	public struct ColorMap
	{
		public string ID;
		public Color Color;
	}

	public class ColorConfig : ScriptableObject
	{
		public ColorMap [] Colors;

		public Color GetColor(string ID)
		{
			ColorMap map = Array.Find(Colors, c => c.ID == ID);
			return map.Color;
		}

#if (UNITY_EDITOR)
		[MenuItem("Assets/Create/Create Color Map")]
		public static void CreateColorConfig()
		{
			Util.ScriptableObjectUtil.CreateScriptableObject<ColorConfig>();
		}
#endif
	}
}
