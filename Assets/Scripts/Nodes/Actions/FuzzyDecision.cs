using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Fuzzy")]
public class FuzzyDecision : ActionNode
{
    public float fleeDistance = 15f;
    public float sampleRadius = 5f;

    public float strafeRadius = 6f;
    public float minStrafeRadius = 4f;
    public float maxStrafeRadius = 8f;

    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;

    private AICharacterActionLandUnicorn _actionUnicorn;
    private AICharacterVehicleLandUnicorn _vehicleUnicorn;

    private enum Estado { Huir, Cautela, Atacar }
    private Estado _estadoActual;
    private NavMeshAgent _agent;

    // Flee
    private bool _fleeTargetSet;

    // Cautela (strafe)
    private float _strafeSign;
    private float _switchTimer;
    private Vector3 _strafeTarget;

    // Atacar
    private float _attackTimer;
    private bool _persiguiendo;

    public override void OnAwake()
    {
        base.OnAwake();
        _actionUnicorn = GetComponent<AICharacterActionLandUnicorn>();
        _vehicleUnicorn = GetComponent<AICharacterVehicleLandUnicorn>();
    }

    public override void OnStart()
    {
        if (_vehicleUnicorn != null)
            _agent = _vehicleUnicorn.Agent;

        _estadoActual = GetEstadoFromTendencia(
            _actionUnicorn != null ? _actionUnicorn.GetTendenciaActual() : 50f);
        IniciarEstado(_estadoActual);
    }

    public override void OnEnd()
    {
        if (_agent != null && !_agent.updateRotation)
            _agent.updateRotation = true;
    }

    public override TaskStatus OnUpdate()
    {
        Transform enemy = _AIEyeNPC != null && _AIEyeNPC.Enemy != null
            ? _AIEyeNPC.Enemy
            : (_Blackboard != null ? _Blackboard.enemyTarget : null);

        if (enemy == null || _actionUnicorn == null) return TaskStatus.Failure;

        float tendency = _actionUnicorn.GetTendenciaActual();
        Estado nuevoEstado = GetEstadoFromTendencia(tendency);

        if (nuevoEstado != _estadoActual)
        {
            FinalizarEstado(_estadoActual);
            _estadoActual = nuevoEstado;
            IniciarEstado(_estadoActual);
        }

        switch (_estadoActual)
        {
            case Estado.Huir: return EjecutarHuir(enemy);
            case Estado.Cautela: return EjecutarCautela(enemy);
            case Estado.Atacar: return EjecutarAtacar(enemy);
            default: return TaskStatus.Failure;
        }
    }

    private Estado GetEstadoFromTendencia(float tendency)
    {
        if (tendency < 40f) return Estado.Huir;
        if (tendency <= 60f) return Estado.Cautela;
        return Estado.Atacar;
    }

    private void IniciarEstado(Estado estado)
    {
        _fleeTargetSet = false;
        _attackTimer = 0f;
        _persiguiendo = false;

        switch (estado)
        {
            case Estado.Cautela:
                _strafeSign = Random.value > 0.5f ? 1f : -1f;
                _switchTimer = Random.Range(1.5f, 3f);
                _strafeTarget = Vector3.zero;
                if (_agent != null)
                    _agent.updateRotation = false;
                break;

            case Estado.Huir:
            case Estado.Atacar:
                if (_agent != null)
                {
                    _agent.updateRotation = true;
                    if (_vehicleUnicorn != null)
                        _vehicleUnicorn.ResetSpeed();
                }
                break;
        }
    }

    private void FinalizarEstado(Estado estado)
    {
        if (estado == Estado.Cautela && _agent != null)
            _agent.updateRotation = true;
    }

    private TaskStatus EjecutarHuir(Transform enemy)
    {
        if (_vehicleUnicorn == null) return TaskStatus.Failure;

        if (!_fleeTargetSet)
        {
            Vector3 dir = (transform.position - enemy.position).normalized;
            Vector3 desiredPos = transform.position + dir * fleeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredPos, out hit, sampleRadius, NavMesh.AllAreas))
            {
                _fleeTargetSet = true;
                _vehicleUnicorn.MoveTo(hit.position);
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        if (HasReachedFlee()) return TaskStatus.Success;
        return TaskStatus.Running;
    }

    private bool HasReachedFlee()
    {
        if (_agent == null) return false;
        if (_agent.pathPending) return false;
        if (_agent.remainingDistance > _agent.stoppingDistance) return false;
        return true;
    }

    private TaskStatus EjecutarCautela(Transform enemy)
    {
        if (_vehicleUnicorn == null) return TaskStatus.Failure;

        Vector3 toEnemy = transform.position - enemy.position;
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

            Vector3 strafeDir = Vector3.Cross(toEnemyDir, Vector3.up) * _strafeSign;
            float radius = Random.Range(minStrafeRadius, maxStrafeRadius);
            _strafeTarget = enemy.position + strafeDir * radius;
        }

        _vehicleUnicorn.MoveCautiously(_strafeTarget);

        if (_agent != null)
        {
            Vector3 lookDir = enemy.position - transform.position;
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

    private TaskStatus EjecutarAtacar(Transform enemy)
    {
        if (_vehicleUnicorn == null || _AICombatLand == null) return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, enemy.position);

        if (dist > attackRange)
        {
            _persiguiendo = true;
            _vehicleUnicorn.MoveTo(enemy.position);
            return TaskStatus.Running;
        }

        _persiguiendo = false;
        _vehicleUnicorn.StopMoving();

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            _AICombatLand.Attack();
            _attackTimer = attackCooldown;
        }

        return TaskStatus.Running;
    }
}
