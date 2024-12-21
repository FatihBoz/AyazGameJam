using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI allyCount;

    public TextMeshProUGUI enemyCount;

    public void UpdateMinerCounts()
    {
        allyCount.text = GetComponent<Mine>().allyMinerCount.ToString();
        enemyCount.text = GetComponent<Mine>().enemyMinerCount.ToString();
    }
}
