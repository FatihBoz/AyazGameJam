using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEffectTrigger : MonoBehaviour
{
    public BaseSkill areaEffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            areaEffect.ApplyEffect();
            print("Basýldý efekti çaðýrdým");
        }

    }
}
