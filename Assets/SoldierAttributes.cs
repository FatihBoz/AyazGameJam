using UnityEngine;

public class SoldierAttributes : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private GameObject rangePrefab;
    [SerializeField] private string layerName = "Ground";

    public GameObject RangePrefab => rangePrefab;

    private void Awake()
    {
        rangePrefab.transform.localScale = new Vector3 (range, rangePrefab.transform.localScale.y, range);
        gameObject.layer = LayerMask.GetMask(layerName);
    }

    private void OnMouseDown()
    {
        rangePrefab.SetActive(true);
    }

    

    private void Update()
    {
        
    }
}
