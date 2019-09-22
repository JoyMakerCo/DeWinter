using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    public class MapVO
    {
        public string Name;
        public FactionType Faction = FactionType.Neutral;
        public string[] Tags;
        public PartySize Size;
        public string AssetID;

        public IncidentVO[] Incidents;

        public MapVO() { }
        public MapVO(PartyVO party)
        {
            if (party == null) return;
            Name = party.Name;
            Faction = party.Faction;
            Tags = party.Tags?.ToArray();
            Size = party.Size;
        }

        // DEPRECATED OLD MAP STUFF
        //[JsonProperty("name")]
        //public string Name;

        //public RoomVO Entrance
        //{
        //	get { return Rooms!=null ? Rooms[0] : null; }
        //}

        //[JsonProperty("rooms", Order=0)]
        //public RoomVO[] Rooms;

        //[JsonProperty("doors", Order=1)]
        //private int[][] _doors
        //{
        //	set
        //	{
        //		for (int i=value.Length-1; i>=0; i--)
        //		{
        //			Rooms[i].Doors = new RoomVO[value[i].Length];
        //			for(int j=value[i].Length-1; j>=0; j--)
        //			{
        //				Rooms[i].Doors[j] = Rooms[value[i][j]];
        //			}
        //		}
        //	}
        //}
    }
}
