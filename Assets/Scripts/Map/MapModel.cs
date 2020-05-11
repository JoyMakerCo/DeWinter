using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
	public class MapModel : Model, IDisposable, IResettable, Util.IInitializable
	{
        private const string PATH = "PartyMaps/";

        public GameObject MapObject { get; private set; }
        public Observable<MapVO> Map;
        public MapVO[] Maps;

        private Dictionary<PartyVO, MapView> _partyMaps = new Dictionary<PartyVO, MapView>();

        public void Initialize()
        {
            GameObject[] maps = Resources.LoadAll<GameObject>(PATH);
            MapView[] views = maps.Select(m => m.GetComponent<MapView>()).ToArray();
            Maps = views.Where(v=>v!=null).Select(v => v.CreateMapVO()).ToArray();
            Resources.UnloadUnusedAssets();
            Map.Observe(LoadMap);
            maps = null;
            views = null;
        }

        public void Dispose() => Map.Remove(LoadMap);

        private void LoadMap(MapVO map)
        {
            if (MapObject?.name != map?.AssetID)
            {
                map = Array.Find(Maps, m => m.AssetID == map.AssetID);
                MapObject = Resources.Load<GameObject>(PATH + map.AssetID);
                if (MapObject != null) Map.Value = map;
            }
        }

        public void SaveMap(PartyVO party, MapView map)
        {
            if (party != null) _partyMaps[party] = map;
        }

        public bool LoadMap(PartyVO party)
        {
            if (party != null && _partyMaps.TryGetValue(party, out MapView map) && map != null)
            {
                MapObject = map.gameObject;
                Map.Value = map.CreateMapVO();
                return true;
            }
            return false;
        }

        public bool Restore(string data)
        {
            return true;
        }

        public string Save()
        {
            return "";
        }

        public void Reset()
        {
            MapObject = null;
            Map.Value = null;
            _partyMaps.Clear();
        }
    }
}
