using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected string currentAnimName;
    [SerializeField] protected Animator anim;
    [SerializeField] private List<Avatar> listAvatar;


    [SerializeField] protected float maxHealth;
    [SerializeField] protected float minHealth;
    [SerializeField] protected float health;
    [SerializeField] protected Slider healthBar;

    [SerializeField] protected float normalCoolDownAttack = 2f;
    public float NormalCoolDownAttack => normalCoolDownAttack;
    public NavMeshAgent Agent => agent;

    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;
    private void Awake()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.minValue = minHealth;
        healthBar.value = maxHealth;
    }
    private void Update()
    {
    }


    public void MoveToPosition(Vector3 position)
    {
        ChangeAnim(Constant.ANIM_RUN);
        agent.stoppingDistance = 0;
        agent.SetDestination(position);
        LookRotation(position);

    }

    public void MoveTowardTarget(Vector3 position)
    {
        ChangeAnim(Constant.ANIM_RUN);
        LookRotation(position);
        agent.stoppingDistance = 2f;
        agent.SetDestination(position);
    }
    public void StopMove()
    {
        agent.SetDestination(transform.position);
        agent.stoppingDistance = 0;
        agent.ResetPath();
        ChangeAnim(Constant.ANIM_IDLE);
    }

    public void ChangeAnim(string newAnimName)
    {
        if (currentAnimName != newAnimName)
        {
            anim.ResetTrigger(newAnimName);

            currentAnimName = newAnimName;
            for (int i = 0; i < listAvatar.Count; i++)
            {
                if (listAvatar[i].name == $"{currentAnimName}Avatar")
                {
                    anim.avatar = listAvatar[i];

                }
            }
            anim.SetTrigger(currentAnimName);
        }
    }
    public void LookRotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;
    }
    public bool IsFinishMove()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
}
