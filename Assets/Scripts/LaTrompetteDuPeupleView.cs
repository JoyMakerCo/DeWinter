using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class LaTrompetteDuPeupleView : MonoBehaviour
    {
        private const string ACTIVE = "Active";
        private const float SLIDER_TRANSITION = .5f;
        private const float TEXTBOX_TIMER = 5f;

        public Text GossipValue;
        public Image GossipFactionSymbol;
        public Text GossipNameText;
        public Text GossipDescriptionText;
        public Text GossipEffectText;
        public Slider FreshnessSlider;

        public Animator animator;
        public Animator textboxAnimator;

        public Image Pierre;
        public Text SpeechBubbleText;

        public Sprite PierreHappy;
        public FMODEvent HappySound;

        public Sprite PierreNeutral;
        public FMODEvent NeutralSound;

        public Sprite PierreAnnoyed;
        public FMODEvent AnnoyedSound;

        public Text EffectText;

        public Button SellBtn;
        public Button PeddleBtn;

        public SpriteConfig FactionSprites;

        public int MinRelevanceSliderValue;

        private float _maxAge;

        private List<QuestVO> _quests;

        private GossipVO _gossip;
        public GossipVO Gossip
        {
            get => _gossip;
            private set
            {
                bool isEnabled = value != null;
                float age = isEnabled ? (float)(AmbitionApp.Calendar.Day - value.Created) : 0;
                GossipModel model = AmbitionApp.Gossip;
                float sliderValue = (age > _maxAge)
                    ? .5f * (float)MinRelevanceSliderValue
                    : (float)(MinRelevanceSliderValue) + (float)(100 - MinRelevanceSliderValue) * (_maxAge - age) / _maxAge;
                float price = model.GetValue(value, AmbitionApp.Calendar.Day);

                _gossip = value;

                SellBtn.interactable = isEnabled;
                PeddleBtn.interactable = isEnabled;

                GossipNameText.text = isEnabled ? model.GetName(_gossip) : "";
                GossipDescriptionText.text = isEnabled ? model.GetDescription(_gossip) : "";
                GossipEffectText.text = isEnabled ? GetEffectString(_gossip) : "";
                StartCoroutine(StartTransition(isEnabled ? sliderValue : 0));
                GossipValue.text = price > 0 ? price.ToString("£### ###") : "£0";
                GossipFactionSymbol.enabled = isEnabled;
                if (isEnabled) GossipFactionSymbol.sprite = FactionSprites.GetSprite(_gossip.Faction.ToString());
            }
        }

        public void SellGossip() //This has to be handled in a pop-up because the player has to evaluate the odds and confirm their decision
        {
            Dictionary<string, string> subs = new Dictionary<string, string>();
            int price = AmbitionApp.Gossip.GetValue(_gossip, AmbitionApp.Calendar.Day);
            subs["$GOSSIPNAME"] = AmbitionApp.Gossip.GetName(_gossip);
            subs["$GOSSIPPRICE"] = price > 0 ? price.ToString("£### ###") : "£0";
            subs["$CAUGHTODDS"] = AmbitionApp.Localize("gossip_caught_odds." + AmbitionApp.Gossip.GossipActivity.ToString());

            AmbitionApp.OpenDialog("sell_gossip_dialog", OnConfirmSellDialog, subs);
        }

        public void PeddleInfluencePopUp() //This has to be handled in a pop-up because the player has to evaluate the odds and confirm their decision
        {
            AmbitionApp.OpenDialog(DialogConsts.PEDDLE_INFLUENCE, Gossip);
        }

        public void OnConfirmSellDialog()
        {
            AmbitionApp.SendMessage(InventoryMessages.SELL_GOSSIP, _gossip);
            HandleTradeGossip(_gossip);
        }

        public void HideTextBox()
        {
            StopAllCoroutines();
            textboxAnimator.SetBool(ACTIVE, false);
            if (_quests != null)
            {
                _quests.ForEach(q => AmbitionApp.SendMessage(QuestMessages.COMPLETE_QUEST, q));
                _quests.Clear();
            }
        }

        private void Awake()
        {
            _maxAge = 0f;
            foreach (Relevance rel in AmbitionApp.Gossip.Relevances)
            {
                if (rel.Age > _maxAge) _maxAge = (float)rel.Age;
            }

            AmbitionApp.Subscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Subscribe(ParisMessages.LEAVE_LOCATION, HandleLeaveLocation);
            AmbitionApp.Subscribe<GossipVO>(InventoryMessages.PEDDLE_INFLUENCE, HandleTradeGossip);
        }

        private void OnEnable()
        {
            animator.SetBool(ACTIVE, true);
            Pierre.sprite = PierreNeutral;
            Reset();
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Unsubscribe(ParisMessages.LEAVE_LOCATION, HandleLeaveLocation);
            AmbitionApp.Unsubscribe<GossipVO>(InventoryMessages.PEDDLE_INFLUENCE, HandleTradeGossip);
        }

        private void DisplayGossip(GossipVO gossip)
        {
            HideTextBox();
            Gossip = gossip;
        }
        private void Reset() => Gossip = null;
        private void HandleLeaveLocation()
        {
            animator.SetBool(ACTIVE, false);
            HideTextBox();
        }

        IEnumerator StartTransition(float value)
        {
            float SLIDER_TRANSITION1 = 1f / SLIDER_TRANSITION;
            float v0 = FreshnessSlider.value;
            for (float t = 0f; t < SLIDER_TRANSITION; t += Time.deltaTime)
            {
                FreshnessSlider.value = (v0 * (SLIDER_TRANSITION - t) + value * t) * SLIDER_TRANSITION1;
                yield return null;
            }
            FreshnessSlider.value = value;
        }

        private string GetEffectString(GossipVO gossip)
        {
            string phrase = GossipConsts.GOSSIP_LOC + gossip.Tier + ".effect." + (gossip.IsPower ? "power" : "allegiance");
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs["$FACTIONNAME"] = gossip.Faction.ToString().ToLower();
            return AmbitionApp.Localize(phrase, subs);
        }

        private void HandleTradeGossip(GossipVO gossip)
        {
            QuestVO quest = AmbitionApp.Gossip.Quests.Find(q => q.Faction == gossip.Faction);
            if (quest != null)
            {
                if (_quests == null) _quests = new List<QuestVO>() { quest };
                else _quests.Add(quest);
                textboxAnimator.SetBool(ACTIVE, true);
                Pierre.sprite = PierreHappy;
                AmbitionApp.SendMessage(AudioMessages.PLAY, HappySound);
                SpeechBubbleText.text = AmbitionApp.Localize(DialogConsts.REDEEM_QUEST_DIALOG + DialogConsts.BODY);
                StartCoroutine(ShowTextBox());
            }
            Reset();
        }

        IEnumerator ShowTextBox()
        {
            yield return new WaitForSeconds(TEXTBOX_TIMER);
            HideTextBox();
        }
    }
}
