using UnityEngine;

[CreateAssetMenu(fileName = "New Soldier", menuName = "Soldier")]
public class SoldierSO : ScriptableObject
{
    [SerializeField] private Soldier soldierPrefab;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float range;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectionRange;
    [SerializeField] private float maxHp;

    public float AttackDamage { get => attackDamage;}
    public float AttackSpeed { get => attackSpeed; }
    public float Range { get => range;}
    public float MoveSpeed { get => moveSpeed; }
    public float AttackRange { get => attackRange; }
    public float DetectionRange { get => detectionRange; }
    public Soldier SoldierPrefab { get => soldierPrefab;}
    public float MaxHp { get => maxHp;}
}
