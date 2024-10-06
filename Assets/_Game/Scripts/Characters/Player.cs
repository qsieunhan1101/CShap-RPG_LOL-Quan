using UnityEngine;

public class Player : Character
{
    protected IState<Player> currentState;


    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Transform enemyTarget;
    [SerializeField] private Collider[] enemyCollider;
    [SerializeField] private string currentNameState;

    [Header("Nomal Attack")]
    [SerializeField] private float nomalHitBoxRadius;
    [SerializeField] private float nomalHitBoxPos;

    [Header("Skill_1")]
    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI => playerUI;

    [SerializeField] private ParticleSystem skill_1_Particle;
    [SerializeField] private GameObject skill_1_Boxtest;
    [SerializeField] private Collider[] skill_1_colliders;


    private Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;
    public Transform EnemyTarget => enemyTarget;



    private void Start()
    {
        ChangeState(new PlayerIdleState());
        enemyCollider = new Collider[10];
        skill_1_colliders = new Collider[10];
    }
    private void Update()
    {
        MoveByClick();
        //AttackByClick();

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        playerUI.Skill_1_UI();



        if (Input.GetKeyDown(KeyCode.L))
        {
            skill_1_Particle.Play();

            anim.ResetTrigger("Skill_1");
            anim.SetTrigger("Skill_1");

            int skill_1_Colli = Physics.OverlapBoxNonAlloc(skill_1_Boxtest.transform.position, skill_1_Boxtest.transform.localScale / 2, skill_1_colliders, skill_1_Boxtest.transform.localRotation);

            for (int i = 0; i < skill_1_Colli; i++)
            {
                if (skill_1_colliders[i].CompareTag(Constant.TAG_ENEMY))
                {
                    Debug.Log(skill_1_colliders[i].gameObject.name);

                }
            }
            for (int i = 0; i < skill_1_Colli; i++)
            {
                skill_1_colliders[i] = null;
            }
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

    public void MoveByClick()
    {
        if (Input.GetMouseButton(1))
        {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(Constant.Tag_GROUND))
                {
                    targetPosition = hit.point;
                    ChangeState(new PlayerRunState());
                }
                if (hit.collider.CompareTag(Constant.TAG_ENEMY))
                {
                    enemyTarget = hit.transform;
                    targetPosition = hit.point;
                    ChangeState(new PlayerAttackState());
                }
            }
        }
    }

    public void AttackByClick()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(Constant.TAG_ENEMY))
                {
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
            LookRotation(enemyTarget.transform.position);
        }
        for (int i = 0; i < numberHitEnemy; i++)
        {
            if (enemyCollider[i] != null && enemyCollider[i].transform != this.transform)
            {
                Debug.Log(enemyCollider[i].name + "Slash");
                enemyCollider[i].GetComponent<Character>().TakeDamage(10);
            }
        }

    }


    public void Skill_1_Cast()
    {
        StopMove();
        transform.rotation = Quaternion.LookRotation((playerUI.posSkill_1 - transform.position).normalized);
        ChangeAnim(Constant.ANIM_SKILL_1);
        skill_1_Particle.Play();


    }
    public void Skill_1_TakeDamage()
    {
        int skill_1_Colli = Physics.OverlapBoxNonAlloc(skill_1_Boxtest.transform.position, skill_1_Boxtest.transform.localScale / 2, skill_1_colliders, skill_1_Boxtest.transform.localRotation);

        for (int i = 0; i < skill_1_Colli; i++)
        {
            if (skill_1_colliders[i].CompareTag(Constant.TAG_ENEMY))
            {
                Debug.Log(skill_1_colliders[i].gameObject.name);
                skill_1_colliders[i].GetComponent<Enemy>().TakeDamage(20);
            }
        }
        for (int i = 0; i < skill_1_Colli; i++)
        {
            skill_1_colliders[i] = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 sphereHitBoxPos = transform.position + transform.forward * nomalHitBoxPos + transform.up;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereHitBoxPos, nomalHitBoxRadius);

        //Gizmos.DrawWireCube(cube.transform.position, cube.transform.localScale);


    }
}
