using UnityEngine;
using DG.Tweening;

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
        // LEFT HAND (đi sang trái rồi về)
        leftHand.DOAnchorPosX(leftHand.anchoredPosition.x - moveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        // RIGHT HAND (đi sang phải rồi về)
        rightHand.DOAnchorPosX(rightHand.anchoredPosition.x + moveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}