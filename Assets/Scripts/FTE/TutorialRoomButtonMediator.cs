using UnityEngine;

namespace Ambition
{
    public class TutorialRoomButtonMediator : TutorialHintView
    {
        public string RoomName;
        void OnEnable()
        {
            MapViewMediator map = this.transform.GetComponentInParent<MapViewMediator>();
            Transform xf = (map != null ? map.transform.Find(RoomName) : null);
            if (xf != null) xf.gameObject.AddComponent<TutorialTarget>().TutorialStep = this.TutorialStep;
        }
    }
}
