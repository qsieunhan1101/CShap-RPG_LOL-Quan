using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeadState : IState<Player>
{
    public void OnEnter(Player character)
    {

        character.StopMove();
        character.ChangeAnim(Constant.ANIM_DEAD);
    }

    public void OnExecute(Player character)
    {
        character.ChangeGameStateFail();

    }

    public void OnExit(Player character)
    {
    }
}
