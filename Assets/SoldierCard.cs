using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    private bool canPlace = true;

    private PlacementArea placementArea;

    private void Awake()
    {
        placementArea=FindAnyObjectByType<PlacementArea>();
        mainCam = Camera.main;
        soldierToTransform = GetSoldierToTransform();
    }


    SoldierSO GetSoldierToTransform()
    {
        if (soldierListCanTransform.Count <= 0)
        {
            return null;
        }
        int r = Random.Range(0, soldierListCanTransform.Count);
        return soldierListCanTransform[r];
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        placementArea.DisableRect();
        if (!isPlacing) return;

        PlaceSoldier();
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlacing) return;

        StartPlacingSoldier();
        placementArea.EnableRect();
    }


    void StartPlacingSoldier()
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

    void PlaceSoldier()
    {
        if (canPlace)
        {
            SoldierCombat soldier = (SoldierCombat)Instantiate(soldierPrefab, placingSoldierPrefab.transform.position, Quaternion.identity);

            soldier.owner.GetComponent<Lord>().AddGold(-soldier.SoldierCost);   
            UIUpdater.instance.UpdateSource();

            soldier.RangePrefab.SetActive(false);
        }


        Destroy(placingSoldierPrefab.gameObject);

        isPlacing = false;
        Cursor.visible = true;

        canPlace = true;
    }

    void PreparePlacingPrefab()
    {
        placingSoldierPrefab = (SoldierCombat)Instantiate(soldierPrefab);
        placingSoldierPrefab.GetComponent<Collider>().enabled = false;
        Destroy(placingSoldierPrefab.GetComponent<Rigidbody>());
        placingSoldierPrefab.GetComponent<NavMeshAgent>().enabled = false;
        placingSoldierPrefab.GetComponent<Animator>().enabled = false;
        placingSoldierPrefab.ChangeMaterial(placingSoldierMaterial);

    }


    private void Update()
    {
        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 realPos = placementArea.GetValidPosition(hit.point + offSet);
                realPos.y= (hit.point + offSet).y;
                placingSoldierPrefab.transform.position = realPos;
            }
        }
    }



}
