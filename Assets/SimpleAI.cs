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
    public GameObject giant;
    public GameObject shooterFly;
    public GameObject meleeFly;
    public GameObject miner;

    [Header("Spawn Points")]
    public Transform minerPoint;
    public List<Transform> enemySpawns;

    GameObject[] soldiers;

    private void Awake()
    {
        GameObject[] soldiers = { giant, shooterFly, meleeFly };

    }


    private void Update()
    {
        //Sald�r� Alt�nda
        if (isUnderAttack)
        {
            if (hasEnoughSource())
            {
                //Produce Soldier
                ProduceRandomSoldierInRandomPlace();

            }
            else
            {
                //Produce Miner
                ProduceMiner();
            }
        }
        else
        {
            if(hasEnoughMiner())
            {
                if (hasEnoughSource())
                {
                    //Produce Soldier
                    ProduceRandomSoldierInRandomPlace();
                }
            }
            else
            {
                //Produce Miner
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

        Debug.Log("Hop yeni asker");
    }

    void ProduceMiner()
    {

        if (miner == null || minerPoint == null || !hasEnoughSourceForMiner())
        {
            print("belkide madenci i�in paran yoktur he?_");
            return; // E�er madenci prefab� ya da spawn noktas� eksikse ��k.

        }

        // Madenciyi spawn et
        Instantiate(miner, minerPoint.position, minerPoint.rotation);

        // Madenci say�s�n� g�ncelle (Lord scriptine ba�l�)
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
        if (GetComponent<Lord>().currentMinerCount < idealMinerCount)
        {
            return true;
        }
        return false;
    }
}
