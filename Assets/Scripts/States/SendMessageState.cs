using UFlow;

namespace Ambition
{
    public class SendMessageState : UState, Util.IInitializable<string>
    {
        string message;
        public void Initialize(string data) => message = data;
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(message);

/*            switch (message)
            {
                case 1:
                    AmbitionApp.SendMessage(args[0]);
                    break;
                case 2:
                    AmbitionApp.SendMessage(args[0], args[1]);
                    break;
            }
            */
        }
    }
}
