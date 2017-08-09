using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

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

		[MenuItem("Assets/Create/Create Guest Config")]
		public static void CreatePrefabConfig()
		{
			ScriptableObjectUtil.CreateScriptableObject<GuestConfig>();
		}
	}
}
