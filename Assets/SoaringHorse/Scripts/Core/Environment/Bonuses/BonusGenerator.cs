using UnityEngine;
using Zenject;

public class BonusGenerator : BaseEnvironmentGenerator<Bonus>
{

    [Header("Spawn validation")]
    [SerializeField] private LayerMask _crystalMask;     // слой, на котором наход€тс€ кристаллы (их коллайдеры)
    [SerializeField] private float _checkRadius = 0.35f; // радиус проверки свободного места
    [SerializeField] private int _maxTries = 8;          // попыток подобрать Y без кристаллов

    [Header("Bonus type")]
    [Range(0f, 1f)]
    [SerializeField] private float _luckChance = 0.5f;   // шанс luck (иначе life)

    private Bonus.Factory _factory;

    [Inject]
    private void Construct(GameConfig gameConfig, Bonus.Factory bonusFactory)
    {
        _factory = bonusFactory;

        // маргины те же, что и дл€ кристаллов
        InitCommon(gameConfig.SpawnEnvironmentMargin, gameConfig.DespawnEnvironmentMargin);

        // ѕереименуй пол€ под свой GameConfig (ниже Ч ожидаемые имена)
        // ƒл€ 2D фиксированный Z обычно 0 (можешь вз€ть из конфига, если нужно дл€ sorting)
        InitSpawnParams(
            gameConfig.MinBonusY,      // minY дл€ бонусов
            gameConfig.MaxBonusY,      // maxY дл€ бонусов
            0f,                        // fixedZ (2D)
            gameConfig.BonusSpacing   // шаг по X дл€ бонусов (чтобы не спавнить слишком часто)
        );
    }

    protected override Bonus SpawnEntry(Vector3 localPos)
    {
        // localPos.x задаЄт BaseEnvironmentGenerator, Y подбираем сами так, чтобы не пересекатьс€ с кристаллами
        for (int i = 0; i < _maxTries; i++)
        {
            float y = Random.Range(minY, maxY);
            Vector3 candidateLocal = new Vector3(localPos.x, y, fixedZ);
            Vector3 candidateWorld = LocalToWorld(candidateLocal);

            if (IsFreeFromCrystals(candidateWorld))
            {
                Bonus bonus = _factory.Create(candidateWorld);
                Transform t = bonus.transform;

                t.SetParent(container, true);

                // ¬ыбор типа бонуса (можешь заменить на логику из конфига)
                BonusType type = (Random.value < _luckChance) ? BonusType.luck : BonusType.life;
                bonus.Initialize(type);

                return bonus;
            }
        }

        // не нашли место Ч просто пропускаем этот X (в очередь попадЄт null, базовый класс его выбросит)
        return null;
    }

    private bool IsFreeFromCrystals(Vector2 worldPos)
    {
        // “ребование: у кристаллов должен быть Collider2D на слое _crystalMask
        return Physics2D.OverlapCircle(worldPos, _checkRadius, _crystalMask) == null;
    }

    protected override bool IsEntryMissing(Bonus entry) => !entry;

    protected override float GetEntryWorldX(Bonus entry) =>
        entry ? entry.transform.position.x : float.NegativeInfinity;

    protected override void DespawnEntry(Bonus entry)
    {
        if (entry)
            entry.Despawn(); // предполагаетс€, что BaseRecyclable даЄт Despawn() (как у Crystal)
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // визуализаци€ радиуса проверки на сцене (необ€зательно)
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
#endif

}
