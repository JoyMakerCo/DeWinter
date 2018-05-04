using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Ambition
{
	public class SceneMediator : MonoBehaviour
	{
		public PrefabMap[] Scenes;
		private string _sceneID;
		void Awake()
		{
			AmbitionApp.Subscribe<string>(GameMessages.LOAD_SCENE, HandleScene);
		}

		void Start()
		{
			AmbitionApp.SendMessage(GameMessages.SCENE_LOADED);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<string>(GameMessages.LOAD_SCENE, HandleScene);
		}

		private void HandleScene(string sceneID)
		{
			if (_sceneID == sceneID) return;
			PrefabMap map = Array.Find(Scenes, s=>s.ID == sceneID);
			if (map.Prefab != null)
			{
				StopAllCoroutines();
				_sceneID = sceneID;
				foreach(Transform child in transform)
					Destroy(child.gameObject);

				GameObject scene = Instantiate(map.Prefab, this.transform);
				StartCoroutine(ReadyCheck(scene));
			}
		}

		IEnumerator ReadyCheck(GameObject scene)
		{
			while (!scene.activeInHierarchy) yield return null;
			AmbitionApp.SendMessage<string>(GameMessages.SCENE_LOADED, _sceneID);
		}
	}
}
