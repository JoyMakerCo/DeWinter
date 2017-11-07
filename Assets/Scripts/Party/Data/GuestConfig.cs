using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
	[Serializable]
	public struct InterestMap
	{
		public string Interest;
		public Sprite Sprite;
	}

	public class GuestConfig : ScriptableObject
	{
		public InterestMap[] InterestSprites;
		public GuestSprite[] GuestSprites;

#if (UNITY_EDITOR)
		[MenuItem("Assets/Create/Create Guest Config")]
		public static void CreatePrefabConfig()
		{
			ScriptableObjectUtil.CreateScriptableObject<GuestConfig>();
		}
#endif
	}
}
