using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
    [Serializable]
    public class FactionVO
    {
        public FactionType Type;
        public int Modesty; //How modest do they like their outfits?
        public int Luxury; //How luxurious do they like their outfits?
        public bool Steadfast;
        public int Allegiance; //-100 means the Faction is devoted to the Third Estate, 100 means they are devoted to the Crown
        public int Power; //How powerful is this faction in the game?

        public FactionVO() { }
        public FactionVO(FactionVO faction)
        {
            Type = faction.Type;
            Modesty = faction.Modesty;
            Luxury = faction.Luxury;
            Steadfast = faction.Steadfast;
            Allegiance = faction.Allegiance;
            Power = faction.Power;
        }

        public override string ToString()
        {
            return "Faction: " + Type.ToString() +
                "\n modesty: " + Modesty.ToString() +
                "\n luxury: " + Luxury.ToString() +
                "\n steadfast: " + Steadfast.ToString() +
                "\n allegiance: " + Allegiance.ToString() +
                "\n power: " + Power.ToString();
        }
    }

    [Serializable]
    public struct FactionStandingsVO
    {
        public FactionType Faction;
        public int Allegiance;
        public int Power;
    }
}
