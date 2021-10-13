using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
    public class EstateView : SceneView, ISubmitHandler, IButtonInputHandler, IAnalogInputHandler
    {
        private const float SWITCH_VIEWS_SECONDS = .2f;
        private const float SWITCH_VIEWS_SECONDS_1 = 1f/SWITCH_VIEWS_SECONDS;

        public GameObject Selector;
        public GameObject CurrentView;

        public void  ShowView(GameObject view)
        {
            CurrentView.SetActive(false);
            CurrentView = view;
            CurrentView.SetActive(true);
        }

        public void MoveSelector(Button button) => StartCoroutine(MoveSelector(button.transform));

        IEnumerator MoveSelector(Transform transform)
        {
            Vector3 pos = Selector.transform.position;
            float x0 = pos.x;
            float dx = transform.position.x - x0;
            float q;
            for (float t = 0f; t < SWITCH_VIEWS_SECONDS; t += Time.deltaTime)
            {
                q = SWITCH_VIEWS_SECONDS_1 * t - 1f;
                pos.x = x0 + dx * (1f - q * q);
                Selector.transform.position = pos;
                yield return null;
            }
            pos.x = transform.position.x;
            Selector.transform.position = pos;
        }

        public void Submit() { }
        public void Cancel()
        {
#if UNITY_STANDALONE
            AmbitionApp.OpenDialog(DialogConsts.GAME_MENU);
#else

#endif
        }

        public void HandleInput(Vector2 [] input)
        {

        }

        public void HandleInput(string input, bool holding)
        {

        }

        private void OnEnable()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, AmbitionApp.Game.GetChapter().EstateMusic);
        }
    }
}
