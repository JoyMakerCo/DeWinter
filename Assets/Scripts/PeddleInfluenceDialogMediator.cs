using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;

namespace Ambition
{
    public class PeddleInfluenceDialogMediator : DialogView<GossipVO>, ISubmitHandler, IButtonInputHandler
    {
        public const string DIALOG_ID = "PEDDLE_INFLUENCE";
        public const string LOC_ID = "peddle_influence_dialog";

        public Text BodyText;
        public Text IncreaseStatText; //Text for the 'Use it to help this faction' button
        public Text DecreaseStatText; //Text for the 'Use it to harm this faction' button
        private GossipVO _gossip;
        private int _price;
        private int _shift;

        public override void OnOpen(GossipVO gossip)
        {
            GossipModel model = AmbitionApp.Gossip;
            int day = AmbitionApp.Calendar.Day;
            _gossip = gossip;
            _price = model.GetValue(_gossip, day);
            _shift = model.GetShift(_gossip, day);

            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs["$SHIFTAMOUNT"] = AmbitionApp.Localize("shift_degree." + _shift);
            subs["$GOSSIPNAME"] = AmbitionApp.Gossip.GetName(_gossip);
            subs["$GOSSIPPRICE"] = _price.ToString("£### ###");
            subs["$CAUGHTODDS"] = AmbitionApp.Localize("gossip_caught_odds." + AmbitionApp.Gossip.GossipActivity.ToString() );
            subs["$FACTION"] = AmbitionApp.Localize( _gossip.Faction.ToString().ToLower() ); 
            //Performing the substitutions themselves
            BodyText.text = AmbitionApp.Localize(GossipConsts.PEDDLE_INFLUENCE_LOC + DialogConsts.BODY, subs);

            if (gossip.IsPower)
            {
                IncreaseStatText.text = AmbitionApp.Localize(GossipConsts.PEDDLE_INFLUENCE_INC_POWER_LOC, subs)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                DecreaseStatText.text = AmbitionApp.Localize(GossipConsts.PEDDLE_INFLUENCE_DEC_POWER_LOC, subs)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
            }
            else
            {
                IncreaseStatText.text = AmbitionApp.Localize(GossipConsts.PEDDLE_INFLUENCE_INC_ALLEGIANCE_LOC, subs)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                DecreaseStatText.text = AmbitionApp.Localize(GossipConsts.PEDDLE_INFLUENCE_DEC_ALLEGIANCE_LOC, subs)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
            }
        }

        public void Submit() { }
        public void Cancel() => Close();
        public void HandleInput(string id, bool holding) //TODO
        {
            switch (id)
            {
                default:
                    break;
            }
        }

        public void IncreaseFactionStat()
        {
            AmbitionApp.Politics.Factions.TryGetValue(_gossip.Faction, out FactionVO faction);
            if (faction != null)
            {
                CommodityVO reward = new CommodityVO(
                    _gossip.IsPower ? CommodityType.Power : CommodityType.Allegiance,
                    _gossip.Faction.ToString(),
                    _shift);
                AmbitionApp.SendMessage(reward);
                AmbitionApp.SendMessage(InventoryMessages.PEDDLE_INFLUENCE, _gossip);
            }
        }

        public void DecreaseFactionStat()
        {
            AmbitionApp.Politics.Factions.TryGetValue(_gossip.Faction, out FactionVO faction);
            if (faction != null)
            {
                CommodityVO reward = new CommodityVO(
                    _gossip.IsPower ? CommodityType.Power : CommodityType.Allegiance,
                    _gossip.Faction.ToString(),
                    -_shift);
                AmbitionApp.SendMessage(reward);
                AmbitionApp.SendMessage(InventoryMessages.PEDDLE_INFLUENCE, _gossip);
            }
        }
    }
}
