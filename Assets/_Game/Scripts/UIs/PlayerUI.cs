using TMPro;
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

    [Header("Skill 2")]
    [SerializeField] private Canvas skill_2_Canvas;
    [SerializeField] private Image skill_2_Image;
    [SerializeField] private TextMeshProUGUI skill_2_TextCoolDown;
    [SerializeField] private bool isSkill_2_UIActive;
    [SerializeField] private bool isSkill_2_CoolDown = false;
    [SerializeField] private float skill_2_MaxCoolDown;
    [SerializeField] private float skill_2_CoolDownTime;

    [Header("Skill 3")]
    [SerializeField] private Canvas skill_3_Canvas;
    [SerializeField] private Image skill_3_Image;
    [SerializeField] private Image skill_3_Selection;
    [SerializeField] private TextMeshProUGUI skill_3_TextCoolDown;
    [SerializeField] private bool isSkill_3_UIActive;
    [SerializeField] private bool isSkill_3_CoolDown = false;
    [SerializeField] private float skill_3_MaxCoolDown;
    [SerializeField] private float skill_3_CoolDownTime;
    [SerializeField] private float skill_3_MaxDistance;
    private Vector3 skill_3_Pos;
    private Vector3 positionSkill;
    private RaycastHit hit;
    public bool IsSkill_1_UIActive => isSkill_1_UIActive;
    public Vector3 PositionSkill => positionSkill;
    public Vector3 Skill_3_Pos => skill_3_Pos;

    [Header("MobileMode")]
    [SerializeField] private FixedJoystick joystick_Skill_1;


    private void Start()
    {
        Skill_1_UIActive(false);
        isSkill_1_CoolDown = false;
        skill_1_Image.fillAmount = 0;
        skill_1_TextCoolDown.gameObject.SetActive(false);


        Skill_2_UIActive(false);
        isSkill_2_CoolDown = false;
        skill_2_Image.fillAmount = 0;
        skill_2_TextCoolDown.gameObject.SetActive(false);

        Skill_3_UIActive(false);
        isSkill_3_CoolDown = false;
        skill_3_Image.fillAmount = 0;
        skill_3_TextCoolDown.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player.IsMobileMode == false)
        {

            Skill_1_UI();


            Skill_2_UI();

            Skill_3_UI();

        }
        Skill_CoolDownUI(ref isSkill_1_CoolDown, ref skill_1_CoolDownTime, skill_1_MaxCoolDown, skill_1_Image, skill_1_TextCoolDown);
        Skill_CoolDownUI(ref isSkill_2_CoolDown, ref skill_2_CoolDownTime, skill_2_MaxCoolDown, skill_2_Image, skill_2_TextCoolDown);
        Skill_CoolDownUI(ref isSkill_3_CoolDown, ref skill_3_CoolDownTime, skill_3_MaxCoolDown, skill_3_Image, skill_3_TextCoolDown);
        if (player.IsMobileMode == true)
        {
            Skill_1_UIMobile();
        }

    }
    private void Skill_CoolDownUI(ref bool isSkill_CoolDown, ref float skill_CoolDownTime, float skill_MaxCoolDown, Image skill_Image, TextMeshProUGUI skill_TextCoolDown)
    {
        if (isSkill_CoolDown == true)
        {
            skill_CoolDownTime -= Time.deltaTime;
            skill_Image.fillAmount = skill_CoolDownTime / skill_MaxCoolDown;
            skill_TextCoolDown.text = (Mathf.Floor(skill_CoolDownTime * 10) / 10).ToString();

            if (skill_CoolDownTime <= 0)
            {
                skill_CoolDownTime = 0;
                skill_Image.fillAmount = 0;
                skill_TextCoolDown.gameObject.SetActive(false);
                isSkill_CoolDown = false;
            }
        }
    }

    public void Skill_1_UI()
    {
        if (isSkill_1_CoolDown == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q) && isSkill_1_UIActive == false && player.CheckSkillCast() == false)
        {
            Skill_1_UIActive(true);
            Skill_2_UIActive(false);
            Skill_3_UIActive(false);
        }
        if (isSkill_1_UIActive == true)
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
            {
                positionSkill = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            }
            Quaternion qua = Quaternion.LookRotation(positionSkill - transform.position);

            qua.eulerAngles = new Vector3(0, qua.eulerAngles.y, qua.eulerAngles.z);

            skill_1_Canvas.transform.rotation = Quaternion.Lerp(qua, skill_1_Canvas.transform.rotation, 0);

            if (Input.GetMouseButtonDown(0))
            {
                Skill_1_UIActive(false);
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
        if (Input.GetKeyDown(KeyCode.W) && isSkill_2_UIActive == false && player.CheckSkillCast() == false)
        {
            Skill_2_UIActive(true);
            Skill_1_UIActive(false);
            Skill_3_UIActive(false);
        }
        if (isSkill_2_UIActive == true)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Skill_2_UIActive(false);
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

    public void Skill_3_UI()
    {
        if (isSkill_3_CoolDown == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E) && isSkill_3_UIActive == false && player.CheckSkillCast() == false)
        {
            Skill_3_UIActive(true);
            Skill_1_UIActive(false);
            Skill_2_UIActive(false);
        }
        if (isSkill_3_UIActive == true)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
            {
                positionSkill = new Vector3(hit.point.x, skill_3_Selection.transform.position.y, hit.point.z);
            }

            Vector3 hitPosDir = (positionSkill - transform.position).normalized;
            float distance = Vector3.Distance(positionSkill, transform.position);
            distance = Mathf.Min(distance, skill_3_MaxDistance);

            skill_3_Pos = transform.position + hitPosDir * distance;
            skill_3_Selection.transform.position = skill_3_Pos;

            if (Input.GetMouseButtonDown(0))
            {
                Skill_3_UIActive(false);
                player.ChangeState(new PlayerSkill_3_State());
                player.isSkill_3_Casting = true;
                isSkill_3_CoolDown = true;
                skill_3_Image.fillAmount = 1;
                skill_3_CoolDownTime = skill_3_MaxCoolDown;
                skill_3_TextCoolDown.gameObject.SetActive(true);
                skill_3_TextCoolDown.text = skill_3_MaxCoolDown.ToString();
            }
        }
    }

    public void Skill_1_UIActive(bool isActive)
    {
        if (isActive == true)
        {
            skill_1_Canvas.gameObject.SetActive(true);
            isSkill_1_UIActive = true;
        }
        else
        {
            skill_1_Canvas.gameObject.SetActive(false);
            isSkill_1_UIActive = false;

        }
    }
    public void Skill_2_UIActive(bool isActive)
    {
        if (isActive == true)
        {
            skill_2_Canvas.gameObject.SetActive(true);
            isSkill_2_UIActive = true;
        }
        else
        {
            skill_2_Canvas.gameObject.SetActive(false);
            isSkill_2_UIActive = false;
        }
    }
    public void Skill_3_UIActive(bool isActive)
    {
        if (isActive == true)
        {
            skill_3_Canvas.gameObject.SetActive(true);
            isSkill_3_UIActive = true;
        }
        else
        {
            skill_3_Canvas.gameObject.SetActive(false);
            isSkill_3_UIActive = false;
        }

    }

    /// <summary>
    /// ////////////////////////
    /// </summary>
    private void Skill_1_UIMobile()
    {
        if (isSkill_1_CoolDown == true)
        {
            return;
        }
        if (joystick_Skill_1.GetComponent<JoystickCheckInput>().isTouching == true && isSkill_1_UIActive == false && player.CheckSkillCast() == false)
        {
            Skill_1_UIActive(true);
            Skill_2_UIActive(false);
            Skill_3_UIActive(false);
        }
        if (isSkill_1_UIActive == true)
        {
            positionSkill = new Vector3(joystick_Skill_1.Horizontal, 0, joystick_Skill_1.Vertical).normalized;


            Quaternion qua = Quaternion.LookRotation(positionSkill - skill_1_Canvas.transform.localPosition);

            qua.eulerAngles = new Vector3(0, qua.eulerAngles.y, qua.eulerAngles.z);

            skill_1_Canvas.transform.rotation = Quaternion.Lerp(qua, skill_1_Canvas.transform.rotation, 0);

            if (joystick_Skill_1.GetComponent<JoystickCheckInput>().isTouching == false)
            {
                positionSkill = joystick_Skill_1.GetComponent<JoystickCheckInput>().dir;
                Skill_1_UIActive(false);
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
}
