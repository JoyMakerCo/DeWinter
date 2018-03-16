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
	public class AmbientClip : ScriptableObject
	{
		public AudioClip Intro;
		public AudioClip Loop;
		public AudioClip Ending;

#if (UNITY_EDITOR)
		[MenuItem("Assets/Create/Create Ambient Audio Clip")]
		public static void CreateAmbientAudioClip()
		{
			ScriptableObjectUtil.CreateScriptableObject<AmbientClip>();
		}
#endif
	}
}
