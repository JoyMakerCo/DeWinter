using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Dialog;

namespace Ambition
{
    public class ErrorModal : DialogView<ErrorEventArgs>, ISubmitHandler
    {
        private string SENDER = "customer-support@ambition-game.com";
        public Button SubmitBtn;
        public Text ErrorTxt;
        public Text EmailPrompt;

        private ErrorEventArgs _args;

        public override void OnOpen(ErrorEventArgs args)
        {
            _args = args;
            ErrorTxt.text = AmbitionApp.Localize("error." + args.Type.ToString(), args.Substitutions);
        }

        public void Cancel() => Close();
        public void Submit()
        {
            string subject = AmbitionApp.Localize("error." + _args.Type.ToString() + ".subject");
            string body = AmbitionApp.Localize("error." + _args.Type.ToString(), _args.Substitutions);
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs["$E"] = body;
            body = AmbitionApp.Localize("error.message_body", subs) + "\n\nError: " + _args.Type.ToString();
            Application.OpenURL("mailto:" + SENDER + "?subject=" + Escape(subject) + "&body=" + Escape(body));
            Close();
        }

        string Escape(string url) => string.IsNullOrEmpty(url)?"":UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}