using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState<Enemy>
{
    float time;
    float timeInState;
    public void OnEnter(Enemy character)
    {
        time = 0;
        timeInState = Random.Range(3,5);
        character.StopMove();
    }

    public void OnExecute(Enemy character)
    {
        time += Time.deltaTime;
        if (time >= timeInState)
        {
            character.ChangState(new EnemyPatrolState());
        }
        if (character.EnemyTarget != null)
        {
            character.ChangState(new EnemyAttackState());
        }
    }

    public void OnExit(Enemy character)
    {
        
    }
}
