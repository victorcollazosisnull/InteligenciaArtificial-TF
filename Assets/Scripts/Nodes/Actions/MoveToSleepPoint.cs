// MoveToSleepPoint.cs
// Mueve al Orco a la cueva de noche y activa la animacion de dormir.
// Cuando amanece (Abort Type Both), OnEnd() desactiva la animacion.
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
    private NPCAnimator _npcAnimator;

    public override void OnAwake()
    {
        base.OnAwake();
        _npcAnimator = gameObject.GetComponentInChildren<NPCAnimator>();
    }

    public override void OnStart()
    {
        _isSleeping = false;
        if (sleepPoint.Value != null && _AIVehicleLand != null)
            _AIVehicleLand.MoveTo(sleepPoint.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (sleepPoint.Value == null || _AIVehicleLand == null)
            return TaskStatus.Failure;

        if (!_isSleeping)
        {
            float dist = Vector3.Distance(transform.position, sleepPoint.Value.position);
            if (dist <= stopDistance)
            {
                _AIVehicleLand.StopMoving();
                _isSleeping = true;
                // Activa animacion de dormir
                if (_npcAnimator != null) _npcAnimator.TriggerSleep();
                Debug.Log($"[{gameObject.name}] durmiendo en la cueva.");
            }
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        // Llamado cuando amanece (Abort Type Both interrumpe esta rama)
        if (_npcAnimator != null) _npcAnimator.WakeUp();
        Debug.Log($"[{gameObject.name}] despertó.");
    }
}
