using System;
using UnityEngine;

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
        public Gender Gender;
        public Sprite Portrait;
        public Sprite GetPose(string pose)
        {
            return Poses == null ? null : Array.Find(Poses, p => p.Label == pose).Pose;
        }
    }

    public class AvatarCollection : ScriptableObject
    {
        public AvatarConfig[] Avatars;

        public Sprite GetPortrait(string id)
        {
            return GetAvatar(id)?.Portrait;
        }

        public AvatarConfig GetAvatar(string ID)
        {
            return Array.Find(Avatars, a => a?.name == ID);
        }

        public Sprite GetPose(string avatarID, string pose)
        {
            return GetAvatar(avatarID).GetPose(pose);
        }

    #if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/Create Avatar Collection")]
        public static void CreateAvatarConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<AvatarCollection>();
        }
#endif
    }
}
