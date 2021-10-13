using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class BarkViewMediator : MonoBehaviour
    {
        private const string FACTION = "$FACTIONNAME";
        private const string NAME = "$NAME";

        public BarkView Bark;
        public BarkConfig[] Barks;
        public SpriteConfig Icons;
        public float SpawnDelay = .5f;

        private List<CommodityVO> _queue = new List<CommodityVO>();

        private void Start()
        {
            AmbitionApp.Subscribe<CommodityVO>(HandleReward);
            AmbitionApp.Subscribe<CommodityVO[]>(HandleRewards);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            AmbitionApp.Unsubscribe<CommodityVO>(HandleReward);
            AmbitionApp.Unsubscribe<CommodityVO[]>(HandleRewards);
        }

        private void HandleReward(CommodityVO reward)
        {
            _queue.Add(reward);
            if (_queue.Count == 1)
            {
                StartCoroutine(StartRewards());
            }
        }
        private void HandleRewards(CommodityVO[] rewards)
        {
            _queue.AddRange(rewards);
            if (_queue.Count == rewards.Length)
            {
                StartCoroutine(StartRewards());
            }
        }

        IEnumerator StartRewards()
        {
            BarkConfig config;
            GameObject obj;
            BarkView bark;
            Dictionary<string, string> subs = new Dictionary<string, string>();
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            Sprite icon;
            CommodityVO reward;
            while (_queue.Count > 0)
            {
                reward = _queue[0];
                _queue.RemoveAt(0);
                if (reward.Value != 0 && Array.Exists(Barks, b => b.Type == reward.Type))
                {
                    if (!string.IsNullOrEmpty(reward.ID))
                    {
                        CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(reward.ID);
                        subs[NAME] = AmbitionApp.Localization.GetFormalName(model.GetCharacter(reward.ID), reward.ID);
                        subs[FACTION] = AmbitionApp.Localize(reward.ID.ToLower());
                        icon = Icons.GetSprite(character != null
                            ? ("seal." + character.Faction.ToString().ToLower())
                            : ("seal.none"));
                    }
                    else icon = null;
                    config = Array.Find(Barks, b => b.Type == reward.Type);
                    obj = Instantiate<GameObject>(Bark.gameObject, Bark.transform.parent, true);
                    bark = obj.GetComponent<BarkView>();
                    if (reward.Value > 0)
                    {
                        bark.SetBark(AmbitionApp.Localize(config.GainLocalization, subs), config.GainColor, icon);
                    }
                    else if (reward.Value < 0)
                    {
                        bark.SetBark(AmbitionApp.Localize(config.LossLocalization, subs), config.LossColor, icon);
                    }
                    bark.gameObject.SetActive(true);
                    yield return new WaitForSeconds(SpawnDelay);
                }
            }
        }

        [Serializable]
        public struct BarkConfig
        {
            public CommodityType Type;
            public string GainLocalization;
            public string LossLocalization;
            public Color GainColor;
            public Color LossColor;
        }
    }
}
