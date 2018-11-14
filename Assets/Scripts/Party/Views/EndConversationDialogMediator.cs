using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Dialog;
using Core;

namespace Ambition
{
    public class EndConversationDialogMediator : DialogView, IPointerClickHandler
    {
        private ModelSvc _models = App.Service<ModelSvc>();

        public const string DIALOG_ID = "END_CONVERSATION";
        public Text TitleText;
        public Text SubText;
        public CommodityTableView Commodities;

        //This is not being handled via an Initialize function because this dialog needs to be brought up via the state machine, and there's currently no way to use 'Open Dialog' and set an array of commodities at the same time
        //While I could have made the state machine able to accept both a string and CommodityVO[] in the same state, I didn't feel comfortable making changes to something as critical as the state machine
        public void Awake()
        {
            ConversationModel conversationModel = _models.GetModel<ConversationModel>();
            Commodities.SetCommodities(conversationModel.Room.Rewards);
        }

        public void SetPhrase(string phrase)
        {
            TitleText.text = AmbitionApp.GetString(phrase + ".title");
            SubText.text = AmbitionApp.GetString(phrase + ".body");
        }

        public void OnPointerClick(PointerEventData data)
        {
            AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, ID);
            FadeView view = GetComponent<FadeView>();
            if (view == null) Close();
            else
            {
                view.FadeOut();
                StartCoroutine(WaitToClose(view.FadeOutSeconds));
            }
        }

        IEnumerator WaitToClose(float time)
        {
            yield return new WaitForSeconds(time);
            Close();
        }
    }
}
