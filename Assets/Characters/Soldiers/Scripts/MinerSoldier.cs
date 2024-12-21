using UnityEngine;

public class MinerSoldier : MonoBehaviour, ICombat
{

    [Header("*** Health ***")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Vector3 healthBarOffset;
    [SerializeField] private float healthBarXRotationOffSet = -45f;
    private HealthBarUI healthBarInstance;

    [Header("*** VFX ***")]
    [SerializeField] private GameObject dyingEffect;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private float effectDestroyTime = 1f;
    [SerializeField] private Vector3 effectOffSet;

    [Header("*** OTHER ***")]

    public Lord owner;
    public float Cost = 50f;
    public MeshRenderer meshRenderer;

    public string OwnerTagNameOfCastle;

    private float currentHp;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();
    }

    private void Start()
    {
        InstantiateHealthBar();
        currentHp = maxHp;

    }

    private void Update()
    {
        healthBarInstance.gameObject.transform.SetPositionAndRotation(transform.position + healthBarOffset, Quaternion.Euler(healthBarXRotationOffSet, 0, 0));
    }

    public void InstantiateHealthBar()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity).GetComponent<HealthBarUI>();

        healthBarInstance.relatedBeing = this;
        GameObject soldierCanvas = GameObject.Find("SoldierCanvas");
        healthBarInstance.transform.SetParent(soldierCanvas.transform);
    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0)
            return;


        currentHp -= damage;
        healthBarInstance.TakeDamage(currentHp);
        Destroy(Instantiate(bloodEffect, transform.position + effectOffSet, bloodEffect.transform.rotation), effectDestroyTime);

        if (currentHp <= 0)
        {
            animator.SetTrigger(AnimationKey.Die);
            Destroy(gameObject, 1f);
        }
    }

    public void ChangeMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    private void OnDestroy()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

    }

}
