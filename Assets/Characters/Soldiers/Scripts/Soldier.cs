using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    [SerializeField] protected SoldierSO soldierSO;
    [SerializeField] protected GameObject rangePrefab;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float rangeIndicatorExpandRate = .3f;

    private bool rangeIsActive;
    protected Soldier soldierToTransform;
    protected bool canRevive = true;
    private EnemySoldierTranformation tranformation;

    [SerializeField] protected SoldierSide soldierSide;


    protected virtual void Awake()
    {
        tranformation = GetComponent<EnemySoldierTranformation>();
        if (transform != null && soldierSide == SoldierSide.Enemy)
        {
            print("buraya girmemesi lazým:"+soldierSide.ToString());
            soldierToTransform = tranformation.GetRandomSoldierToTransform();
        }


        if (rangePrefab != null)
        {
            rangePrefab.transform.localScale = new Vector3(soldierSO.Range * rangeIndicatorExpandRate, soldierSO.Range * rangeIndicatorExpandRate,  rangePrefab.transform.localScale.z);
            canRevive = true;
        }

    }

    public void SetCanRevive(bool canRevive)
    {
        this.canRevive = canRevive;
    }

    protected void OnMouseDown()
    {
        if (rangePrefab == null)
            return;

        if (rangeIsActive)
        {
            rangeIsActive = false;
            rangePrefab.SetActive(rangeIsActive);
        }
        else
        {
            rangeIsActive = true;
            rangePrefab.SetActive(rangeIsActive);
        }
    }

    public GameObject RangePrefab => rangePrefab;

    public float SoldierCost => soldierSO.Cost;

    public SoldierSide SoldierSide => soldierSide;

    public Sprite NextSoldierSprite => soldierSO.NextImageSprite;

}


public enum SoldierSide
{
    Ally,
    Enemy
}
