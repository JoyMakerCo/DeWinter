using System;
using UnityEngine;

#if (UNITY_EDITOR)
	using UnityEditor;
#endif

namespace Util
{
	[Serializable]
	public struct PrefabMap
	{
		public string ID;
		public GameObject Prefab;
	}

	public class PrefabMapConfig : ScriptableObject
	{
		public PrefabMap [] Prefabs;

		public GameObject Instantiate(string ID)
		{
			PrefabMap map = Array.Find(Prefabs, p=>p.ID==ID);
			return (!map.Equals(default(PrefabMap)))
				? GameObject.Instantiate<GameObject>(map.Prefab) : null;
		}
		
		public GameObject Instantiate(string ID, Transform parent)
		{
			PrefabMap map = Array.Find(Prefabs, p=>p.ID==ID);
			return (!map.Equals(default(PrefabMap)))
				? GameObject.Instantiate<GameObject>(map.Prefab, parent) : null;
		}

		public GameObject Instantiate(string ID, Vector3 position, Quaternion rotation)
		{
			PrefabMap map = Array.Find(Prefabs, p=>p.ID==ID);
			return (!map.Equals(default(PrefabMap)))
				? GameObject.Instantiate<GameObject>(map.Prefab, position, rotation) : null;
		}

		public GameObject Instantiate(string ID, Transform parent, bool worldPositionStays)
		{
			PrefabMap map = Array.Find(Prefabs, p=>p.ID==ID);
			return (!map.Equals(default(PrefabMap)))
				? GameObject.Instantiate<GameObject>(map.Prefab, parent, worldPositionStays) : null;
		}

		public GameObject Instantiate(string ID, Vector3 position, Quaternion rotation, Transform parent)
		{
			PrefabMap map = Array.Find(Prefabs, p=>p.ID==ID);
			return (!map.Equals(default(PrefabMap)))
				? GameObject.Instantiate<GameObject>(map.Prefab, position, rotation, parent) : null;
		}

#if (UNITY_EDITOR)
		[MenuItem("Assets/Create/Create Prefab Map")]
		public static void CreatePrefabConfig()
		{
			Util.ScriptableObjectUtil.CreateScriptableObject<PrefabMapConfig>();
		}
#endif
	}
}
