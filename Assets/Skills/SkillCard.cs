using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject DamageArea;

    GameObject instantiatedPrefab;

    public GameObject Prefab;

    private Camera mainCam;

    Vector3 worldPosition;

    bool isPlacing = false;

    public LayerMask groundLayer;



    //[Header("CountDown")]
    //public float cooldownDuration = 5f; // Geri sayým süresi (saniye cinsinden)
    //private float cooldownTimer = 0f;
    //private bool isCooldown = false;

    public TextMeshProUGUI cooldownText;

    private void Awake()
    {
        mainCam = Camera.main;
        DamageArea = Instantiate(DamageArea, worldPosition, DamageArea.transform.rotation);

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        isPlacing = true;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCam.transform.position.z); // Z eksenini ayarla

        worldPosition = mainCam.ScreenToWorldPoint(mousePosition);
        DamageArea.SetActive(true);
        instantiatedPrefab = Instantiate(Prefab, worldPosition, Quaternion.identity);
        instantiatedPrefab.GetComponent<ParticleSystem>().Stop();


        //virtualPrefab.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (instantiatedPrefab != null)
        {
            DamageArea.GetComponent<DamageArea>().ApplyEffect();

            DamageArea.SetActive(false);

            instantiatedPrefab.GetComponent<ParticleSystem>().Play();
            isPlacing=false;

        }
    }


    // Update is called once per frame
    void Update()
    {


        if (isPlacing && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                instantiatedPrefab.transform.position = hit.point;
                DamageArea.transform.position = hit.point;

            }
        }

        /*
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownText != null)
            {
                cooldownText.text = Mathf.Ceil(cooldownTimer).ToString(); // UI'da geri sayýmý göster
            }

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                if (cooldownText != null)
                {
                    cooldownText.text = "Ready!";
                }
            }
        }
        */
    }
}
