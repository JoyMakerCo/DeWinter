using System;
using UnityEngine;
namespace Ambition
{
    public struct ChapterVO
    {
        public string ID;
        public Sprite Splash;
        public FMODEvent Sting;
        public DateTime Date;

        public ChapterVO(string id, DateTime date, Sprite splash, FMODEvent sting)
        {
            ID = id;
            Date = date;
            Splash = splash;
            Sting = sting;
        }
    }
}
