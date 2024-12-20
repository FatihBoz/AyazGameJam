using UnityEngine;
using UnityEngine.EventSystems;

public class SoldierCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SoldierAttributes soldierPrefab;

    private Camera mainCam;
    private bool isPlacing;
    private Vector3 offSet = new(0f,.9f,0f);
    private GameObject placingPrefab;


    private void Awake()
    {
        mainCam = Camera.main;
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

        placingPrefab = Instantiate(soldierPrefab.gameObject);
        placingPrefab.GetComponent<Collider>().enabled = false;

        isPlacing = true;
        Cursor.visible = false; 
    }

    void PlaceSoldier()
    {
        SoldierAttributes soldier = Instantiate(soldierPrefab, placingPrefab.transform.position, Quaternion.identity);
        soldier.RangePrefab.SetActive(false);
        Destroy(placingPrefab);

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
                placingPrefab.transform.position = hit.point + offSet;
            }
        }
    }



}
