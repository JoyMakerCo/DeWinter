using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GuestActionInterestView : GuestActionIcon
    {
        public SpriteConfig InterestSprites;
        public Image FromInterest;
        public Image ToInterest;
        public override void SetAction(GuestActionVO action)
        {
            FromInterest.sprite = InterestSprites.GetSprite(action.Tags[0]);
            ToInterest.sprite = InterestSprites.GetSprite(action.Tags[1]);
        }
    }
}
