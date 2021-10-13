using System;
using UnityEngine;
namespace Ambition
{
    public class AvatarConfig : ScriptableObject
    {
        public PoseMap[] Poses;
        public Gender Gender;
        public Sprite Portrait;
        public Sprite GetPose(string pose)
        {
            return Poses == null ? null : Array.Find(Poses, p => p.Label == pose).Pose;
        }
    }
}
