using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
    [Serializable]
    public struct PartyMusic
    {
        public FMODEvent Event;
        public bool Map; //Is this the map music, or the conversation music?
        public string Faction;
    }

    //TO DO: Get it to randomly select conversation music
    public class PartyMusicCollection : ScriptableObject
    {
        public PartyMusic[] PartyMusics;

        public FMODEvent GetFMODEvent(bool map, string faction)
        {
            List<PartyMusic> tempMusics = new List<PartyMusic>();
            foreach (PartyMusic p in PartyMusics)
            {
                if(p.Map == map && p.Faction == faction)
                {
                    tempMusics.Add(p);
                }
            }
            return tempMusics[UnityEngine.Random.Range(0, tempMusics.Count())].Event; // Returns a randomly chosen event from the list, or just that that particular event
        }

        #if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create Party Music Collection")]
        public static void CreatePartyMusicCollection()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<PartyMusicCollection>();
        }
        #endif
    }
}