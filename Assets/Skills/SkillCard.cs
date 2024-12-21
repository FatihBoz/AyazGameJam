using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //public GameObject Effect;

    public GameObject virtualPrefab;

    private Camera mainCam;

    Vector3 worldPosition;

    bool isPlacing = false;

    public LayerMask groundLayer;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isPlacing = true;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCam.transform.position.z); // Z eksenini ayarla

        worldPosition = mainCam.ScreenToWorldPoint(mousePosition);

        virtualPrefab = Instantiate(virtualPrefab, worldPosition, Quaternion.identity);
        virtualPrefab.GetComponent<ParticleSystem>().Stop();


        //virtualPrefab.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (virtualPrefab != null)
        {

            virtualPrefab.GetComponent<ParticleSystem>().Play();
            isPlacing=false;
            // Mouse pozisyonunu tekrar al ve instantiate et
            //Vector3 mousePosition = Input.mousePosition;
            //mousePosition.z = Mathf.Abs(mainCam.transform.position.z); // Z eksenini ayarla

            //Vector3 worldPosition = mainCam.ScreenToWorldPoint(mousePosition);

            // Sanal objeyi sil ve gerçek objeyi oluþtur
            //Destroy(virtualPrefab);
            //Instantiate(Effect, worldPosition, Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                virtualPrefab.transform.position = hit.point;
            }
        }
    }
}
