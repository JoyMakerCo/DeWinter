using System;
using UnityEngine;

namespace Ambition
{
	[Serializable]
	public struct PrefabElement
	{
		public string ID;
		public GameObject Prefab;
	}


	public class PrefabMap : ScriptableObject
	{
		public PrefabElement [] Prefabs;

		public GameObject Intantiate(string ID)
		{
			PrefabElement result = Array.Find(Prefabs, p=>p.ID == ID);
			return (result != null) ? GameObject.Instantiate<GameObject>(result.Prefab) : null;
		}

		public GameObject Intantiate(string ID, Transform parent)
		{
			PrefabElement result = Array.Find(Prefabs, p=>p.ID == ID);
			return (result != null) ? GameObject.Instantiate<GameObject>(result.Prefab, parent) : null;
		}

		public GameObject Intantiate(string ID, Vector3 position, Quaternion rotation)
		{
			PrefabElement result = Array.Find(Prefabs, p=>p.ID == ID);
			return (result != null) ? GameObject.Instantiate<GameObject>(result.Prefab, position, rotation) : null;
		}

		public GameObject Intantiate(string ID, Transform parent, bool worldPositionStays)
		{
			PrefabElement result = Array.Find(Prefabs, p=>p.ID == ID);
			return (result != null) ? GameObject.Instantiate<GameObject>(result.Prefab, parent, worldPositionStays) : null;
		}

		public GameObject Intantiate(string ID, Vector3 position, Quaternion rotation, Transform parent)
		{
			PrefabElement result = Array.Find(Prefabs, p=>p.ID == ID);
			return (result != null) ? GameObject.Instantiate<GameObject>(result.Prefab, position, rotation, parent) : null;
		}


	}
}
