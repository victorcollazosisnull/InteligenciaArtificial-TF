using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Caution : ActionNode
{
    public float strafeRadius = 6f;
    public float minStrafeRadius = 4f;
    public float maxStrafeRadius = 8f;

    private AICharacterVehicleLandUnicorn _unicorn;
    private Transform _enemy;
    private float _strafeSign = 1f;
    private float _switchTimer;
    private Vector3 _strafeTarget;
    private NavMeshAgent _agent;

    public override void OnAwake()
    {
        base.OnAwake();
        _unicorn = GetComponent<AICharacterVehicleLandUnicorn>();
    }

    public override void OnStart()
    {
        _strafeSign = Random.value > 0.5f ? 1f : -1f;
        _switchTimer = Random.Range(1.5f, 3f);
        _strafeTarget = Vector3.zero;

        if (_unicorn != null)
        {
            _agent = _unicorn.Agent;
            if (_agent != null)
                _agent.updateRotation = false;
        }
    }

    public override void OnEnd()
    {
        if (_agent != null)
            _agent.updateRotation = true;
    }

    public override TaskStatus OnUpdate()
    {
        _enemy = _AIEyeNPC != null && _AIEyeNPC.Enemy != null
            ? _AIEyeNPC.Enemy
            : (_Blackboard != null ? _Blackboard.enemyTarget : null);

        if (_enemy == null || _unicorn == null) return TaskStatus.Failure;

        Vector3 toEnemy = transform.position - _enemy.position;
        toEnemy.y = 0f;
        float dist = toEnemy.magnitude;
        if (dist < 0.01f) return TaskStatus.Failure;
        Vector3 toEnemyDir = toEnemy / dist;

        _switchTimer -= Time.deltaTime;
        bool reachedTarget = _strafeTarget == Vector3.zero ||
            Vector3.Distance(transform.position, _strafeTarget) < 2f;

        if (_switchTimer <= 0f || reachedTarget)
        {
            _switchTimer = Random.Range(1.5f, 3.5f);
            _strafeSign *= -1f;
            CalculateStrafeTarget(toEnemyDir);
        }

        _unicorn.MoveCautiously(_strafeTarget);

        if (_agent != null)
        {
            Vector3 lookDir = _enemy.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                _agent.transform.rotation = Quaternion.Slerp(
                    _agent.transform.rotation, targetRot, Time.deltaTime * 5f);
            }
        }

        return TaskStatus.Running;
    }

    private void CalculateStrafeTarget(Vector3 toEnemyDir)
    {
        Vector3 strafeDir = Vector3.Cross(toEnemyDir, Vector3.up) * _strafeSign;
        float radius = Random.Range(minStrafeRadius, maxStrafeRadius);
        _strafeTarget = _enemy.position + strafeDir * radius;
    }
}
