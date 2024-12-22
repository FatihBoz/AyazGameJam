using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldierCard : MinerCard, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private List<Soldier> soldiersToTransform;
    [SerializeField] private Soldier soldierPrefab;
    [SerializeField] private Image nextSoldierImage;

    private SoldierCombat placingSoldierPrefab;
    private Soldier soldierToTransform;

  

    protected override void Awake()
    {

        base.Awake();
        mainCam = Camera.main;

        SetNextSoldier();
    }


    public Soldier GetRandomSoldierToTransform()
    {
        if (soldiersToTransform.Count <= 0)
        {
            return null;
        }

        int r = Random.Range(0, soldiersToTransform.Count);

        if (soldiersToTransform[r] == null)
        {
            return null;
        }
        return soldiersToTransform[r];
    }


    void SetNextSoldier()
    {
        soldierToTransform = GetRandomSoldierToTransform();
        nextSoldierImage.sprite = soldierToTransform.NextSoldierSprite;
    }



    protected override void StartPlacingSoldier()
    {
        if (isPlacing) return;

        PreparePlacingPrefab();

        if (placingSoldierPrefab.owner.GetComponent<Lord>().gold < placingSoldierPrefab.SoldierCost)
        {
            canPlace = false;
        }

        isPlacing = true;
        Cursor.visible = false; 
        
    }

    protected override void PlaceSoldier()
    {
        if (canPlace)
        {
            SoldierCombat soldier = (SoldierCombat)Instantiate(soldierPrefab, placingSoldierPrefab.transform.position, Quaternion.identity);

            if (soldier.SoldierSide == SoldierSide.Ally)
            {
                soldier.SetNextSoldierToTransform(soldierToTransform);
            }

            soldier.owner.GetComponent<Lord>().AddGold(-soldier.SoldierCost);   
            UIUpdater.instance.UpdateSource();

            
            //soldier.RangePrefab.SetActive(false);

            SetNextSoldier();
        }



        Destroy(placingSoldierPrefab.gameObject);

        isPlacing = false;
        Cursor.visible = true;

        canPlace = true;
    }

    void PreparePlacingPrefab()
    {
        placingSoldierPrefab = (SoldierCombat)Instantiate(soldierPrefab);
        placingSoldierPrefab.RangePrefab.SetActive(true);
        //Disable soldier interactions while placing
        placingSoldierPrefab.GetComponent<Collider>().enabled = false;
        Destroy(placingSoldierPrefab.GetComponent<Rigidbody>());
        placingSoldierPrefab.GetComponent<NavMeshAgent>().enabled = false;

        placingSoldierPrefab.ChangeMaterial(placingSoldierMaterial);

    }


    protected override void Update()
    {

        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 realPos = PlacementArea.Instance.GetValidPosition(hit.point + offSet);
                realPos.y= (hit.point + offSet).y;
                placingSoldierPrefab.transform.position = realPos;
            }
        }
    }



}
