using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Player player;
    [Header("Skill 1")]
    [SerializeField] private Canvas skill_1_Canvas;
    [SerializeField] private bool isSkill_1_UIActive;
    public Vector3 posSkill_1;


    public bool IsSkill_1_UIActive => isSkill_1_UIActive;
    private void Start()
    {
        Skill_1_UIDeactive();
    }

    public void Skill_1_UI()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isSkill_1_UIActive == false)
        {
            skill_1_Canvas.gameObject.SetActive(true);
            isSkill_1_UIActive = true;
        }
        if (isSkill_1_UIActive == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
            {
                posSkill_1 = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            }
            Quaternion qua = Quaternion.LookRotation(posSkill_1 - transform.position);

            qua.eulerAngles = new Vector3(0, qua.eulerAngles.y, qua.eulerAngles.z);

            skill_1_Canvas.transform.rotation = Quaternion.Lerp(qua, skill_1_Canvas.transform.rotation, 0);

            if (Input.GetMouseButtonDown(0))
            {
                Skill_1_UIDeactive();
                player.ChangeState(new PlayerSkill_1_State());
            }
        }
    }

    public void Skill_1_UIDeactive()
    {
        skill_1_Canvas.gameObject.SetActive(false);
        isSkill_1_UIActive = false;
    }
}
