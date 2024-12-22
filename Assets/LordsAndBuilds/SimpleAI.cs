using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public int idealMinerCount = 5;

    public bool isUnderAttack = false;

    public int requiredGold = 100;

    public int requiredGoldForMiner = 50;

    //public float enemyAttackPower = 0;

    [Header("Prefabs")]
    public GameObject miner;

    [Header("Spawn Points")]
    public List<Transform> minerSpawns;
    public List<Transform> enemySpawns;

    public GameObject[] soldiers;

    public GameObject tempMiner;

    private float decisionInterval = 5f; // Karar verme aral��� (5 saniye)
    private float lastDecisionTime = 0f; // Son karar zaman�

    private void Update()
    {
        if (Time.time >= lastDecisionTime + decisionInterval)
        {
            lastDecisionTime = Time.time; // Karar zaman�n� g�ncelle
            MakeDecision();
        }
    }

    void MakeDecision()
    {
        // Sald�r� Alt�nda
        if (isUnderAttack)
        {
            if (hasEnoughSource())
            {
                // Produce Soldier
                ProduceRandomSoldierInRandomPlace();
            }
            else
            {
                // Produce Miner
                ProduceMiner();
            }
        }
        else
        {
            if (hasEnoughMiner())
            {
                if (hasEnoughSource())
                {
                    // Produce Soldier
                    ProduceRandomSoldierInRandomPlace();
                }
            }
            else
            {
                // Produce Miner
                ProduceMiner();
            }
        }
    }


    void ProduceRandomSoldierInRandomPlace()
    {
        if (enemySpawns.Count == 0)
            return; // E�er spawn noktas� yoksa ��k.

        // Rastgele bir spawn noktas� se�
        Transform spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Count)];

        // Rastgele bir asker t�r� se�
        GameObject selectedSoldier = soldiers[Random.Range(0, soldiers.Length)];

        // Askeri spawn et
        Instantiate(selectedSoldier, spawnPoint.position, spawnPoint.rotation);


        //Cost'a eri�
        //GetComponent<Lord>().AddGold(selectedSoldier.GetComponent<SoldierCombat>().soldierSO);
        GetComponent<Lord>().AddGold(-20);


        Debug.Log("Hop yeni asker");
    }

    void ProduceMiner()
    {

        if (minerSpawns.Count == 0)
            return; // E�er spawn noktas� yoksa ��k.

        // Rastgele bir spawn noktas� se�
        Transform spawnPoint = minerSpawns[Random.Range(0, minerSpawns.Count)];

        // Askeri spawn et
        tempMiner = Instantiate(miner, spawnPoint.position, spawnPoint.rotation);
        tempMiner.GetComponent<Miner>().SpawnPoint = spawnPoint.position;

        tempMiner.GetComponent<Miner>().Owner.GetComponent<Lord>().currentMinerCount++;

        //Cost'a eri�
        //GetComponent<Lord>().AddGold(selectedSoldier.GetComponent<SoldierCombat>().soldierSO);
        GetComponent<Lord>().AddGold(-20);


        Debug.Log("Hop yeni asker");

    }

    bool hasEnoughSourceForMiner()
    {
        if (GetComponent<Lord>().gold >= requiredGoldForMiner)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    bool hasEnoughSource()
    {
        if(GetComponent<Lord>().gold >= requiredGold)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    bool hasEnoughMiner()
    {
        if (GetComponent<Lord>().currentMinerCount >= idealMinerCount)
        {
            return true;
        }
        return false;
    }
}
