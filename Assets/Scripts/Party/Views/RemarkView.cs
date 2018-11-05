using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class RemarkView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
        private const float SLIDE_TIME = .5f;
        private const string SELECTED = "Selected";
        private const string USE = "Use";
        private const string REMOVE = "Remove";
        private const string DRAW = "Draw";
        private const string FILL_IN = "Fill";
        private const string SACRIFICE = "Sacrifice";

        public Image Arrow;
        public SpriteConfig RemarksConfig;
        public Transform[] RemarkViews;

        private Animator _animator;
		private RemarkVO _remark;
        private RemarkVO[] _hand;
        private Image _image;
        private Vector3 _savedPosition;
        private int _index;

		void Awake ()
		{
            _image = GetComponent<Image>();
            _index = transform.GetSiblingIndex();
            _animator = GetComponent<Animator>();
            AmbitionApp.Subscribe<RemarkVO[]>(HandleHand);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            _savedPosition = RemarkViews[_index].localPosition;
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO[]>(HandleHand);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
		}

		private void HandleHand(RemarkVO[] hand)
		{
            int altIndex = ((_remark == null || _hand == null) ? -1 : Array.IndexOf(_hand, _remark));
            if (_hand == null) _hand = new RemarkVO[hand.Length];
            hand.CopyTo(_hand, 0);
            _remark = _index < hand.Length ? hand[_index] : null;
            StopAllCoroutines();
            transform.localPosition = _savedPosition;
            if (_remark != null)
            {
                _image.sprite = RemarksConfig.GetSprite(_remark.Interest);
                Arrow.sprite = RemarksConfig.GetSprite(_remark.Interest + "_" + _remark.NumTargets.ToString());
                //if (altIndex < 0)
                //{
                //    _animator.SetTrigger(DRAW);
                //}
                //else if (altIndex != _index)
                if (altIndex >= 0 && altIndex != _index)
                {
                    StartCoroutine(Slide(RemarkViews[altIndex].localPosition));
                    //_animator.SetTrigger(FILL_IN);
                }
            }
            gameObject.SetActive(_remark != null);
        }

        private void HandleRemark(RemarkVO remark)
        {
            if (_remark != null)
            {
                bool isRemark = remark == _remark;
                //_animator.SetBool(SELECTED, isRemark);

                Arrow.color = Color.white;
                _image.sprite = RemarksConfig.GetSprite(isRemark ? (_remark.Interest + "_Select") : _remark.Interest);
            }
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

        IEnumerator Slide(Vector3 pos)
        {
            float T1 = 1f / SLIDE_TIME;
            transform.localPosition = pos;
            for (float t = 0f; t < SLIDE_TIME; t+=Time.deltaTime)
            {
                transform.localPosition = (pos * (SLIDE_TIME - t) + _savedPosition*t)*T1;
                yield return null;
            }
            transform.localPosition = _savedPosition;
        }
	}
}
