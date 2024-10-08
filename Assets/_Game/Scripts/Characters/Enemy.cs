using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    protected IState<Enemy> currentState;

    [SerializeField] private string currentNameState;
    [SerializeField] private Transform mapTransform;
    [SerializeField] private float radiusMap;
    [SerializeField] private float radiusEnemy;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private bool isAttack = false;

    public static Action emenyDeadEvent;
    private void Start()
    {
        ChangState(new EnemyPatrolState());
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
            emenyDeadEvent?.Invoke();
            ChangState(new EnemyDeadState());
        }


        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        CheckPlayer();

        if (enemyTarget != null && isAttack == false)
        {
            isAttack = true;
            ChangState(new EnemyAttackState());
        }
    }
    public void ChangState(IState<Enemy> newState)
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
    public void NomalAttack()
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<Player>().TakeDamage(nomalAttackDamage);
        }
    }

    public Vector3 RandomPositionInMap()
    {
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * radiusMap + mapTransform.position;
        NavMeshHit navHit;
        bool hasHit = NavMesh.SamplePosition(randomPosition, out navHit, 1.0f, NavMesh.AllAreas);
        if (hasHit == true)
        {
            return navHit.position;
        }
        return RandomPositionInMap();
    }

    private void CheckPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusEnemy, playerMask);
        if (colliders.Length != 0)
        {
       
            enemyTarget = colliders[0].transform;
            if (enemyTarget.GetComponent<Character>().IsDead == true)
            {
                enemyTarget = null;
            }
            
        }
        else
        {
            enemyTarget = null;
        }

    }

    public void DestroyEvent()
    {
        Destroy(gameObject, 2f);
    }
}
