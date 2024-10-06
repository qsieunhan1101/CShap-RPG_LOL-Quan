using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_1_State : IState<Player>
{
    public void OnEnter(Player character)
    {

        character.Skill_1_Cast();
    }

    public void OnExecute(Player character)
    {

    }

    public void OnExit(Player character)
    {
        
    }
}
