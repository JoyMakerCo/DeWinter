using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarButton : MonoBehaviour {

    public int rowID; //Button Row
    public int columnID; // Button Column?
    public GameObject screenFader; // It's for the RSVP pop-up

    private Text myText;
    private Text myDate;
    private Image myBlockImage;
    private Image myXImage;
    private Image myCircleImage;
    private Image mySlashImage;
    private Outline myOutline;
    private Color defaultColor;
    private Day buttonDay;

    int startPositionPlusDays;

    void Start()
    {
        myText = this.transform.Find("Text").GetComponent<Text>();
        myDate = this.transform.Find("DateText").GetComponent<Text>();
        myBlockImage = this.GetComponent<Image>();
        myXImage = this.transform.Find("XImage").GetComponent<Image>();
        myCircleImage = this.transform.Find("CircleImage").GetComponent<Image>();
        mySlashImage = this.transform.Find("SlashImage").GetComponent<Image>();
       // myOutline = this.GetComponent<Outline>();
        defaultColor = myBlockImage.color;
    }

    void Update ()
    {
        UpdateButtonDisplay();
    }

    void UpdateButtonDisplay()
    {
        SetButtonDay();
        if (buttonDay != null)
        {
            //Is there a party? If so, then display it. If not, then blank text
            if (buttonDay.party1.invited && !buttonDay.party2.invited)
            {
                myDate.text = buttonDay.displayDay.ToString();
                myText.text = buttonDay.party1.SizeString() + " " + buttonDay.party1.faction + " Party";
            } else if (!buttonDay.party1.invited && buttonDay.party2.invited)
            {
                myDate.text = buttonDay.displayDay.ToString();
                myText.text = buttonDay.party2.SizeString() + " " + buttonDay.party2.faction + " Party";
            } else if (buttonDay.party1.invited && buttonDay.party2.invited)
            {
                myDate.text = buttonDay.displayDay.ToString();
                myText.text = "Two Parties";
            } else
            {
                myDate.text = buttonDay.displayDay.ToString();
                myText.text = "";
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
               // myOutline.effectColor = Color.black;
                myXImage.color = Color.clear;
            } else if (GameData.currentMonth > buttonDay.month || (GameData.currentMonth == buttonDay.month && GameData.currentDay > buttonDay.day))
            {
             //   myOutline.effectColor = Color.clear;
                myXImage.color = Color.white;
            }  else
            {
             //   myOutline.effectColor = Color.clear;
                myXImage.color = Color.clear;
            }
            //What's the state of the Party RSVP?
            if(buttonDay.party1.RSVP == 1)
            {
                myCircleImage.color = Color.white;
                mySlashImage.color = Color.clear;
            } else if (buttonDay.party1.RSVP == -1)
            {
                myCircleImage.color = Color.clear;
                mySlashImage.color = Color.white;
            } else
            {
                myCircleImage.color = Color.clear;
                mySlashImage.color = Color.clear;
            }
        } else
        {
            myDate.text = "000";
            myText.text = "Error: Null Day";
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
}
