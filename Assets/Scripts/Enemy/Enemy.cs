using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : LivingEntity
{
    public enum EnemyState {FINDTARGET, ATTACK}

    public EnemyState currentState;

    public int minDamage = 1;
    public int maxDamage = 2;
    public float attackDelay = 1f;
    public float detectionRadius = 2f;
    public int coutOfRay = 4;
    public LayerMask targetMask;

    float _myCapsuleRadius;
    Vector3[] _castVectors;
    float _nextTimeAttack;

    Vector3 _destination;
    Vector3 _mainBasePosition;
    NavMeshAgent _agent;

    Coroutine _findTargetCoroutine;
    Coroutine _attackCoroutine;

    Animator _animController;

    DamageTrigger[] _attackTriggers;

    void Awake()
    {
        _mainBasePosition = FindObjectOfType<MainBase>().gameObject.transform.position;

        _agent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<Animator>();

        _myCapsuleRadius = GetComponent<CapsuleCollider>().radius;
    }

    protected override void Start()
    {
        base.Start();

        GenerateCastRays();

        if (_findTargetCoroutine != null)
        {
            StopCoroutine(_findTargetCoroutine);
        }
        _findTargetCoroutine = StartCoroutine(FindTarget());

        _nextTimeAttack = Time.time + attackDelay;

        SetDamageTriggers();
    }

    void SetDamageTriggers()
    {
        _attackTriggers = transform.GetComponentsInChildren<DamageTrigger>();

        for (int i = 0; i < _attackTriggers.Length; i++)
        {
            _attackTriggers[i].minDamage = minDamage;
            _attackTriggers[i].maxDamage = maxDamage;

            _attackTriggers[i].targetMask = targetMask;

            _attackTriggers[i].SetStartProperties();
        }
    }

    void GenerateCastRays()
    {
        _castVectors = new Vector3[coutOfRay];

        for (int i = 0; i < coutOfRay; i++)
        {
            _castVectors[i] = Quaternion.Euler(0f, (360f / coutOfRay) * i, 0f) * Vector3.forward;
        }
    }

    IEnumerator FindTarget()
    {
        currentState = EnemyState.FINDTARGET;
        _animController.SetBool("IsRun", true);

        while (true)
        {
            RaycastHit hit;
            if (CastDetectionRay(out hit))
            {
                LivingEntity entity = hit.collider.GetComponent<LivingEntity>();
                if (entity != null)
                {
                    _destination = hit.collider.transform.position;
                }
            }
            else
            {
                _destination = _mainBasePosition;
            }

            _agent.SetDestination(_destination);

            Debug.DrawRay(transform.position, transform.forward * _myCapsuleRadius * 3, Color.green);
            RaycastHit nearHit;
            if (Physics.Raycast(transform.position, transform.forward, out nearHit, _myCapsuleRadius * 3, targetMask))
            {
                LivingEntity nearEntity = nearHit.collider.GetComponent<LivingEntity>();
                if (nearEntity != null)
                {
                    if (_attackCoroutine != null)
                    {
                        StopCoroutine(_attackCoroutine);
                    }
                    _attackCoroutine = StartCoroutine(Attack());

                    StopCoroutine(_findTargetCoroutine);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void StartAttackTriggers()
    {
        foreach (var attackTrigger in _attackTriggers)
        {
            attackTrigger.StartAttack();
        }
    }

    IEnumerator Attack()
    {
        currentState = EnemyState.ATTACK;
        _agent.Stop();
        _animController.SetBool("IsRun", false);

        while (Physics.Raycast(transform.position, transform.forward, _myCapsuleRadius * 3, targetMask))
        {
            if (_nextTimeAttack < Time.time)
            {
                StartAttackTriggers();

                _animController.SetTrigger("Attack");
                _nextTimeAttack = Time.time + attackDelay;
            }
            yield return null;
        }

        _agent.Resume();

        if (_findTargetCoroutine != null)
        {
            StopCoroutine(_findTargetCoroutine);
        }
        _findTargetCoroutine = StartCoroutine(FindTarget());
    }

    bool CastDetectionRay(out RaycastHit resultHit)
    {
        for (int i = 0; i < coutOfRay; i++)
        {
            Debug.DrawRay(transform.position, _castVectors[i] * detectionRadius, Color.red);
            if (Physics.Raycast(transform.position, _castVectors[i], out resultHit, detectionRadius, targetMask))
            {
                return true;
            }
        }

        resultHit = new RaycastHit();
        return false;
    }

    protected override void Die()
    {
        _animController.SetTrigger("Death");

        if (_findTargetCoroutine != null)
        {
            StopCoroutine(_findTargetCoroutine);
        }

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
      
        GetComponent<CapsuleCollider>().enabled = false;
        _agent.Stop();
        _agent.enabled = false;
    }

    public void EndOfDeath()
    {
        StartCoroutine(AfterDeathMove());
    }

    IEnumerator AfterDeathMove()
    {
        while (transform.position.y > -1f)
        {
            transform.position += Vector3.down * Time.deltaTime / 5f;

            yield return null;
        }

        base.Die();
    }
}
