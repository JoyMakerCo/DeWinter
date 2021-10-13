using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LivreView : MonoBehaviour
	{
        public Text FloatingText;
        public Animator TextAnimator;

        private float RATE = 400f; //Livre counted per second

		private Text _text;
        private GameModel _Model => AmbitionApp.GetModel<GameModel>();
        private int _livre = -1;

        void OnEnable ()
		{
			_text = _text ?? GetComponent<Text>();
            _livre = -1;
            AmbitionApp.Subscribe<int>(GameConsts.LIVRE, HandleLivre);
            HandleLivre(AmbitionApp.Game.Livre);
		}

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(GameConsts.LIVRE, HandleLivre);
            StopAllCoroutines();
        }

        private void HandleLivre(int livre)
        {
            if (livre == _livre) return;
            StopAllCoroutines();
            bool gain = livre > _livre;
            if (_livre < 0) _text.text = LivreString(livre);
            else
            {
                StartCoroutine(Count(_livre, livre));
                if (FloatingText != null)
                {
                    FloatingText.text = (gain ? "+" : "-") + LivreString(System.Math.Abs(livre - _livre));
                }
                else
                {
                    Debug.LogError("LivreView missing FloatingText");
                }
                if (TextAnimator != null)
                {
                    TextAnimator.SetInteger("Amount", livre - _livre);
                }
                else
                {
                    Debug.LogError("LivreView missing TextAnimator");
                }
            }
            _livre = livre;
        }

        IEnumerator Count(int l0, int l1)
        {
            float duration = (float)(System.Math.Abs(l1 - l0)) / RATE;
            for (float t=0f; t<duration; t+=Time.deltaTime)
            {
                _text.text = LivreString(l0 + (int)((l1 - l0) * t / duration));
                yield return null;
            }
            _text.text = LivreString(l1);
        }

        private string LivreString(int amount)=> "£" + (amount == 0 ? "0" : amount.ToString("### ###"));
    }
}
