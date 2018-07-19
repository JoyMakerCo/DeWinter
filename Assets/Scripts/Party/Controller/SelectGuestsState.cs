using System;
using UFlow;

namespace Ambition
{
    public class SelectGuestsState : UState
    {
        override public void OnEnterState()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			RemarkVO remark = model.Remark;
			float levelBonus = (AmbitionApp.GetModel<GameModel>().Level >= 4)
				? 1.25f
				: 1.0f;
            float ReparteBonus = 1.0f + (model.Repartee ? AmbitionApp.GetModel<PartyModel>().ReparteeBonus : 0f);

            model.Remarks.Remove(model.Remark);
            model.Remark = null;

            foreach (GuestVO guest in model.Guests)
			{
				if (!guest.IsLockedIn)
				{
                    if (remark != null)
                    {
                        //Do they like the Tone?
                        if (guest.Like == remark.Interest) //They like the tone
                        {
                            guest.Opinion += (int)(Util.RNG.Generate(25, 36) * ReparteBonus * levelBonus);
                            AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
                            model.Confidence += 5;
                        }
                        else if (guest.Dislike == remark.Interest) //They dislike the tone
                        {
                            // TODO: All of this needs to be affected by the passive bonus system
                            if (!model.ItemEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
                            {
                                guest.Opinion -= (int)(Util.RNG.Generate(11, 17) * ReparteBonus * levelBonus);
                                model.Confidence -= 10;
                            }
                            model.ItemEffect = false;
                        }
                        else //Neutral Tone
                        {
                            guest.Opinion += (int)(Util.RNG.Generate(12, 18) * ReparteBonus * levelBonus);
                        }
                    }

                    if (guest.Opinion >= 100)
                    {
                        guest.Opinion = 100;
                        guest.State = GuestState.Charmed;
                        AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
                    }
                    else if (guest.Opinion <= 0)
                    {
                        guest.Opinion = 0;
                        guest.State = GuestState.PutOff;
                        // TODO: Configure the spite bonus
                        if (guest is EnemyVO) model.Confidence += 10;
                        else model.Confidence -= 30;
                    }
				}
                if (guest.Interest <= 0)
                {
                    guest.Interest = 0;
                    guest.State = GuestState.Bored;
                }
            }
        }
    }
}
