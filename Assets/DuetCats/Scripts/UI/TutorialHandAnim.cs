using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace DuetCats.Scripts.UI
{
    public class TutorialHandAnim : MonoBehaviour
    {
        public RectTransform leftHand;
        public RectTransform rightHand;

        [Header("Popup")]
        public RectTransform popup;
        public Image popupImage;

        public float moveDistance = 100f;
        public float duration = 0.5f;

        void Start()
        {
            PlayLoop();
            PlayPopup();
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

        void PlayPopup()
        {
            if (popup == null || popupImage == null) return;

            // reset
            popup.localScale = Vector3.one * 0.8f;
            popupImage.color = new Color(1, 1, 1, 0);

            Sequence seq = DOTween.Sequence();

            seq.Append(popup.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            seq.Join(popupImage.DOFade(1f, 0.3f));

            seq.AppendInterval(0.5f);

            seq.Append(popup.DOScale(0.8f, 0.3f));
            seq.Join(popupImage.DOFade(0f, 0.3f));

            seq.SetLoops(-1);
        }
    }
}