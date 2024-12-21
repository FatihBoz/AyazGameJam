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
    public Transform minerPoint;
    public List<Transform> enemySpawns;

    public GameObject[] soldiers;


    private float decisionInterval = 5f; // Karar verme aralýðý (5 saniye)
    private float lastDecisionTime = 0f; // Son karar zamaný

    private void Update()
    {
        if (Time.time >= lastDecisionTime + decisionInterval)
        {
            lastDecisionTime = Time.time; // Karar zamanýný güncelle
            MakeDecision();
        }
    }

    void MakeDecision()
    {
        // Saldýrý Altýnda
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
            return; // Eðer spawn noktasý yoksa çýk.

        // Rastgele bir spawn noktasý seç
        Transform spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Count)];

        // Rastgele bir asker türü seç
        GameObject selectedSoldier = soldiers[Random.Range(0, soldiers.Length)];

        // Askeri spawn et
        Instantiate(selectedSoldier, spawnPoint.position, spawnPoint.rotation);


        //Cost'a eriþ
        //GetComponent<Lord>().AddGold(selectedSoldier.GetComponent<SoldierCombat>().soldierSO);
        GetComponent<Lord>().AddGold(-20);


        Debug.Log("Hop yeni asker");
    }

    void ProduceMiner()
    {

        if (miner == null || minerPoint == null || !hasEnoughSourceForMiner())
        {
            print("belkide madenci için paran yoktur he?_");
            return; // Eðer madenci prefabý ya da spawn noktasý eksikse çýk.

        }

        // Madenciyi spawn et
        Instantiate(miner, minerPoint.position, minerPoint.rotation);

        // Madenci sayýsýný güncelle (Lord scriptine baðlý)
        GetComponent<Lord>().currentMinerCount++;

        print("HOp yeni madenci");
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
