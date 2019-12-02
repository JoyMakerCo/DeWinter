using UFlow;

namespace Ambition
{
    public class SendMessageState : UState<string>
    {
        string message;
        public override void SetData(string data) => message = data;
        public override void OnEnterState(string[] args)
        {
            AmbitionApp.SendMessage(message);

            if (args != null) switch (args.Length)
            {
                case 1:
                    AmbitionApp.SendMessage(args[0]);
                    break;
                case 2:
                    AmbitionApp.SendMessage(args[0], args[1]);
                    break;
            }
        }
    }
}
