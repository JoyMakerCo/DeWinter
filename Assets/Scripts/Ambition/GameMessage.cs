using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class GameMessage : MonoBehaviour
	{
        public void SendGameMessage(string MessageID) => AmbitionApp.SendMessage(MessageID);
        public void SendGameMessage(string MessageID, string str) => AmbitionApp.SendMessage(MessageID, str);
        public void SendGameMessage(string MessageID, int num) => AmbitionApp.SendMessage(MessageID, num);
    }
}
