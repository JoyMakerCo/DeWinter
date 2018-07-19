using System;
using UFlow;

namespace Ambition
{
    public class AmbitionMessageState : UState<string>
    {
        private string _messageID;

        public override void SetData(string data)
        {
            _messageID = data;
        }

        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(_messageID);
        }
    }
}
