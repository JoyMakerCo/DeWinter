﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class MapScene : MonoBehaviour
    {
        public AvatarCollection Avatars;

        private MapModel _map;
        private MapView _loadedMap;

        public Sprite GetPortrait(string avatarID) => Avatars?.GetAvatar(avatarID).Portrait;

        private void OnEnable()
        {
            _map = AmbitionApp.GetModel<MapModel>();
            _map.Map.Observe(SetMap);
        }

        private void OnDisable() => _map.Map.Remove(SetMap);

        private void SetMap(MapVO map)
        {
            MapView view = _map.MapObject?.GetComponent<MapView>();
            if (view?.name != _loadedMap?.name)
            {
                _loadedMap = view;
                for(int i=transform.childCount-1; i>=0; i--)
                {
                    Destroy(transform.GetChild(i));
                }

                if (_loadedMap != null)
                    GameObject.Instantiate(_loadedMap.gameObject, this.transform, false);
            }
        }
    }
}