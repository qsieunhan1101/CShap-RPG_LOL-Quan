using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : IState<Enemy>
{
    public void OnEnter(Enemy character)
    {
        character.StopMove();
        character.ChangeAnim(Constant.ANIM_DEAD);
        character.gameObject.tag = Constant.TAG_UNTAGGED;
    }

    public void OnExecute(Enemy character)
    {
    }

    public void OnExit(Enemy character)
    {
    }
}
