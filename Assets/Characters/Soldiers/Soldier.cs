using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    [SerializeField] protected SoldierSO soldierSO;
    [SerializeField] protected GameObject rangePrefab;
    [SerializeField] protected LayerMask groundLayer;

    private bool rangeIsActive;
    protected SoldierSO soldierToTransform;
    protected bool canRevive = true;


    protected virtual void Awake()
    {
        if (rangePrefab != null)
        {
            rangePrefab.transform.localScale = new Vector3(soldierSO.Range, rangePrefab.transform.localScale.y, soldierSO.Range);
            canRevive = true;
        }

    }

    public void SetCanRevive(bool canRevive)
    {
        this.canRevive = canRevive;
    }

    public void SetSoldierToTransform(SoldierSO soldierToTransform)
    {
        print("soldier set");
        this.soldierToTransform = soldierToTransform;
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

}
