using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public Vector3 minimumSpawnArea=Vector3.zero;
    public Vector3 maximumSpawnArea=Vector3.zero;

    public Transform lineRect;

    public void EnableRect()
    {
        lineRect.gameObject.SetActive(true);
        lineRect.position=(minimumSpawnArea+maximumSpawnArea)/2;
        lineRect.localScale=new Vector3(
            Mathf.Abs(maximumSpawnArea.x-minimumSpawnArea.x)/24,
            Mathf.Abs(maximumSpawnArea.z-minimumSpawnArea.z)/48,
            1);
    }

public void DisableRect()
{
    lineRect.gameObject.SetActive(false);
}
    public Vector3 GetValidPosition(Vector3 pos)
    {
        float realX= Mathf.Clamp(pos.x,minimumSpawnArea.x,maximumSpawnArea.x);
        float realZ= Mathf.Clamp(pos.z,minimumSpawnArea.z,maximumSpawnArea.z);
        return new Vector3(realX,0,realZ);
    }



    private void OnDrawGizmos() {
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube((minimumSpawnArea+maximumSpawnArea)/2,maximumSpawnArea-minimumSpawnArea);
    }
}
