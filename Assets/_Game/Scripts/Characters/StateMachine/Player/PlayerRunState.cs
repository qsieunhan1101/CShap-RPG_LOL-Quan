using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState<Player>
{
    public void OnEnter(Player character)
    {
            
        character.MoveToPosition(character.TargetPosition);
    }

    public void OnExecute(Player character)
    {
        if (character.IsFinishMove() == true)
        {
            character.ChangeState(new PlayerIdleState());
        }
    }

    public void OnExit(Player character)
    {

    }
}
