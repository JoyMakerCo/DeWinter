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

		private RemarkVO _remark;
        private RemarkVO[] _hand;
        private Image _image;
        private Vector3 _savedPosition;
        private int _index;

		void Awake ()
		{
            _image = GetComponent<Image>();
            _index = transform.GetSiblingIndex();
            _savedPosition = RemarkViews[_index].localPosition;
            AmbitionApp.Subscribe<RemarkVO[]>(HandleHand);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Subscribe(PartyMessages.END_CONVERSATION, HandleEndConversation);
            AmbitionApp.Subscribe(PartyMessages.FLEE_CONVERSATION, HandleEndConversation);
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO[]>(HandleHand);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Unsubscribe(PartyMessages.END_CONVERSATION, HandleEndConversation);
            AmbitionApp.Unsubscribe(PartyMessages.FLEE_CONVERSATION, HandleEndConversation);
        }

        private void HandleEndConversation()
        {
            _remark = null;
            _hand = null;
            gameObject.SetActive(false);
        }



        private void HandleHand(RemarkVO[] hand)
		{
            _hand = (RemarkVO[])(hand.Clone());
            _remark = _index < hand.Length ? hand[_index] : null;
            StopAllCoroutines();
            transform.localPosition = _savedPosition;
            if (_remark != null)
            {
                _image.sprite = RemarksConfig.GetSprite(_remark.Interest);
                Arrow.sprite = RemarksConfig.GetSprite(_remark.Interest + "_" + _remark.NumTargets.ToString());
                _image.color = Arrow.color = Color.white;
            }
            gameObject.SetActive(_remark != null);
        }

        private void HandleRemark(RemarkVO remark)
        {
            if (_remark != null)
            {
                bool isRemark = remark == _remark;
                _image.sprite = RemarksConfig.GetSprite(isRemark ? (_remark.Interest + "_Select") : _remark.Interest);
                _image.color = Arrow.color = (isRemark || remark == null) ? Color.white : Color.gray;
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
	}
}
