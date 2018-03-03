using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Ambition;

public class EnemyButton : MonoBehaviour {

    public EnemyVO enemy;
    Text nameText;
    Image enemyThumbnail;
    private Outline outline; // This is for highlighting buttons

    public Sprite femaleImage0;
    public Sprite femaleImage1;
    public Sprite femaleImage2;
    public Sprite femaleImage3;

    public Sprite maleImage0;
    public Sprite maleImage1;
    public Sprite maleImage2;
    public Sprite maleImage3;
    public Sprite maleImage4;

    List<Sprite> femaleSpriteList = new List<Sprite>();
    List<Sprite> maleSpriteList = new List<Sprite>();

    EnemyList enemyList;

    void Start()
    {
        StockGuestImageLists();
        nameText = this.transform.Find("NameText").GetComponent<Text>();
        enemyThumbnail = this.transform.Find("EnemyThumbnail").GetComponent<Image>();
        outline = this.GetComponent<Outline>();
        enemyList = this.transform.parent.GetComponent<EnemyList>();
    }

    void Update()
    {
        if(enemy != null)
        {
            DisplayEnemy(enemy);
        }
        if (enemyList.selectedEnemy == enemy)
        {
            outline.effectColor = Color.yellow;
        }
        else
        {
            outline.effectColor = Color.clear;
        }
    }

    public void DisplayEnemy(EnemyVO e)
    {
        if (e != null)
        {
            nameText.text = e.DisplayName;
            if (e.Gender == Gender.Female)
            {
                enemyThumbnail.sprite = femaleSpriteList[e.imageInt];
            } else
            {
                enemyThumbnail.sprite = maleSpriteList[e.imageInt];
            }
        }
    }

    public void SetEnemy()
    {
        Debug.Log("Selected Enemy: " + enemy.Name);
        enemyList.selectedEnemy = enemy;
    }

    void StockGuestImageLists()
    {
        femaleSpriteList.Add(femaleImage0);
        femaleSpriteList.Add(femaleImage1);
        femaleSpriteList.Add(femaleImage2);
        femaleSpriteList.Add(femaleImage3);

        maleSpriteList.Add(maleImage0);
        maleSpriteList.Add(maleImage1);
        maleSpriteList.Add(maleImage2);
        maleSpriteList.Add(maleImage3);
        maleSpriteList.Add(maleImage4);
    }
}
