using System.Collections.Generic;
using UnityEngine;

public class SoldierTranformation : MonoBehaviour
{
    [SerializeField] private List<Soldier> soldiersToTransform;

    public Soldier GetRandomSoldierToTransform()
    {
        int r = Random.Range(0, soldiersToTransform.Count);

        if (soldiersToTransform[r] == null)
        {
            return null;
        }
        return soldiersToTransform[r];
    }

}
