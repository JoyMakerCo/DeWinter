using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Ambition
{  
    [Serializable]
    public struct AvatarMap
    {
        public string Label;
        public Sprite Pose;
    }

    [Serializable]
	public class AvatarVO
	{
        public string ID;
        public AvatarMap[] Poses;
        public string Tags;
        public Gender Gender;
        public Sprite GetPose(string pose)
        {
            return Array.Find(Poses, p=>p.Label == pose).Pose;
        }
    }

    public class AvatarConfig : ScriptableObject
    {
        public AvatarVO[] Avatars;

        public AvatarVO GetAvatar(string ID)
        {
            return Array.Find(Avatars, a => a.ID == ID);
        }

        public AvatarVO[] FindByTag (params string[] tags)
        {
            return Array.FindAll(Avatars, a=>Array.TrueForAll(tags, a.Tags.Contains));
        }

    #if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create Avatar Config")]
        public static void CreateAvatarConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<AvatarConfig>();
        }
#endif
    }
}
