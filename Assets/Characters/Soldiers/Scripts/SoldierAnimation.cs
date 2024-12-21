using UnityEngine;

public class SoldierAnimation : MonoBehaviour, ISoldierAnimation
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(AnimationKey.Attack);
    }

    public void DieAnimation()
    {
        animator.SetTrigger(AnimationKey.Die);
    }

    public void HitReactAnimation()
    {
        animator.SetTrigger(AnimationKey.HitReact);
    }


}

public static class AnimationKey
{
    public static readonly string Attack = "Attack";
    public static readonly string HitReact = "HitReact";
    public static readonly string Die = "Die";
}