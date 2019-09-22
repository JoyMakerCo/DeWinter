using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core
{
    public class SaveStateVO : ScriptableObject
    {
        public List<SaveRecordVO> Records = new List<SaveRecordVO>();

        [Serializable]
        public struct SaveRecordVO
        {
            public string SaveID;
            public long SaveDate; // Can be converted to DateTime as ticks
            public string SaveData;

            public SaveRecordVO(string id, DateTime date, string json)
            {
                SaveID = id;
                SaveDate = date.Ticks;
                SaveData = json;
            }
        }
    }
}
