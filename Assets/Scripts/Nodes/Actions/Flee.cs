// Flee.cs
// Delega la huida al metodo virtual Flee() de AICharacterVehicleLand,
// permitiendo que cada NPC (Unicornio, Hada, etc.) sobrescriba su comportamiento.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class Flee : ActionNode
{
    public float fleeDistance = 15f;
    public float sampleRadius = 5f;

    private bool _fleeTargetSet;

    public override void OnStart()
    {
        _fleeTargetSet = false;

        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.enemyTarget != null)
            enemy = _Blackboard.enemyTarget;

        if (enemy == null || _AIVehicleLand == null) return;

        _fleeTargetSet = _AIVehicleLand.Flee(enemy, fleeDistance, sampleRadius);
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || !_fleeTargetSet) return TaskStatus.Failure;
        if (_AIVehicleLand.HasReachedDestination()) return TaskStatus.Failure;
        return TaskStatus.Running;
    }
}
