using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIAnimator : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image _lifeImage;
    [SerializeField] private Image _scoreImage;
    [Header("Animat values")]
    [SerializeField] private float maxScale = 1.25f;
    [SerializeField] private float squeezeScale = 0.95f;
    [SerializeField] private float upDuration = 0.08f;
    [SerializeField] private float downDuration = 0.07f;
    [SerializeField] private float settleDuration = 0.08f;
    [SerializeField] private float shakeStrength = 8f;
    [SerializeField] private float shakeDuration = 0.12f;
    [Header("Score Multipliers")]
    [SerializeField] private float _scoreScaleMultiplier = 0.7f;
    [SerializeField] private float _scoreShakeMultiplier = 0.45f;
    [SerializeField] private float _scoreDurationMultiplier = 0.85f;

    private readonly Dictionary<Image, Vector3> _baseScales = new();
    private readonly Dictionary<Image, Vector2> _baseAnchoredPositions = new();

    private void Awake()
    {
        CacheBaseState(_lifeImage);
        CacheBaseState(_scoreImage);
    }

    public void LifeImageJuiceBumpDecreased() => 
        JuiceBump(_lifeImage);

    public void LifeImageJuiceBumpIncrease() => 
        JuiceBump(_lifeImage);

    public void ScoreImageJuiceBump() => 
        JuiceBump(
            _scoreImage,
            scaleMultiplier: _scoreScaleMultiplier,
            shakeMultiplier: _scoreShakeMultiplier,
            durationMultiplier: _scoreDurationMultiplier);

    public void JuiceBump(
        Image image,
        float scaleMultiplier = 1f,
        float shakeMultiplier = 1f,
        float durationMultiplier = 1f)
    {
        if (image == null) return;

        RectTransform rect = image.rectTransform;
        CacheBaseState(image);

        Vector3 originalScale = _baseScales[image];
        Vector2 originalAnchoredPos = _baseAnchoredPositions[image];

        rect.DOKill();
        rect.localScale = originalScale;
        rect.anchoredPosition = originalAnchoredPos;

        Sequence sequence = DOTween.Sequence();

        float targetScale = 1f + (maxScale - 1f) * scaleMultiplier;
        float targetSqueeze = 1f - (1f - squeezeScale) * scaleMultiplier;

        sequence.Append(rect.DOScale(originalScale * targetScale, upDuration * durationMultiplier).SetEase(Ease.OutQuad));

        sequence.Append(rect.DOScale(originalScale * targetSqueeze, downDuration * durationMultiplier).SetEase(Ease.InOutQuad));

        sequence.Append(rect.DOScale(originalScale, settleDuration * durationMultiplier).SetEase(Ease.OutBack));

        sequence.Join(
            rect.DOShakeAnchorPos(
                shakeDuration * durationMultiplier,
                strength: shakeStrength * shakeMultiplier,
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
        });
    }

    private void CacheBaseState(Image image)
    {
        if (image == null)
            return;

        RectTransform rect = image.rectTransform;
        _baseScales[image] = rect.localScale;
        _baseAnchoredPositions[image] = rect.anchoredPosition;
    }
}
