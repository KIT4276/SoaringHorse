using System.Collections;
using UnityEngine;
using Zenject;

public class Bonus : BaseRecyclable
{
    [SerializeField] private SpriteRenderer _image;
    [Space]
    [SerializeField] private Sprite _lifeBonusSprite;
    [SerializeField] private Sprite _luckBonusSprite;
    [Space]
    [SerializeField] private int _value = 1;

    private bool _inited = false;
    private Coroutine _waitInitRoutine;
    private LiveSystem _liveSystem;
    private ScoreSystem _scoreSystem;

    public BonusType BonusType { get; private set; }

    [Inject]
    private void Construct(LiveSystem liveSystem, ScoreSystem scoreSystem)
    {
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
    }

    public void Initialize(BonusType bonusType)
    {
        BonusType = bonusType;
        _inited = true;
    }
    public  void RewardedDespawn()
    {
        switch (BonusType)
        {
            case BonusType.luck:
                _scoreSystem.ChangeScore((float)_value);
                break;
            case BonusType.life:
                _liveSystem.AddLives(_value);
                break;
        }
        Despawn();
    }

    protected override void Activate()
    {
        if (_inited)
        {
            ApplySprite();
            return;
        }

        if (_waitInitRoutine == null)
            _waitInitRoutine = StartCoroutine(WaitForInitAndApply());
    }

    private void ApplySprite()
    {
        
        switch (BonusType)
        {
            case BonusType.luck:
                _image.sprite = _luckBonusSprite;
                break;
            case BonusType.life:
                _image.sprite = _lifeBonusSprite;
                break;
        }
    }

    private IEnumerator WaitForInitAndApply()
    {
        yield return new WaitUntil(() => _inited);

        _waitInitRoutine = null;
        ApplySprite();
    }

    public override void OnDespawned()
    {

        if (_waitInitRoutine != null)
        {
            StopCoroutine(_waitInitRoutine);
            _waitInitRoutine = null;
        }

        _inited = false;
    }

    public class Factory : PlaceholderFactory<Vector3, Bonus> { }

    public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Bonus> { }
}

public enum BonusType
{
    luck,
    life,
}
