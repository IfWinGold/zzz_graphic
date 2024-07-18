using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EState
{
    private static int ANIM_ATTACK = Animator.StringToHash("bAttack");
    public override State state => State.Attack;
    public override void EndState(Enemy _enemy)
    {
        _enemy.animator.SetBool(ANIM_ATTACK, false);
        _enemy.SetStateLock(false);
        _enemy.agent.isStopped = false;
    }

    public override void StartState(Enemy _enemy)
    {
        _enemy.agent.isStopped = true;
        _enemy.animator.SetBool(ANIM_ATTACK, true);
        _enemy.SetStateLock(true);
    }

    public override void UpdateState(Enemy _enemy)
    {
       
    }
}
