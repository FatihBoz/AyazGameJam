using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MineMovement : MonoBehaviour
{

    public NavMeshAgent agent;

    public void MoveToMine(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 1f; // Hedefe yaklaþýnca dur
    }
}
