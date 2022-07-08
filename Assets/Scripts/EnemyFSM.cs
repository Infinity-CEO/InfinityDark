using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    EnemyState m_State;

    public float findDistance = 60f;

    Transform player;

    public float attackDistance = 2f;

    public float moveSpeed = 7f;

    CharacterController cc;

    float currentTime = 0;

    public float attackDelay = 2f;

    public int attackPower = 8;

    Vector3 originPos;

    Quaternion originRot;

    public float moveDistance = 100f;

    public float hp = 100;

    float maxHp = 100;

    //private TakeDamage takeDmg;

    public Slider hpSlider;

    Animator anim;

    NavMeshAgent smith;
    public void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        anim = transform.GetComponentInChildren<Animator>();

        smith = GetComponent<NavMeshAgent>();
    }
    public void Update()
    {
        if(GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        switch(m_State)
        {
            case EnemyState.Idle:
            Idle();
            break;

            case EnemyState.Move:
            Move();
            break;

            case EnemyState.Attack:
            Attack();
            break;

            case EnemyState.Return:
            Return();
            break;

            case EnemyState.Damaged:
            //Damaged();
            break;

            case EnemyState.Die:
            //Die();
            break;
        }
        hpSlider.value = (float)hp / (float)maxHp;
        
    }
    public void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : 대기>이동");

            anim.SetTrigger("IdleToMove");
        }
    }
    public void Move()
    {
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("상태 전환 : 이동>복귀");
        }
        else if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //Vector3 dir = (player.position - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;

            smith.isStopped = true;

            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : 이동>공격");

            currentTime = attackDelay;

            anim.SetTrigger("MoveToAttackDelay");
        }
    }
    public void Attack()
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                //player.GetComponent<Controller>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;

                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : 공격>이동");
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }
    public void Return()
    {
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;

            smith.destination = originPos;

            smith.stoppingDistance = 0;
        }
        else
        {
            smith.isStopped = true;

            smith.ResetPath();
            
            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("상태 전환 : 복귀>대기");

            anim.SetTrigger("MoveToIdle");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<Controller>().DamageAction(attackPower);
    }

    public void HitEnemy(float attackpower)
    {
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        hp -= attackpower;

        smith.isStopped = true;
        smith.ResetPath();

        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : 모든 상태>데미지");

            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환 : 모든 상태>사망");

            anim.SetTrigger("Die");
            Die();
        }
    }
    public void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.1f);

        m_State = EnemyState.Move;
        print("상태 전환 : 데미지>이동");
    }
    public void Die()
    {
        if(OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }
        GameObject kmObject = GameObject.Find("KMG");
        QuestManager km = kmObject.GetComponent<QuestManager>();
        km.SetScore(km.GetScore() + 1);

        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;
        smith.enabled = false;
        yield return new WaitForSeconds(5.0f);
        print("소멸!");
        Destroy(gameObject);
    }
}
