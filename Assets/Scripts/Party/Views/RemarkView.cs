using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class RemarkView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
        private const float SLIDE_TIME = 1f;

        public Image Arrow;
        public SpriteConfig RemarksConfig;

        private Animator _animator;
		private RemarkVO _remark;
        private Image _image;
        private Vector3 _position;

		void Awake ()
		{
            _image = GetComponent<Image>();
            _position = transform.position;
            _animator = GetComponent<Animator>();
            AmbitionApp.Subscribe<List<RemarkVO>>(HandleHand);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<List<RemarkVO>>(HandleHand);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
		}

		private void HandleHand(List<RemarkVO> hand)
		{
            int index = transform.GetSiblingIndex();
            RemarkVO remark = (index < hand.Count) ? hand[index] : null;
            bool slide = false;//(_remark == null && remark != null);
            StopAllCoroutines();
            transform.position = _position;
            _remark = remark;
            if (_remark != null)
            {
                if (slide)
                {
                    Vector3 pos = _position;
                    pos.x = Screen.width;
                    transform.position = pos;
                }
                _image.sprite = RemarksConfig.GetSprite(_remark.Interest);
                Arrow.sprite = RemarksConfig.GetSprite(_remark.Interest + "_" + _remark.NumTargets.ToString());
            }
            gameObject.SetActive(_remark != null);
            if (slide) StartCoroutine(Slide());
		}

        private void HandleRemark(RemarkVO remark)
        {
            bool selected = (remark == _remark);
            Arrow.color = Color.white;
            if (_remark != null)
                _image.sprite = RemarksConfig.GetSprite(selected ? (_remark.Interest + "_Select") : _remark.Interest);
            //_animator.SetBool("Selected", remark == _remark);
        }

		public void OnPointerClick(PointerEventData eventData)
		{
			AmbitionApp.GetModel<ConversationModel>().Remark = _remark;
		}

        public void OnPointerDown(PointerEventData eventData)
        {
            Arrow.color = Color.grey;
            _image.sprite = RemarksConfig.GetSprite(_remark.Interest + "_Down");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Arrow.color = Color.white;
            _image.sprite = RemarksConfig.GetSprite(_remark.Interest);
        }

        IEnumerator Slide()
        {
            float from = transform.position.x;
            float to = _position.x;
            Vector3 pos = _position;
            float k = 1f / SLIDE_TIME;
            for (float t = 0; t < SLIDE_TIME; t += Time.deltaTime)
            {
                pos.x = from * (1f - (t * k)) + (t * k * to);
                transform.position = pos;
                yield return null;
            }
            transform.position = _position;
        }
	}
}
