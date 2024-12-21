using UnityEngine;

public class Mine : MonoBehaviour
{
    public int allyMinerCount;
    public int enemyMinerCount;

    public LayerMask allyLayer;
    public LayerMask enemyLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Miner"))
        {
            Miner miner = other.GetComponent<Miner>();

            if (miner != null)
            {
                if (miner.OwnerTagNameOfCastle == "AllyCastle")
                {
                    allyMinerCount++;
                }
                else
                {
                    enemyMinerCount++;
                }

                GetComponent<MineUIUpdater>().UpdateMinerCounts();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Miner"))
        {
            Miner miner = other.GetComponent<Miner>();

            if (miner != null)
            {
                if (miner.OwnerTagNameOfCastle == "AllyCastle")
                {
                    allyMinerCount--;
                }
                else
                {
                    enemyMinerCount--;
                }

                GetComponent<MineUIUpdater>().UpdateMinerCounts();
            }
        }
    }
}
