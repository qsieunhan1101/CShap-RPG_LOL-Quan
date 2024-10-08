using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_3_State : IState<Player>
{
    float time;
    float delayStateTime;
    public void OnEnter(Player character)
    {
        time = 0;
        delayStateTime = 1.0f;
        character.Skill_3_Cast();
    }

    public void OnExecute(Player character)
    {
        if (character.isSkill_3_Casting == false)
        {
            time += Time.deltaTime;
            if (time >= delayStateTime)
            {
                character.ChangeState(new PlayerIdleState());
            }
        }
    }

    public void OnExit(Player character)
    {
    }
}
