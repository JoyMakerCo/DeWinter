using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UFlow;

namespace Ambition
{
    public class ResourceTutorial : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public string TutorialID;
        public CommodityType Reward;
        public GameObject Prefab;
        public CanvasGroup Canvas;
        public float DisplayDuration;
        public float FadeDuration;
        public float Delay = 0;
        public string TooltipLoc;
        public bool OnGain = true;
        public bool OnLoss = true;

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            AmbitionApp.Game.Tutorials.Remove(TutorialID);
            GameObject.Destroy(gameObject);
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            StopAllCoroutines();
            Canvas.alpha = 1;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            StartCoroutine(ShowTooltip(FadeDuration));
        }

        private void OnEnable()
        {
            if (!AmbitionApp.Game.Tutorials.Contains(TutorialID))
            {
                GameObject.Destroy(gameObject);
            }
            else if (OnGain || OnLoss)
            {
                AmbitionApp.Subscribe<CommodityVO>(HandleReward);
                AmbitionApp.Subscribe<CommodityVO[]>(HandleRewards);
            }
            else HandleReward(new CommodityVO(Reward));
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<float>(GameMessages.FADE_OUT, HandleFade);
            AmbitionApp.Unsubscribe<CommodityVO>(HandleReward);
            AmbitionApp.Unsubscribe<CommodityVO[]>(HandleRewards);
        }

        private void HandleFade(float time)
        {
            StopAllCoroutines();
            AmbitionApp.Game.Tutorials.Remove(TutorialID);
            Destroy(gameObject);
        }

        private void HandleReward(CommodityVO reward)
        {
            if ((reward.Type == Reward)
                && ((OnGain && reward.Value > 0) || (OnLoss && reward.Value < 0) || (!OnLoss && !OnGain)))
            {
                AmbitionApp.Unsubscribe<CommodityVO>(HandleReward);
                AmbitionApp.Unsubscribe<CommodityVO[]>(HandleRewards);
                AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFade);
                StartCoroutine(ShowTooltip(DisplayDuration));
            }
        }

        private void HandleRewards(CommodityVO[] rewards)
        {
            Array.ForEach(rewards, HandleReward);
        }

        IEnumerator ShowTooltip(float duration)
        {
            for (float t = 0; t < Delay; t += Time.deltaTime)
                yield return null;
            GameObject obj = Instantiate<GameObject>(Prefab, this.transform, false);
            ResourceTooltipLoc tutorialTxt = obj.GetComponent<ResourceTooltipLoc>();
            if (tutorialTxt != null) tutorialTxt.TutorialText.text = AmbitionApp.Localize(TooltipLoc);
            for (float t = 0; t < duration; t += Time.deltaTime)
                yield return null;
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            float T1 = 1f / FadeDuration;
            for (float t = FadeDuration; t > 0; t -= Time.deltaTime)
            {
                Canvas.alpha = t * T1;
                yield return null;
            }
            AmbitionApp.Game.Tutorials.Remove(TutorialID);
            Destroy(gameObject);
        }
    }
}
