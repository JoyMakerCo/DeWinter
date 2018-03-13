using System;
using System.Linq;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{  
    [Serializable]
    public struct PoseMap
    {
        public string Label;
        public Sprite Pose;
    }

    [Serializable]
	public struct AvatarVO
	{
        public string ID;
        public PoseMap[] Poses;
        public string Tags;
        public Gender Gender;
        public Sprite GetPose(string pose)
        {
            return Poses == null ? null : Array.Find(Poses, p=>p.Label == pose).Pose;
        }
    }

    public class AvatarCollection : ScriptableObject
    {
        public AvatarVO[] Avatars;

        public AvatarVO GetAvatar(string ID)
        {
            return Array.Find(Avatars, a => a.ID == ID);
        }

        public Sprite GetPose(string avatarID, string pose)
        {
            AvatarVO config = Array.Find(Avatars, a=>a.ID == avatarID);
            return config.GetPose(pose);
        }

        public AvatarVO[] FindByTag (params string[] tags)
        {
            return Array.FindAll(Avatars, a=>Array.TrueForAll(tags, a.Tags.Contains));
        }

    #if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create Avatar Collection")]
        public static void CreateAvatarConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<AvatarCollection>();
        }
#endif
    }
}
