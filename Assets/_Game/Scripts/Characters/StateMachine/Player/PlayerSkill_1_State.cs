
using UnityEngine;

public class PlayerSkill_1_State : IState<Player>
{
    float time;
    float delayStateTime;
    public void OnEnter(Player character)
    {
        time = 0;
        delayStateTime = 1.0f;
        character.Skill_1_Cast();
    }

    public void OnExecute(Player character)
    {
        if (character.isSkill_1_Casting == false)
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
