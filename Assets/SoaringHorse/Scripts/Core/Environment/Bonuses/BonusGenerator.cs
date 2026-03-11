using UnityEngine;
using Zenject;

public class BonusGenerator : BaseEnvironmentGenerator<Bonus>
{

    [Header("Spawn validation")]
    [SerializeField] private LayerMask _crystalMask;     // слой, на котором находятся кристаллы (их коллайдеры)
    [SerializeField] private float _checkRadius = 0.35f; // радиус проверки свободного места
    [SerializeField] private int _maxTries = 8;          // попыток подобрать Y без кристаллов

    private float _luckChance = 0.5f;   // шанс luck (иначе life)

    private Bonus.Factory _factory;

    [Inject]
    private void Construct(EnvironmentConfig envConfig, BonusConfig bonusConfig, Bonus.Factory bonusFactory)
    {
        _factory = bonusFactory;
        _luckChance = bonusConfig.LuckChance;
        InitCommon(envConfig.SpawnEnvironmentMargin/2, envConfig.DespawnEnvironmentMargin/2);

        InitSpawnParams(
            bonusConfig.MinBonusY,     
            bonusConfig.MaxBonusY,    
            0f,                       
            bonusConfig.BonusSpacing   
        );
    }

    protected override Bonus SpawnEntry(Vector3 localPos)
    {
        // localPos.x задаёт BaseEnvironmentGenerator, Y подбираем сами так, чтобы не пересекаться с кристаллами
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

                // Выбор типа бонуса (можешь заменить на логику из конфига)
                BonusType type = (Random.value < _luckChance) ? BonusType.luck : BonusType.life;
                bonus.Initialize(type);

                return bonus;
            }
        }

        // не нашли место — просто пропускаем этот X (в очередь попадёт null, базовый класс его выбросит)
        return null;
    }

    private bool IsFreeFromCrystals(Vector2 worldPos)
    {
        // Требование: у кристаллов должен быть Collider2D на слое _crystalMask
        return Physics2D.OverlapCircle(worldPos, _checkRadius, _crystalMask) == null;
    }

    protected override bool IsEntryMissing(Bonus entry) => !entry;

    protected override float GetEntryWorldX(Bonus entry) =>
        entry ? entry.transform.position.x : float.NegativeInfinity;

    protected override void DespawnEntry(Bonus entry)
    {
        if (entry)
            entry.Despawn(); // предполагается, что BaseRecyclable даёт Despawn() (как у Crystal)
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // визуализация радиуса проверки на сцене (необязательно)
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
#endif

}
