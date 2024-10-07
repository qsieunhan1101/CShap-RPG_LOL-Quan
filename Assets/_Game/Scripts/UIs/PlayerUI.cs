using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Player player;
    [Header("Skill 1")]
    [SerializeField] private Canvas skill_1_Canvas;
    [SerializeField] private Image skill_1_Image;
    [SerializeField] private TextMeshProUGUI skill_1_TextCoolDown;
    [SerializeField] private bool isSkill_1_UIActive;
    [SerializeField] private bool isSkill_1_CoolDown = false;
    [SerializeField] private float skill_1_MaxCoolDown;
    [SerializeField] private float skill_1_CoolDownTime;
    private Vector3 posSkill_1;

    [Header("Skill 2")]
    [SerializeField] private Canvas skill_2_Canvas;
    [SerializeField] private Image skill_2_Image;
    [SerializeField] private TextMeshProUGUI skill_2_TextCoolDown;
    [SerializeField] private bool isSkill_2_UIActive;
    [SerializeField] private bool isSkill_2_CoolDown = false;
    [SerializeField] private float skill_2_MaxCoolDown;
    [SerializeField] private float skill_2_CoolDownTime;

    public bool IsSkill_1_UIActive => isSkill_1_UIActive;
    public Vector3 PosSkill_1 => posSkill_1;

    private void Start()
    {
        Skill_1_UIDeactive();
        isSkill_1_CoolDown = false;
        skill_1_Image.fillAmount = 0;
        skill_1_TextCoolDown.gameObject.SetActive(false);


        Skill_2_UIDeactive();
        isSkill_2_CoolDown = false;
        skill_2_Image.fillAmount = 0;
        skill_2_TextCoolDown.gameObject.SetActive(false);
    }

    private void Update()
    {

        //Skill_1_CoolDownUI();
        Skill_1_UI();

        Skill_CoolDownUI(ref isSkill_1_CoolDown,ref skill_1_CoolDownTime, skill_1_MaxCoolDown, skill_1_Image, skill_1_TextCoolDown);


        Skill_2_UI();
        Skill_CoolDownUI(ref isSkill_2_CoolDown, ref skill_2_CoolDownTime, skill_2_MaxCoolDown, skill_2_Image, skill_2_TextCoolDown);

    }

    public void Skill_1_UI()
    {
        if (isSkill_1_CoolDown == true)
        {
            return;
        }
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
                player.isSkill_1_Casting = true;
                isSkill_1_CoolDown = true;
                skill_1_Image.fillAmount = 1;
                skill_1_CoolDownTime = skill_1_MaxCoolDown;
                skill_1_TextCoolDown.gameObject.SetActive(true);
                skill_1_TextCoolDown.text = skill_1_MaxCoolDown.ToString();
                
            }
        }
    }
    public void Skill_2_UI()
    {
        if (isSkill_2_CoolDown == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.W) && isSkill_2_UIActive == false)
        {
            skill_2_Canvas.gameObject.SetActive(true);
            isSkill_2_UIActive = true;
        }
        if (isSkill_2_UIActive == true)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Skill_2_UIDeactive();
                player.ChangeState(new PlayerSkill_2_State());
                player.isSkill_2_Casting = true;
                isSkill_2_CoolDown = true;
                skill_2_Image.fillAmount = 1;
                skill_2_CoolDownTime = skill_2_MaxCoolDown;
                skill_2_TextCoolDown.gameObject.SetActive(true);
                skill_2_TextCoolDown.text = skill_2_MaxCoolDown.ToString();

            }
        }
    }
    private void Skill_1_CoolDownUI()
    {
        if (isSkill_1_CoolDown == true)
        {
            skill_1_CoolDownTime -= Time.deltaTime;
            skill_1_Image.fillAmount = skill_1_CoolDownTime / skill_1_MaxCoolDown;
            skill_1_TextCoolDown.text = Mathf.Floor(skill_1_CoolDownTime).ToString();

            if (skill_1_CoolDownTime <= 0)
            {
                skill_1_CoolDownTime = 0;
                skill_1_Image.fillAmount = 0;
                skill_1_TextCoolDown.gameObject.SetActive(false);
                isSkill_1_CoolDown = false;
            }
        }
    }

    private void Skill_CoolDownUI(ref bool isSkill_CoolDown, ref float skill_CoolDownTime, float skill_MaxCoolDown, Image skill_Image, TextMeshProUGUI skill_TextCoolDown)
    {
        if (isSkill_CoolDown == true)
        {
            skill_CoolDownTime -= Time.deltaTime;
            skill_Image.fillAmount = skill_CoolDownTime / skill_MaxCoolDown;
            skill_TextCoolDown.text = (Mathf.Floor(skill_CoolDownTime*10)/10).ToString();

            if(skill_CoolDownTime <= 0)
            {
                skill_CoolDownTime = 0;
                skill_Image.fillAmount = 0;
                skill_TextCoolDown.gameObject.SetActive(false);
                isSkill_CoolDown = false;
            }
        }
    }

    public void Skill_1_UIDeactive()
    {
        skill_1_Canvas.gameObject.SetActive(false);
        isSkill_1_UIActive = false;
    }
    public void Skill_2_UIDeactive()
    {
        skill_2_Canvas.gameObject.SetActive(false);
        isSkill_2_UIActive = false;
    }
}
