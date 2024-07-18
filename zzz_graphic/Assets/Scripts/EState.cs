using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EState
{
    public enum State
    {
        Idle,
        Walk,
        Attack
    }
    public abstract State state { get; }
    public abstract void StartState(Enemy _enemy);
    public abstract void UpdateState(Enemy _enemy);
    public abstract void EndState(Enemy _enemy);
}
