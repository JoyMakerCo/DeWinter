using System;
using UnityEngine;

namespace Ambition
{
	public class GuestViewsMediator : MonoBehaviour
	{
		public GuestView[] GuestViews;

		private GuestVO[] _guests;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
		private PartyArtLibrary _artLibrary;

		void Awake()
		{
			_artLibrary = GetComponent<PartyArtLibrary>();
			
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
			AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargets);
		}

		void Start()
		{
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			AmbitionApp.Execute<GenerateGuestsCmd, RoomVO>(room);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
			AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargets);
		}

		private void HandleRemark(RemarkVO remark)
		{
			_remark = remark;
			Deselect();
		}

		private void Deselect()
		{
			Color c = _remark != null ? Color.grey : Color.white;
			foreach (GuestView view in GuestViews)
			{	
				view.GuestImage.color = c;
			}
		}

		private void HandleTargets(GuestVO [] guests)
		{
			if (guests != null && guests.Length > 0 && _remark != null)
			{
				GuestVO guest;
				Color c;
				for (int i=_guests.Length-1; i >= 0; i--)
				{
					guest = _guests[i];
					if (Array.IndexOf(guests, guest) < 0) c = Color.gray;
					else if (_isIntoxicated) c = Color.white;
					else if (_remark.Interest == guest.Like) c = Color.green;
					else if (_remark.Interest == guest.Disike) c = Color.red;
					else c = Color.white;

					GuestViews[i].GuestImage.color = c;
				}
			}
			else foreach (GuestView view in GuestViews)
			{
				view.GuestImage.color = Color.white;
			}
		}

		private void HandleGuests(GuestVO [] guests)
		{
			GuestView guestView;
			GuestVO guest;
			EnemyVO enemy;
			bool isEnemy;
			GuestSprite [] sprites;

			_guests = guests;

			for (int i=GuestViews.Length-1; i>=0; i--)
			{
				guestView = GuestViews[i];
				guestView.Enabled = (i < guests.Length);

				// Update the art to show the current guest state`
				if (guestView.Enabled)
				{
					guest = guests[i];
					enemy = guest as EnemyVO;
					isEnemy = (enemy != null);

					guestView.NameText.text = guest.Name;
					guestView.NameText.color = isEnemy ? Color.red : Color.white;

					guestView.OpinionIndicator.gameObject.SetActive(!guest.IsLockedIn);
					guestView.OpinionIndicator.fillAmount = 0.1f*(float)guest.Interest;

					sprites = guest.IsFemale
						? _artLibrary.FemaleGuestSprites
						: _artLibrary.MaleGuestSprites;

					if (guest.Variant < 0)
					{
						// Random guest variation is driven by art,
						// as opposed to explicit values
						guest.Variant = (new System.Random()).Next(sprites.Length);
					}
					if (_isIntoxicated)
					{
						guestView.GuestImage.sprite = sprites[guest.Variant].BoredSprite;
					}
					else
					{
						guestView.GuestImage.sprite = sprites[guest.Variant].GetSprite(guest);
					}

					InterestMap interest = Array.Find(_artLibrary.InterestSprites, n=>n.Interest == guest.Like);
					if (!default(InterestMap).Equals(interest))
						guestView.InterestIcon.sprite = interest.Sprite;
				}
			}
		}

		private void HandleIntoxication(int tox)
		{
			_isIntoxicated = (tox >= 50);
		}
	}
}
