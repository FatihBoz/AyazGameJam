using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MinerCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private MinerSoldier minerPrefab;
    [SerializeField] protected Material placingSoldierMaterial;

    [SerializeField] protected Vector3 offSet;

    protected Camera mainCam;
    protected bool isPlacing;
    protected bool canPlace = true;
    private MinerSoldier placingMinerPrefab;

    public GameObject allyCastle;

    protected virtual void Awake()
    {
        mainCam = Camera.main;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        PlacementArea.Instance.DisableRect();
        if (!isPlacing) return;

        PlaceSoldier();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlacing) return;

        StartPlacingSoldier();
        PlacementArea.Instance.EnableRect();
    }





    protected virtual void StartPlacingSoldier()
    {
        if (isPlacing) return;

        PreparePlacingPrefab();

        

        if (placingMinerPrefab.owner.GetComponent<Lord>().gold < placingMinerPrefab.Cost)
        {
            canPlace = false;
        }

        isPlacing = true;
        Cursor.visible = false;
    }

    protected virtual void PlaceSoldier()
    {
        if (canPlace)
        {
            MinerSoldier soldier = Instantiate(minerPrefab, placingMinerPrefab.transform.position, Quaternion.identity);

            soldier.gameObject.GetComponent<Miner>().SpawnPoint = allyCastle.transform.position;

            Lord lord = soldier.gameObject.GetComponent<Miner>().Owner.GetComponent<Lord>();

            lord.AddGold(-soldier.Cost);

            lord.currentMinerCount++;


            print(lord.currentMinerCount);

            UIUpdater.instance.UpdateSource();
        }


        Destroy(placingMinerPrefab.gameObject);

        isPlacing = false;
        Cursor.visible = true;

        canPlace = true;
    }

    void PreparePlacingPrefab()
    {
        placingMinerPrefab = Instantiate(minerPrefab);
        placingMinerPrefab.GetComponent<Collider>().enabled = false;
        placingMinerPrefab.GetComponent<NavMeshAgent>().enabled = false;
        placingMinerPrefab.meshRenderer.material = placingSoldierMaterial;
    }

    protected virtual void Update()
    {
        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 realPos =  PlacementArea.Instance.GetValidPosition(hit.point + offSet);
                realPos.y= (hit.point + offSet).y;
                placingMinerPrefab.transform.position = realPos;
            }
        }
    }

}
