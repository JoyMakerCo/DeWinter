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
            AmbitionApp.Subscribe<GameObject>(GameMessages.LOAD_SCENE, HandleScene);
        }

		void OnDestroy()
		{
            AmbitionApp.Unsubscribe<string>(GameMessages.LOAD_SCENE, HandleScene);
            AmbitionApp.Unsubscribe<GameObject>(GameMessages.LOAD_SCENE, HandleScene);
		}

		private void HandleScene(string sceneID)
		{
			if (_sceneID != sceneID)
            {
                PrefabMap map = Array.Find(Scenes, s=>s.ID == sceneID);
                HandleScene(map.Prefab, sceneID);
            }
		}

        void HandleScene(GameObject sceneObject) => HandleScene(sceneObject, sceneObject.name);
        private void HandleScene(GameObject prefab, string id)
        {
            if (prefab == null) return;
            _sceneID = id;

            StopAllCoroutines();

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            Debug.LogFormat( "instantiating prefab {0}", prefab.name);
            GameObject scene = Instantiate(prefab, transform);
            StartCoroutine(ReadyCheck(scene));
        }

        IEnumerator ReadyCheck(GameObject scene)
		{
			while (!scene.activeInHierarchy) yield return null;
			AmbitionApp.SendMessage(GameMessages.SCENE_LOADED, _sceneID);
		}
	}
}
