using System;
using Core;
using Util;

namespace Ambition
{
    public class EnemyAttackCmd : ICommand<EnemyVO>
    {
        public void Execute(EnemyVO guest)
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
            guest.Opinion += (int)(((float)RNG.Generate(result.OpinionMin, result.OpinionMax)) * ReparteBonus * levelBonus);
            if (guest.Opinion >= 100)
            {
                guest.Opinion = 100;
                guest.State = GuestState.Charmed;
                AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                AmbitionApp.SendMessage(PartyMessages.GUEST_CHARMED, guest);
            }
            else if (guest.Opinion <= 0)
            {
                guest.Opinion = 0;
                guest.State = GuestState.PutOff;
                AmbitionApp.SendMessage(PartyMessages.GUEST_OFFENDED, guest);
            }
            else switch (key)
            {
                case PartyConstants.LIKE:
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_POSITIVE, guest);
                    break;
                case PartyConstants.DISLIKE:
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_NEGATIVE, guest);
                    AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                    break;
                default:
                    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_NEUTRAL, guest);
                    break;
            }
            // So, there's a potential that the clock won't reset? Deal with that when it's a thing
        }
    }
}