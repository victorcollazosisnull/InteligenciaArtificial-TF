// MoveToSleepPoint.cs
// FIX: usa SharedTransform en vez de Transform publico.
// SharedTransform permite asignar el punto de la cueva desde
// las variables del BehaviorTree en el GameObject (no en el asset),
// evitando el error de referencias de escena en assets reutilizables.
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToSleepPoint : ActionNode
{
    // Variable Shared del BehaviorTree apuntando al punto de la cueva.
    public SharedTransform sleepPoint;

    public float stopDistance = 1f;
    private bool _isSleeping;

    public override void OnStart()
    {
        _isSleeping = false;
        if (sleepPoint.Value != null && _AIVehicleLand != null)
            _AIVehicleLand.MoveTo(sleepPoint.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (sleepPoint.Value == null || _AIVehicleLand == null) return TaskStatus.Failure;

        if (!_isSleeping)
        {
            float dist = Vector3.Distance(transform.position, sleepPoint.Value.position);
            if (dist <= stopDistance)
            {
                _AIVehicleLand.StopMoving();
                _isSleeping = true;
                Debug.Log($"[{gameObject.name}] llegó a la cueva, durmiendo.");
            }
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        Debug.Log($"[{gameObject.name}] despertó.");
    }
}