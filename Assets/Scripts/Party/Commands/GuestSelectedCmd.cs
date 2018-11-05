using System;
using Core;
using Util;

namespace Ambition
{
    public class GuestSelectedCmd : ICommand<GuestVO>
    {
        public void Execute(GuestVO guest)
        {
            if (guest.IsLockedIn) return; // Don't bother if the guest is already locked in

            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            RemarkVO remark = model.Remark;
            float levelBonus = (AmbitionApp.GetModel<GameModel>().Level >= 4)
                ? 1.25f
                : 1.0f;
            float ReparteBonus = 1.0f + (model.Repartee ? AmbitionApp.GetModel<PartyModel>().ReparteeBonus : 0f);
            // Determine reaction to remark
            string key = remark.Interest == guest.Like
                        ? PartyConstants.LIKE
                        : remark.Interest == guest.Dislike
                        ? PartyConstants.DISLIKE
                        : PartyConstants.NEUTRAL;

            if (key == PartyConstants.DISLIKE && model.ItemEffect)
            {
                model.ItemEffect = false;
                return;
            }

            // Adjust guest according to configued reaction
            RemarkResult result = partyModel.RemarkResults[key];
            int opinionDelta = (int)(((float)RNG.Generate(result.OpinionMin, result.OpinionMax)) * ReparteBonus * levelBonus);
            guest.Opinion += opinionDelta;
            if (guest.Opinion >= 100) AmbitionApp.SendMessage(PartyMessages.GUEST_CHARMED, guest);
            else if (guest.Opinion <= 0) AmbitionApp.SendMessage(PartyMessages.GUEST_OFFENDED, guest);
            else switch(key)
            {
                case PartyConstants.LIKE:
                    guest.State = GuestState.Interested;
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_POSITIVE, guest);
                    AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                    break;
                case PartyConstants.DISLIKE:
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_NEGATIVE, guest);
                    guest.State = GuestState.Bored;
                    break;
                default:
                    guest.State = GuestState.Interested;
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_NEUTRAL, guest);
                    break;
            }

            // So, there's a potential that the clock won't reset? Deal with that when it's a thing
            AmbitionApp.SendMessage(result.Remarks < 0 ? PartyMessages.BURN_REMARKS : PartyMessages.RESHUFFLE_REMARKS, Math.Abs(result.Remarks));
        }
    }
}
