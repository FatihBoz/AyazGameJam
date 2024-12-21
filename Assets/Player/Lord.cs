using UnityEngine;

public class Lord : MonoBehaviour
{
    public int gold = 0;

    public int currentMinerCount = 0;

    public void AddGold(float amount)
    {
        gold += (int)amount;
    }

    private void Update()
    {
    }
}
