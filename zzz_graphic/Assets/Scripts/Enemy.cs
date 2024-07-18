using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [SerializeField] private Actor simpleTarget;

    [SerializeField] private float fstopDistance;

    [SerializeField] private BoxCollider attackCollider;


    private static int ANIM_WALK = Animator.StringToHash("bWalk");
    private static int ANIM_ATTACK = Animator.StringToHash("bAttack");

    private EState state;

    public Animator animator;

    public bool bStateFance = false;

    private EState.State estate = EState.State.Idle;
    private EState currentState;
    private bool bstateFlag = false;

    public Animator anim { get => animator; }
    public NavMeshAgent agent { get => navMeshAgent; }
    public Actor target { get => simpleTarget; }
    public float stopDistance { get => fstopDistance; }

    public bool bisAttack = false;
    public int attackDmg = 1;


    public void SwitchState<T>() where T : EState, new()
    {
        currentState.EndState(this);
        currentState = new T();
        currentState.StartState(this);
    }
    public void SetStateLock(bool _active)
    {
        bStateFance = _active;
    }



    private float GetTargetDistance(Actor _actor)
    {
        return Vector3.Distance(this.transform.position, _actor.transform.position);
    }


    public void OnStartAnimation()
    {
        //bStateFance = true;
    }
    public void OnEndAniamtion()
    {
        //bStateFance = false;
        //agent.isStopped = false;
        if (currentState.state == EState.State.Attack)
            currentState.EndState(this);

        bisAttack = false;
    }
    public void OnStartAttack()
    {
        bisAttack = true;
    }
    public void OnEndAttack()
    {
        bisAttack = false;
    }


    private void Tick()
    {
        //입력이 없을경우 Idle 상태로 변경

        if (bStateFance) return;

        float distance = GetTargetDistance(simpleTarget);
        if (distance > stopDistance)
        {
            //walk
            if (currentState.state != EState.State.Walk)
            {
                SwitchState<EnemyWalkState>();
            }               
            return;
        }
        else
        {
            //attack
            if(currentState.state != EState.State.Attack)
            {
                SwitchState<EnemyAttackState>();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor != null && bisAttack)
        {
            actor.Damaged(attackDmg);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit");
    }

    void Update()
    {
        Tick();

        if (!bStateFance)
            currentState.UpdateState(this);

    }

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = new EnemyIdleState();
        currentState.StartState(this);
    }
}
