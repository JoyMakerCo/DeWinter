using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public PlayerConfig[] Characters;

        public Image Doll;
        public Text Name;
        public Text Description;
        public GameObject ButtonPrefab;

        private PlayerConfig _selected;
        private float _btnHeight;

        public void Show()
        {
            if (Characters.Length > 1) 
            {
                gameObject.SetActive(true);
                _btnHeight = ButtonPrefab.GetComponent<RectTransform>().rect.height;
                Array.ForEach(Characters, GenerateButton);
            }
            else {
                _selected = Characters[0];
                PickCharacter();
            }
        }

        private void OnDestroy()
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            Array.ForEach(buttons, b => b.onClick.RemoveAllListeners());
        }

        private void GenerateButton(PlayerConfig config)
        {
            GameObject prefab = Instantiate(ButtonPrefab, this.transform, false);
            Vector3 localXform = prefab.transform.localPosition;
            localXform.y = _btnHeight * transform.childCount;
            prefab.transform.localPosition = localXform;

            prefab.GetComponent<Text>().text = config.name;
            prefab.GetComponent<Image>().sprite = config.Portrait;
            prefab.GetComponent<Button>().onClick.AddListener(delegate { SetConfig(config); });
        }

        private void SetConfig(PlayerConfig config)
        {
            _selected = config;
            Name.text = config.name;
            Description.text = config.Description;
            Doll.sprite = config.Doll;
        }

        public void PickCharacter()
        {
            AmbitionApp.Execute<InitGameCmd, PlayerConfig>(_selected);
        }

        public Dictionary<string, string> StockCharacterList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("Karoline", "From Northern Europe, Karoline is bubbly, insecure and prone to drink.");
            result.Add("Félicité", "Born in Haiti and recently arrvied in Paris, Félicité will have to contend both with mundane social challenges, but with contentions surrounding her mixed heritage. Not recommended for new players.");
            result.Add("Donatien", "A mysterious and handsome man, many rumors swirl around Donatien and his past, including several scandalous ones concerning his... choice of pleasures.");
            return result;
        }
    }
}
