using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierTranformation : MonoBehaviour
{
    [SerializeField] private List<Soldier> soldiersToTransform;

    public Soldier GetRandomSoldierToTransform()
    {
        if (soldiersToTransform.Count <= 0)
        {
            return null;
        }

        int r = Random.Range(0, soldiersToTransform.Count);

        if (soldiersToTransform[r] == null)
        {
            return null;
        }
        return soldiersToTransform[r];
    }

}
