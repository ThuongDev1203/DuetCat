using UnityEngine;
using DG.Tweening;

namespace DuetCats.Scripts.UI
{
    public class TutorialHandAnim : MonoBehaviour
    {
        public RectTransform leftHand;
        public RectTransform rightHand;

        public float moveDistance = 100f;
        public float duration = 0.5f;

        void Start()
        {
            PlayLoop();
        }

        void PlayLoop()
        {
            leftHand.DOAnchorPosX(leftHand.anchoredPosition.x - moveDistance, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            rightHand.DOAnchorPosX(rightHand.anchoredPosition.x + moveDistance, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}