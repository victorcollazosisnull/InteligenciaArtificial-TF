// Patrol.cs
// FIX: eliminado StopMoving() de OnStart().
// Llamar StopMoving() antes de GenerateNewPoint() causaba
// un frame de idle visible antes de empezar a moverse.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Patrol : ActionNode
{
    public override void OnStart()
    {
        _AIVehicleLand.GenerateNewPoint(); 
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null) return TaskStatus.Failure;
        if (_AIVehicleLand.HasReachedDestination())
            _AIVehicleLand.GenerateNewPoint();
        return TaskStatus.Running;
    }   
}
