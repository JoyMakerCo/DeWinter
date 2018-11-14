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
        public string Faction;
    }

    public class PartyMusicCollection : ScriptableObject
    {
        public PartyMusic[] PartyMusics;

        public FMODEvent GetFMODEvent(string faction)
        {
            List<PartyMusic> factionPartyMusics = new List<PartyMusic>();
            foreach(PartyMusic p in PartyMusics)
            {
                if(p.Faction == faction)
                {
                    factionPartyMusics.Add(p);
                }
            }
            FMODEvent fmodEvent = factionPartyMusics[UnityEngine.Random.Range(0, factionPartyMusics.Count)].Event;
            return fmodEvent;
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