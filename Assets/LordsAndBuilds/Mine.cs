using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int allyMinerCount;
    public int enemyMinerCount;

    public LayerMask allyLayer;
    public LayerMask enemyLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == allyLayer)
        {
            allyMinerCount++;
            GetComponent<MineUIUpdater>().UpdateMinerCounts();

        }
        else if (other.gameObject.layer == enemyLayer)
        {
            enemyMinerCount++;
            GetComponent<MineUIUpdater>().UpdateMinerCounts();
        }
    }

    void MinerEnteredMine(GameObject miner)
    {
        
    }

}
