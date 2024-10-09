using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    protected IState<Player> currentState;


    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Collider[] enemyColliders;
    [SerializeField] private string currentNameState;
    [SerializeField] private GameObject canvas2d;
    [SerializeField] private HightLightManager hightLightManager;
    [Header("Nomal Attack")]
    [SerializeField] private float nomalHitBoxRadius;
    [SerializeField] private float nomalHitBoxPos;
    private Vector3 targetPosition;

    [Header("Skill_1")]
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private ParticleSystem skill_1_Particle;
    [SerializeField] private GameObject skill_1_Boxtest;
    [SerializeField] private Collider[] skill_1_colliders;
    [SerializeField] private float skill_1_Damage;
    public bool isSkill_1_Casting = false;

    [Header("Skill_2")]
    [SerializeField] private ParticleSystem skill_2_Particle;
    [SerializeField] private float skill_2_Damage;
    [SerializeField] private float skill_2_Radius;
    [SerializeField] private float skill_2_HealHp;
    public bool isSkill_2_Casting = false;

    [Header("Skill_3")]
    [SerializeField] private GameObject skill_3_ParticlePrefab;
    [SerializeField] private float skill_3_Damage;
    [SerializeField] private float skill_3_Radius;
    public bool isSkill_3_Casting = false;



    [Header("Mobile")]
    [SerializeField] private bool isMobileMode = false;
    [SerializeField] private FixedJoystick joystick_Move;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float nomalAttackMobileRadius;
    [SerializeField] private Button btnAttack;
    [SerializeField] private Collider[] mobileEnemyColliders;
    [SerializeField] private JoystickCheckInput joystickCheckInput;

    public bool isAttack = false;


    public PlayerUI PlayerUI => playerUI;
    public Vector3 TargetPosition => targetPosition;

    public bool IsMobileMode => isMobileMode;

    private void Start()
    {
        ChangeState(new PlayerIdleState());
        enemyColliders = new Collider[10];
        skill_1_colliders = new Collider[10];

        isSkill_1_Casting = false;
        isSkill_2_Casting = false;
        isSkill_3_Casting = false;

        if (isMobileMode == true)
        {
            rb.isKinematic = false;
            btnAttack.onClick.AddListener(AttackMobieMode);
            mobileEnemyColliders = new Collider[10];
            ChangeState(new PlayerNullState());
        }
    }
    private void OnEnable()
    {
        GameManager.openCanvas2dEvent += OpenCanvas2D;
        GameManager.closeCanvas2dEvent += CloseCanvas2D;
    }
    private void OnDisable()
    {
        GameManager.openCanvas2dEvent -= OpenCanvas2D;
        GameManager.closeCanvas2dEvent -= CloseCanvas2D;
    }
    private void Update()
    {
        if (isDead == true)
        {
            return;
        }
        if (health <= 0 && isDead == false)
        {
            isDead = true;
            ChangeState(new PlayerDeadState());
        }

        MoveOrAttackByClick();

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        if (isMobileMode == true)
        {


            MoveByJoystick();

        }

    }


    public void ChangeState(IState<Player> newState)
    {
        currentNameState = newState.ToString();
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    protected void MoveOrAttackByClick()
    {
        if (CheckSkillCast() == true || isDead == true)
        {
            return;
        }
        if (Input.GetMouseButton(1))
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(Constant.TAG_GROUND))
                {
                    targetPosition = hit.point;
                    enemyTarget = null;
                    ChangeState(new PlayerRunState());
                    hightLightManager.DeselectHighLight();
                }
                if (hit.collider.CompareTag(Constant.TAG_ENEMY))
                {
                    if (enemyTarget == hit.transform)
                    {
                        return;
                    }
                    enemyTarget = hit.transform;
                    targetPosition = hit.point;
                    hightLightManager.SelectedHighLight();
                    ChangeState(new PlayerAttackState());
                }
            }
        }
    }

    /// <summary>
    /// Call by Animaton Slash
    /// </summary>
    public void NomarAttack()
    {
        Vector3 sphereHitBoxPos = transform.position + transform.forward * nomalHitBoxPos + transform.up;
        int numberHitEnemy = Physics.OverlapSphereNonAlloc(sphereHitBoxPos, nomalHitBoxRadius, enemyColliders, enemyLayerMask);
        if (enemyTarget != null)
        {
            SetRotation(enemyTarget.transform.position);
        }
        for (int i = 0; i < numberHitEnemy; i++)
        {
            if (enemyColliders[i] != null && enemyColliders[i].transform != this.transform)
            {

                enemyColliders[i].GetComponent<Character>().TakeDamage(nomalAttackDamage);


            }
        }

    }

    //Skill_1========================================
    public void Skill_1_Cast()
    {
        StopMove();
        //transform.rotation = Quaternion.LookRotation((playerUI.posSkill_1 - transform.position).normalized);

        if (isMobileMode == true)
        {
            Vector3 dir = (transform.position + playerUI.PositionSkill);
            SetRotation(dir);

        }
        else
        {
            SetRotation(playerUI.PositionSkill);

        }


        Debug.Log("Look " + playerUI.PositionSkill);
        ChangeAnim(Constant.ANIM_SKILL_1);
        skill_1_Particle.Play();
    }
    /// <summary>
    /// Call by Anim Skill_1
    /// </summary>
    public void Skill_1_TakeDamage()
    {
        int skill_1_Colli = Physics.OverlapBoxNonAlloc(skill_1_Boxtest.transform.position, skill_1_Boxtest.transform.localScale / 2, skill_1_colliders, skill_1_Boxtest.transform.localRotation);

        for (int i = 0; i < skill_1_Colli; i++)
        {
            if (skill_1_colliders[i].CompareTag(Constant.TAG_ENEMY))
            {
                skill_1_colliders[i].GetComponent<Enemy>().TakeDamage(skill_1_Damage);
            }
        }
        for (int i = 0; i < skill_1_Colli; i++)
        {
            skill_1_colliders[i] = null;
        }
        isSkill_1_Casting = false;
    }

    //Skill_2========================================
    public void Skill_2_Cast()
    {
        StopMove();
        ChangeAnim(Constant.ANIM_SKILL_2);
        skill_2_Particle.Play();
    }
    /// <summary>
    /// Call By Anim Skill_2
    /// </summary>
    public void Skill_2_TakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, skill_2_Radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(Constant.TAG_ENEMY))
            {
                collider.GetComponent<Enemy>().TakeDamage(skill_2_Damage);
            }
        }
        isSkill_2_Casting = false;
        Healing(skill_2_HealHp);
    }

    public void Skill_3_Cast()
    {
        StopMove();
        SetRotation(playerUI.Skill_3_Pos);
        ChangeAnim(Constant.ANIM_SKILL_3);
        GameObject particle = Instantiate(skill_3_ParticlePrefab);
        particle.transform.position = playerUI.Skill_3_Pos;

    }
    /// <summary>
    /// Call By Anim Skill_3
    /// </summary>
    public void Skill_3_TakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(playerUI.Skill_3_Pos, skill_3_Radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(Constant.TAG_ENEMY))
            {
                collider.GetComponent<Enemy>().TakeDamage(skill_3_Damage);
            }
        }
        isSkill_3_Casting = false;
    }

    public void ChangeGameStateFail()
    {
        StartCoroutine(ChangeFailState());
    }
    private IEnumerator ChangeFailState()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.ChangeState(GameState.Fail);
    }
    private void OpenCanvas2D()
    {
        canvas2d.gameObject.SetActive(true);
    }
    private void CloseCanvas2D()
    {
        canvas2d.gameObject.SetActive(false);
    }

    // Mobile Mode =====================================================

    private void MoveByJoystick()
    {
        if (isMoving == false)
        {

            if (isAttack == false && CheckSkillCast() == false)
            {
                ChangeAnim(Constant.ANIM_IDLE);
            }
        }
        else if (isMoving == true)
        {
            ChangeAnim(Constant.ANIM_RUN);
            rb.velocity = new Vector3(joystick_Move.Horizontal * speed, rb.velocity.y, joystick_Move.Vertical * speed);
            enemyTarget = null;
            ChangeState(new PlayerNullState());
            isAttack = false;
        }
        if (CheckSkillCast() == false)
        {
            PlayerJoystickRotation();
        }
    }

    private void PlayerJoystickRotation()
    {
        if (joystickCheckInput.isTouching == true)
        {

            moveDirection = new Vector3(joystick_Move.Horizontal, 0, joystick_Move.Vertical);
            isMoving = true;

            transform.rotation = Quaternion.LookRotation(moveDirection);

        }
        else if (joystickCheckInput.isTouching == false)
        {
            isMoving = false;
        }
    }
    private void AttackMobieMode()
    {

        int numberEnemy = Physics.OverlapSphereNonAlloc(transform.position, nomalAttackMobileRadius, mobileEnemyColliders, enemyLayerMask);

        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < numberEnemy; i++)
        {
            float distance = Vector3.Distance(mobileEnemyColliders[i].transform.position, transform.position);
            if (distance < closestDistance && mobileEnemyColliders[i].transform != transform)
            {
                enemyTarget = mobileEnemyColliders[i].transform;
                closestDistance = distance;
            }

        }

        if (enemyTarget != null && isAttack == false)
        {
            ChangeState(new PlayerAttackState());
            isAttack = true;
        }
    }
    public bool CheckSkillCast()
    {
        if (isSkill_1_Casting == true)
        {
            return true;
        }
        if (isSkill_2_Casting == true)
        {
            return true;
        }
        if (isSkill_3_Casting == true)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skill_2_Radius);

        Gizmos.DrawWireSphere(transform.position, nomalAttackMobileRadius);
    }
}
