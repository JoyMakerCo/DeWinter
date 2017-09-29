using System;
using System.Collections.Generic;
using Dialog;
using Core;
using UnityEngine.UI;

namespace Ambition
{
	public class RoomChoiceDialog : MessageViewMediator, Util.IInitializable<RoomVO>, IDisposable
	{
		public Text WorkTheRoomLabel;
		public Button WorkTheRoomBtn;
		public Button PassThroughBtn;

		public void Initialize(RoomVO room)
		{
			LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			int chance = room.MoveThroughChance;
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			// TODO: Implement Item states
			if(inventory.Equipped.TryGetValue("accessory", out accessory)
				&& accessory.Name == "Cane")
	        {
                chance = ((chance < 90) ? (chance + 10) : 100);
	        }

			Dictionary<string, string> substitutions = new Dictionary<string, string>()
			{{"$ROOMNAME", room.Name},
			{"$CHANCE", chance.ToString()}};

			string title = model.GetString("move_through_dialog" + DialogConsts.TITLE, substitutions);
			string body = model.GetString("move_through_dialog" + DialogConsts.BODY, substitutions);
			string ok = model.GetString("move_through_dialog" + DialogConsts.OK, substitutions);
			MessageDialogVO vo = new MessageDialogVO(title, body, ok);
			base.Initialize(vo);
			WorkTheRoomLabel.text = model.GetString("move_through_dialog" + DialogConsts.CANCEL);

			WorkTheRoomBtn.onClick.AddListener(WorkTheRoom);
			PassThroughBtn.onClick.AddListener(PassThrough);
		}

		public void Dispose()
		{
			WorkTheRoomBtn.onClick.RemoveAllListeners();
			PassThroughBtn.onClick.RemoveAllListeners();
		}

		private void WorkTheRoom()
		{
			AmbitionApp.SendMessage(PartyMessages.START_ENCOUNTER);
			Close();
		}

		private void PassThrough()
		{
			AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
			Close();
		}
	}
}
