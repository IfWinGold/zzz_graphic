using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private ActorCamera actorCamera;
    [SerializeField] private float fsmoothTime = 0.05f;

    
    protected private static int ANIM_B_WALK = Animator.StringToHash("bWalk");
    protected private static int ANIM_T_DASH = Animator.StringToHash("tDash");
    protected private static int ANIM_B_RUN = Animator.StringToHash("bDash");

    protected CharacterController characterController;
    protected Animator animator;    
    protected float fmoveSpeed = 1.0f;
    protected float frotSpeed = 50.0f;

    private AState.State estate = AState.State.Idle;
    private AState currentState;
    private bool bstateFance = false;


    public Animator anim { get => animator; }
    public Vector3 inputDirection { get => vmoveDiretion; }


    protected Vector3 vmoveDiretion = Vector3.zero;

    private float fcurrentVelocity = 0f;

    [SerializeField] private Renderer[] playerRenderers;


    public void SwitchState<T>() where T : AState, new()
    {
        currentState.EndState(this);
        currentState = new T();
        currentState.StartState(this);
    }
    

    protected virtual void Initialized()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        transform.rotation = Quaternion.identity;
        animator = GetComponent<Animator>();

        currentState = new IdleState();
        currentState.StartState(this);
    }
    public void UpdateMove(Vector3 _moveDir, float _speed)
    {
        float targetAngle = Mathf.Atan2(_moveDir.x, _moveDir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref fcurrentVelocity, fsmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        characterController.Move(_moveDir * Time.deltaTime * _speed);
    }



    protected void Tick()
    {
        //입력이 없을경우 Idle 상태로 변경

        if (bstateFance) return;

        Vector3 inputDirection = GameUtility.GetInputDirection();
        if (inputDirection == Vector3.zero)
        {
            //Idle
            if(currentState.state != AState.State.Idle)
                SwitchState<IdleState>();

            return;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Dash
                SwitchState<DashState>();
            }            
            else if(Input.GetKey(KeyCode.LeftShift))
            {
                //Run
                SwitchState<RunState>();
            }
            else
            {

                //Walk
                SwitchState<WalkState>();
            }
        }                                          
    }

    public void Damaged(int _dmg)
    {
        if(currentState.state == AState.State.Dash)
        {
            Debug.Log($"Damaged!");
        }
        else
        {
            
        }       
    }

    public void SetStateLock(bool _active)
    {
        bstateFance = _active;
    }

    void Start()
    {
        Initialized();
    }

    
    void Update()
    {
        Tick();
        
        currentState.UpdateState(this);


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(var renderer in playerRenderers)
            {
                renderer.material.SetFloat("_RimPower", 10f);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var renderer in playerRenderers)
            {
                renderer.material.SetFloat("_RimPower", 0f);
            }
        }
    }    
}
