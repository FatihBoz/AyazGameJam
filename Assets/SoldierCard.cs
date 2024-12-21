using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private List<SoldierSO> soldierListCanTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Soldier soldierPrefab;
    [SerializeField] private Material placingSoldierMaterial;

    [SerializeField] private Vector3 offSet;

    private Camera mainCam;
    private bool isPlacing;
    private SoldierCombat placingSoldierPrefab;
    private SoldierSO soldierToTransform;



    private void Awake()
    {
        mainCam = Camera.main;
        soldierToTransform = GetSoldierToTransform();
    }


    SoldierSO GetSoldierToTransform()
    {
        int r = Random.Range(0, soldierListCanTransform.Count);
        return soldierListCanTransform[r];
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPlacing) return;

        PlaceSoldier();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlacing) return;

        StartPlacingSoldier();
    }


    void StartPlacingSoldier()
    {
        if (isPlacing) return;

        placingSoldierPrefab = (SoldierCombat)Instantiate(soldierPrefab);
        placingSoldierPrefab.GetComponent<Collider>().isTrigger = true;
        placingSoldierPrefab.ChangeMaterial(placingSoldierMaterial);    

        isPlacing = true;
        Cursor.visible = false; 
    }

    void PlaceSoldier()
    {
        SoldierCombat soldier = (SoldierCombat)Instantiate(soldierPrefab, placingSoldierPrefab.transform.position, Quaternion.identity);

        //COST belirleme
        //soldier.GetComponent<SoldierCombat>().owner.GetComponent<Lord>().AddGold(-20);
        //UIUpdater.instance.UpdateSource();
        
        soldier.RangePrefab.SetActive(false);
        Destroy(placingSoldierPrefab.gameObject);

        isPlacing = false;
        Cursor.visible = true;
    }


    private void Update()
    {
        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                placingSoldierPrefab.transform.position = hit.point + offSet;
            }
        }
    }



}
