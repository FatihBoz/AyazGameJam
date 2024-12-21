using TMPro;
using UnityEngine;

public class Mine : MonoBehaviour
{
    int allyMinerCount;
    int enemyMinerCount;

    public TextMeshProUGUI allyCount;

    public TextMeshProUGUI enemyCount;

    public string allyLayer;
    public string enemyLayer;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer(allyLayer))
        {
            allyMinerCount++;
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            enemyMinerCount++;
        }

        UpdateMinerCounts();
    }

    private void OnTriggerExit(Collider other)
    {
        print("dost madenci Çýkýyo: " + allyMinerCount);

        if (other.gameObject.layer == LayerMask.NameToLayer(allyLayer))
        {
            allyMinerCount--;
            print("dost madenci Ç: " + allyMinerCount);

        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            enemyMinerCount--;
        }

        UpdateMinerCounts();

    }


    public void UpdateMinerCounts()
    {
        allyCount.text = allyMinerCount.ToString();
        enemyCount.text = enemyMinerCount.ToString();
    }
}
