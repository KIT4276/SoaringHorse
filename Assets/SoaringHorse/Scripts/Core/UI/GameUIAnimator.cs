using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIAnimator : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image _lifeImage;
    [SerializeField] private Image _scoreImage;
    [Header("Colors")]
    [SerializeField] private Color _lifeIncrease = Color.green;
    [SerializeField] private Color _lifeDecreased = Color.red;
    [SerializeField] private Color _scoreIncrease = Color.blue;
    [SerializeField]    private Color _originalColor = Color.white;
    [Header("Animat values")]
    [SerializeField] private float maxScale = 1.25f;
    [SerializeField] private float squeezeScale = 0.95f;
    [SerializeField] private float upDuration = 0.08f;
    [SerializeField] private float downDuration = 0.07f;
    [SerializeField] private float settleDuration = 0.08f;
    [SerializeField] private float shakeStrength = 8f;
    [SerializeField] private float shakeDuration = 0.12f;


    public void LifeImageJuiceBumpDecreased() => 
        JuiceBump(_lifeImage, _lifeDecreased);

    public void LifeImageJuiceBumpIncrease() => 
        JuiceBump(_lifeImage, _lifeIncrease);

    public void ScoreImageJuiceBump() => 
        JuiceBump(_scoreImage, _scoreIncrease);

    public void JuiceBump(Image image, Color bumpColor)
    {
        if (image == null) return;

        RectTransform rect = image.rectTransform;

        rect.DOKill();
        image.DOKill();

        Vector3 originalScale = rect.localScale;
        Vector2 originalAnchoredPos = rect.anchoredPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(rect.DOScale(originalScale * maxScale, upDuration).SetEase(Ease.OutQuad));
        sequence.Join(image.DOColor(bumpColor, upDuration * 0.6f));

        sequence.Append(rect.DOScale(originalScale * squeezeScale, downDuration).SetEase(Ease.InOutQuad));

        sequence.Append(rect.DOScale(originalScale, settleDuration).SetEase(Ease.OutBack));
        sequence.Join(image.DOColor(_originalColor, downDuration + settleDuration));

        sequence.Join(
            rect.DOShakeAnchorPos(
                shakeDuration,
                strength: shakeStrength,
                vibrato: 12,
                randomness: 90,
                snapping: false,
                fadeOut: true));

        sequence.OnKill(() =>
        {
            if (rect != null)
            {
                rect.localScale = originalScale;
                rect.anchoredPosition = originalAnchoredPos;
            }

            if (image != null)
                image.color = _originalColor;
        });
    }
}