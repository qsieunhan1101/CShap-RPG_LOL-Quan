using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState<Player>
{
    public void OnEnter(Player character)
    {
        character.StopMove();

    }

    public void OnExecute(Player character)
    {

    }

    public void OnExit(Player character)
    {

    }
}
