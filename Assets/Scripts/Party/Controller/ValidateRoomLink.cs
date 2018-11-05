using System;
using Core;
using UFlow;

namespace Ambition
{
    public class ValidateRoomLink : ULink
    {
        public override bool Validate()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            return model.Room != null && !model.Room.Cleared;
        }
    }
}
