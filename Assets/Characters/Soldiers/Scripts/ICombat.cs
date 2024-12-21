using UnityEngine;

public interface ICombat
{
    void TakeDamage(float damageAmount);

    void TakeDamage(float damageAmount,Vector3 pos);
}
