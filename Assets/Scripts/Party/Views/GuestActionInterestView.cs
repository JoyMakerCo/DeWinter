using UnityEngine.UI;

namespace Ambition
{
    public class GuestActionInterestView : GuestActionIconView
    {
        public SpriteConfig InterestSprites;
        public Image FromInterest;
        public Image ToInterest;
        private GuestActionVO _action;
        override public GuestActionVO Action
        {
            get { return _action; }
            set
            {
                _action = value;
                FromInterest.sprite = InterestSprites.GetSprite(_action.Tags[0]);
                ToInterest.sprite = InterestSprites.GetSprite(_action.Tags[1]);
            }
        }
    }
}
