using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    protected IState<Player> currentState;


    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Collider[] enemyCollider;
    [SerializeField] private string currentNameState;
    [SerializeField] private GameObject canvas2d;
    [Header("Nomal Attack")]
    [SerializeField] private float nomalHitBoxRadius;
    [SerializeField] private float nomalHitBoxPos;

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
    public bool isSkill_2_Casting = false;

    public PlayerUI PlayerUI => playerUI;



    private Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;

  
    private void Start()
    {
        ChangeState(new PlayerIdleState());
        enemyCollider = new Collider[10];
        skill_1_colliders = new Collider[10];
        
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




        if (Input.GetKeyDown(KeyCode.L))
        {
            skill_2_Particle.Play();
            anim.ResetTrigger(currentAnimName);
            anim.SetTrigger("Skill_2");
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
        if (isSkill_1_Casting == true || isDead == true)
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
                }
                if (hit.collider.CompareTag(Constant.TAG_ENEMY))
                {
                    Debug.Log(hit.collider.name);
                    if (enemyTarget == hit.transform)
                    {
                        return;
                    }
                    enemyTarget = hit.transform;
                    targetPosition = hit.point;

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
        int numberHitEnemy = Physics.OverlapSphereNonAlloc(sphereHitBoxPos, nomalHitBoxRadius, enemyCollider);
        //transform.rotation = Quaternion.LookRotation((enemyTarget.position - transform.position).normalized);
        if (enemyTarget != null)
        {
            //LookRotation(enemyTarget.transform.position);
            SetRotation(enemyTarget.transform.position);
        }
        for (int i = 0; i < numberHitEnemy; i++)
        {
            if (enemyCollider[i] != null && enemyCollider[i].transform != this.transform)
            {
                Debug.Log(enemyCollider[i].name + "Slash");
                enemyCollider[i].GetComponent<Character>().TakeDamage(nomalAttackDamage);
            }
        }

    }

    //Skill_1========================================
    public void Skill_1_Cast()
    {
        StopMove();
        //transform.rotation = Quaternion.LookRotation((playerUI.posSkill_1 - transform.position).normalized);
        SetRotation(playerUI.PosSkill_1);
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
                Debug.Log(skill_1_colliders[i].gameObject.name);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skill_2_Radius);
    }
}
