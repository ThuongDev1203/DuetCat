using DG.Tweening;
using UnityEngine;

namespace DuetCats.Scripts.UI
{
    public class FingerTap : MonoBehaviour
    {
        public RectTransform finger;
        public RectTransform leftButton;
        public RectTransform rightButton;

        public float moveTime = 0.6f;
        public float delayTime = 0.3f;

        void Start()
        {
            PlayLoop();
        }

        void PlayLoop()
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(finger.DOAnchorPos(leftButton.anchoredPosition, moveTime).SetEase(Ease.InOutSine));
            seq.AppendInterval(delayTime);
            seq.AppendCallback(() => TapEffect(leftButton));

            seq.AppendInterval(0.4f);

            seq.Append(finger.DOAnchorPos(rightButton.anchoredPosition, moveTime).SetEase(Ease.InOutSine));
            seq.AppendInterval(delayTime);
            seq.AppendCallback(() => TapEffect(rightButton));

            seq.AppendInterval(0.5f);

            seq.SetLoops(-1);
        }

        void TapEffect(RectTransform targetButton)
        {
            Sequence tap = DOTween.Sequence();
            tap.Append(finger.DOScale(0.85f, 0.12f));
            tap.AppendInterval(0.08f);
            tap.Append(finger.DOScale(1f, 0.2f).SetEase(Ease.OutBack));

            targetButton.DOScale(1.12f, 0.2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}