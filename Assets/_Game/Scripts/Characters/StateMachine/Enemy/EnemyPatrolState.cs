using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IState<Enemy>
{
    float time;
    float timeInState;
    public void OnEnter(Enemy character)
    {
        time = 0;
        timeInState = Random.Range(6,7);
        character.MoveToPosition(character.RandomPositionInMap());
    }

    public void OnExecute(Enemy character)
    {
        time += Time.deltaTime;
        if (character.IsFinishMove() == true || time >= timeInState)
        {
            character.ChangState(new EnemyIdleState());
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
