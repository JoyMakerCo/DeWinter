using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
namespace Ambition
{
    public class MapView : SceneView, IAnalogInputHandler
    {
        public FactionType Faction = FactionType.None;
        public string[] Tags;
        public PartySize Size;
        public FMODEvent Music;

        public void HandleInput(Vector2[] input)
        {
        }
    }
}
