using UnityEngine;

public class Lord : MonoBehaviour
{
    public int gold = 0;

    public void AddGold(float amount)
    {
        gold += (int)amount;
        Debug.Log("Gold: " + gold);
    }

    private void Update()
    {
        print(gold);
    }
}
