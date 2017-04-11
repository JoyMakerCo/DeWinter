using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class GuestViewMediator : MonoBehaviour
	{
		private GuestVO _guest;
		private Image _guestImage;
		private bool _turnTimerActive;
		private GuestSprite _sprite;

public GameObject guest0Visual;
public Text guest0NameText;
public Image guest0GuestImage;
public Text guest0InterestText;
public Scrollbar guest0InterestBar;
public Image guest0InterestBarImage;
public Text guest0OpinionText;
public Scrollbar guest0OpinionBar;
public Image guest0OpinionBarImage;
public Image guest0DispositionIcon;

		void Awake()
		{
			_guestImage = GetComponent<Image>();
			DeWinterApp.Subscribe(PartyMessages.START_TIMERS, HandleStartTimers);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe(PartyMessages.START_TIMERS, HandleStartTimers);
		}

		public GuestSprite Avatar
		{
			set {
				_sprite = value;
				_guest = _guest;
			}
		}

		public GuestVO Guest
		{
			set {
				_guest = value;
				if (_guest != null && _sprite != null)
				{
					_guestImage.sprite = _sprite.GetSpite(_guest.Opinion);
				}
			}
		}

		private void HandleStartTimers()
		{
			StartCoroutine(ConversationStartTimerWait());
		}

	    public IEnumerator ConversationStartTimerWait()
	    {
	        Debug.Log("Ready? Go! Timer Started!");
	        yield return new WaitForSeconds(2.0f);
//	        Destroy(readyGoPanel);
//	        Destroy(readyGoText);
	    }

		void SetUpGuests()
	    {
	        //---- Set Up Guest 0 ----
	        guest0NameText.text = room.Guests[0].Name;
	        guest0GuestImage.sprite = GuestStateSprite(0);
	        guest0DispositionIcon.color = DispositionImageColor(0);
	        if (room.Guests[0].isEnemy)
	        {
	            guest0NameText.color = Color.red;
	            guest0InterestBarImage.color = Color.clear;
	        }
	        else
	        {
	            guest0NameText.color = Color.white;
	            guest0InterestBarImage.color = Color.white;
	        }
	        guestImageList.Add(guest0GuestImage);

	        //---- Set Up Guest 1 ----
	        guest1NameText.text = room.Guests[1].Name; 
	        guest1GuestImage.sprite = GuestStateSprite(1);    
	        guest1DispositionIcon.color = DispositionImageColor(1);
	        if (room.Guests[1].isEnemy)
	        {
	            guest1NameText.color = Color.red;
	            guest1InterestBarImage.color = Color.clear;
	        }
	        else
	        {
	            guest1NameText.color = Color.white;
	            guest1InterestBarImage.color = Color.white;
	        }
	        guestImageList.Add(guest1GuestImage);

	        //---- Set Up Guest 2 ----
	        if (room.Guests.Length > 2)
	        {
	            guest2NameText.text = room.Guests[2].Name;
	            guest2GuestImage.sprite = GuestStateSprite(2);
	            guest2DispositionIcon.color = DispositionImageColor(2);
	            if (room.Guests[2].isEnemy)
	            {
	                guest2NameText.color = Color.red;
	                guest2InterestBarImage.color = Color.clear;
	            }
	            else
	            {
	                guest2NameText.color = Color.white;
	                guest2InterestBarImage.color = Color.white;
	            }
	            guestImageList.Add(guest2GuestImage);
	        } else
	        {
	            guest2NameText.text = "";
	            guest2GuestImage.sprite = null;
	            guest2GuestImage.color = Color.clear;
	            guest2DispositionIcon.color = Color.clear;
	            guest2InterestBar.image.color = Color.clear;
	            guest2InterestBarImage.color = Color.clear;
	            guest2InterestText.color = Color.clear;
	            guest2OpinionText.color = Color.clear;
	            guest2OpinionBarImage.color = Color.clear;
	            guest2OpinionBar.image.color = Color.clear;
	        }

	        //---- Set Up Guest 3 ----
	        if (room.Guests.Length > 3)
	        {
	            guest3NameText.text = room.Guests[3].Name;
	            guest3GuestImage.sprite = GuestStateSprite(3);
	            guest3DispositionIcon.color = DispositionImageColor(3);
	            if (room.Guests[3].isEnemy)
	            {
	                guest3NameText.color = Color.red;
	                guest3InterestBarImage.color = Color.clear;
	            }
	            else
	            {
	                guest3NameText.color = Color.white;
	                guest3InterestBarImage.color = Color.white;
	            }
	            guestImageList.Add(guest3GuestImage);
	        }
	        else
	        {
	            guest3NameText.text = "";
	            guest3GuestImage.sprite = null;
	            guest3GuestImage.color = Color.clear;
	            guest3DispositionIcon.color = Color.clear;
	            guest3InterestBarImage.color = Color.clear;
	            guest3InterestBar.image.color = Color.clear;
	            guest3InterestText.color = Color.clear;
	            guest3OpinionText.color = Color.clear;
	            guest3OpinionBarImage.color = Color.clear;
	            guest3OpinionBar.image.color = Color.clear;
	        }
	    }

		void InterestTimersDisplayCheck()
	    {
	        //------------- Guest 0 -------------
	        if (room.Guests[0].lockedInState != LockedInState.Interested || room.Guests[0].isEnemy)
	        {
	            guest0InterestBar.image.color = Color.clear;
	            guest0InterestBarImage.color = Color.clear;
	        }
	        else
	        {
	            guest0InterestBar.image.color = Color.white;
	            guest0InterestBarImage.color = Color.white;
	        }

	        //------------- Guest 1 -------------
	        if (room.Guests[1].lockedInState != LockedInState.Interested || room.Guests[1].isEnemy)
	        {
	            guest1InterestBar.image.color = Color.clear;
	            guest1InterestBarImage.color = Color.clear;
	        }
	        else
	        {
	            guest1InterestBar.image.color = Color.white;
	            guest1InterestBarImage.color = Color.white;
	        }

	        //------------- Guest 2 -------------
	        //There might not be 3 Guests or more, so this check is to make sure nothing breaks
	        if (room.Guests.Length > 2)
	        {
	            if (room.Guests[2].lockedInState != LockedInState.Interested || room.Guests[2].isEnemy)
	            {
	                guest2InterestBar.image.color = Color.clear;
	                guest2InterestBarImage.color = Color.clear;
	            }
	            else
	            {
	                guest2InterestBar.image.color = Color.white;
	                guest2InterestBarImage.color = Color.white;
	            }
	        }

	        //------------- Guest 3 -------------
	        //There might not be 4 Guests or more, so this check is to make sure nothing breaks
	        if (room.Guests.Length > 3)
	        {
	            if (room.Guests[3].lockedInState != LockedInState.Interested || room.Guests[3].isEnemy)
	            {
	                guest3InterestBar.image.color = Color.clear;
	                guest3InterestBarImage.color = Color.clear;
	            }
	            else
	            {
	                guest3InterestBar.image.color = Color.white;
	                guest3InterestBarImage.color = Color.white;
	            }
	        }
	    }
	}
}