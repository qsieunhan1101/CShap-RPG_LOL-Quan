using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IState<Player>
{
    float nextTimeToFire;
    float fireRate;
    public void OnEnter(Player character)
    {

        fireRate = 1.5f;
        nextTimeToFire = 1;
        character.MoveTowardTarget(character.EnemyTarget.position);
    }

    public void OnExecute(Player character)
    {
        if (character.IsFinishMove() == true)
        {
            character.Agent.ResetPath();
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                character.ChangeAnim(Constant.ANIM_ATTACK);
            }
        }
    }

    public void OnExit(Player character)
    {
    }
}
