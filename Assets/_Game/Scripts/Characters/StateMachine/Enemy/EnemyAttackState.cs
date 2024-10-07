using UnityEngine;

public class EnemyAttackState : IState<Enemy>
{
    float nextTimeToFire;
    float fireRate;
    public void OnEnter(Enemy character)
    {
        fireRate = 1.5f;
        nextTimeToFire = 1;
        character.MoveTowardTarget(character.EnemyTarget.position);
    }

    public void OnExecute(Enemy character)
    {
        if (character.EnemyTarget == null)
        {
            character.ChangState(new EnemyIdleState());
        }
        else
        {

            if (Vector3.Distance(character.EnemyTarget.position, character.transform.position) > character.Agent.stoppingDistance + 0.2f)
            {
                character.ChangState(new EnemyAttackState());
            }
            if (character.IsFinishMove() == true)
            {
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + fireRate;
                    character.SetRotation(character.EnemyTarget.position);
                    character.ChangeAnim(Constant.ANIM_ATTACK);
                }
            }
        }
    }

    public void OnExit(Enemy character)
    {

    }
}
