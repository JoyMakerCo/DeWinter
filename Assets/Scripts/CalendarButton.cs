using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalendarButton : MonoBehaviour {

    public int rowID; //Button Row
    public int columnID; // Button Column?
    public GameObject screenFader; // It's for the RSVP pop-up
    public Image currentDayOutline;
    public Text date;
    public Image pastDayXImage;
    //Party Indicators
    public Image party1FactionImage;
    public Image party1PositiveReplyImage;
    public Image party1NegativeReplyImage;
    public Image party2FactionImage;
    public Image party2PositiveReplyImage;
    public Image party2NegativeReplyImage;

    //Faction Icon Sprites
    public Sprite crownFactionSprite;
    public Sprite churchFactionSprite;
    public Sprite militaryFactionSprite;
    public Sprite bourgeoisieFactionSprite;
    public Sprite thirdEstateFactionSprite;

    Dictionary<string, Sprite> factionSprites = new Dictionary<string, Sprite>();

    private Image myBlockImage;
    private Color defaultColor;
    private Day buttonDay;

    int startPositionPlusDays;

    void Start()
    {
        myBlockImage = this.GetComponent<Image>();
        defaultColor = myBlockImage.color;
        StockSpriteDictionary();
    }

    void Update ()
    {
        UpdateButtonDisplay();
    }

    void UpdateButtonDisplay()
    {
        SetButtonDay();
        if (buttonDay != null) //If there's no error in the date or Day
        {
            date.text = buttonDay.displayDay.ToString();
            //Is there a party? If so, then display it. If not, then no icons
            if (buttonDay.party1.invited)
            {
                party1FactionImage.color = Color.white;
                party1FactionImage.sprite = factionSprites[buttonDay.party1.faction.Name()];         
            } else
            {
                party1FactionImage.color = Color.clear;
            }
            if (buttonDay.party2.invited)
            {
                party2FactionImage.color = Color.white;
                party2FactionImage.sprite = factionSprites[buttonDay.party2.faction.Name()];
            }
            else
            {
                party2FactionImage.color = Color.clear;
            }
            //What's the state of the Party RSVP?
            //---- Party 1 ----
            if (buttonDay.party1.RSVP == 1)
            {
                party1PositiveReplyImage.color = Color.white;
                party1NegativeReplyImage.color = Color.clear;
            }
            else if (buttonDay.party1.RSVP == -1)
            {
                party1PositiveReplyImage.color = Color.clear;
                party1NegativeReplyImage.color = Color.white;
            }
            else
            {
                party1PositiveReplyImage.color = Color.clear;
                party1NegativeReplyImage.color = Color.clear;
            }
            //---- Party 2 ----
            if (buttonDay.party2.RSVP == 1)
            {
                party2PositiveReplyImage.color = Color.white;
                party2NegativeReplyImage.color = Color.clear;
            }
            else if (buttonDay.party2.RSVP == -1)
            {
                party2PositiveReplyImage.color = Color.clear;
                party2NegativeReplyImage.color = Color.white;
            }
            else
            {
                party2PositiveReplyImage.color = Color.clear;
                party2NegativeReplyImage.color = Color.clear;
            }
            // Is this Day in the display month? If not, then gray it out
            if (buttonDay.month == GameData.displayMonthInt)
            {
                myBlockImage.color = defaultColor;
            } else
            {
                myBlockImage.color = Color.gray;
            }
            // Is this day today? If so, then Outline it
            if (GameData.currentMonth == buttonDay.month && GameData.currentDay == buttonDay.day)
            {
                currentDayOutline.color = Color.white;
                this.transform.SetAsLastSibling();
                pastDayXImage.color = Color.clear;
            } else if (GameData.currentMonth > buttonDay.month || (GameData.currentMonth == buttonDay.month && GameData.currentDay > buttonDay.day))
            {
                currentDayOutline.color = Color.clear;
                pastDayXImage.color = Color.white;
            }  else
            {
                currentDayOutline.color = Color.clear;
                pastDayXImage.color = Color.clear;
            }
        } else
        {
            date.text = "000";
        }
    }

    public void RSVP()
    {
        //Pop Up Window      
        if (GameData.calendar.monthList[GameData.displayMonthInt].dayList[rowID, columnID] != null)
        {
            if (buttonDay.party1.invited && !buttonDay.party2.invited)
            {
                if (buttonDay.party1.RSVP != 1)
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", buttonDay.party1);
                }
                else
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] = buttonDay.party1;
                    if (GameData.currentMonth == buttonDay.month && GameData.currentDay == buttonDay.day)
                    {
                        objectStorage[1] = true;
                    }
                    else
                    {
                        objectStorage[1] = false;
                    }
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                }
            }
            else if (!buttonDay.party1.invited && buttonDay.party2.invited)
            {
                if (buttonDay.party2.RSVP != 1)
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", buttonDay.party2);
                }
                else
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] = buttonDay.party2;
                    if (GameData.currentMonth == buttonDay.month && GameData.currentDay == buttonDay.day)
                    {
                        objectStorage[1] = true;
                    }
                    else
                    {
                        objectStorage[1] = false;
                    }
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                }
            }
            else if (buttonDay.party1.invited && buttonDay.party2.invited)
            {
                screenFader.gameObject.SendMessage("CreateTwoPartyChoicePopUp", buttonDay);
            }
            else
            {
                Debug.Log("No Party On This Day :(");
            }
        }
    }

    //TODO Make the month of April/March display properly. Currently, if you scroll the Calendar past April the days start getting positioned wrong, with March's days coming after... it's own days. It's very strange.
    void SetButtonDay()
    {
        if (GameData.calendar.monthList[GameData.displayMonthInt].dayList[rowID, columnID] != null)
        {
            buttonDay = GameData.calendar.monthList[GameData.displayMonthInt].dayList[rowID, columnID];
        }
        else if (rowID < 2)
        {
            buttonDay = GameData.calendar.monthList[GameData.displayMonthInt - 1].dayList[(rowID + GameData.calendar.monthList[GameData.displayMonthInt - 1].weeks), columnID];
        }
        else
        {
            //Something is going wrong here with April, hand math it out?
            buttonDay = GameData.calendar.monthList[GameData.displayMonthInt + 1].dayList[(rowID - GameData.calendar.monthList[GameData.displayMonthInt + 1].weeks), columnID];
        }
    }

    void StockSpriteDictionary()
    {
        factionSprites.Add("Crown", crownFactionSprite);
        factionSprites.Add("Church", churchFactionSprite);
        factionSprites.Add("Military", militaryFactionSprite);
        factionSprites.Add("Bourgeoisie", bourgeoisieFactionSprite);
        factionSprites.Add("Third Estate", thirdEstateFactionSprite);
    }
}
