using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class StartPartyBtn : MonoBehaviour
	{
		private Button _button;

		void Awake()
		{
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO outfit;
            inventory.Equipped.TryGetValue(ItemConsts.OUTFIT, out outfit);
			_button = gameObject.GetComponent<Button>();
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIPPED, HandleOutfit);
            HandleOutfit(outfit);
		}

		void OnEnable ()
		{
			_button.onClick.AddListener(OnClick);
		}

		void OnDisable()
		{
            _button.onClick.RemoveAllListeners();
		}

		void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIPPED, HandleOutfit);
            _button.onClick.RemoveAllListeners();
        }

        private void HandleOutfit(ItemVO outfit) => _button.interactable = (outfit is OutfitVO);
		private void OnClick() => AmbitionApp.SendMessage(PartyMessages.START_PARTY);
	}
}
