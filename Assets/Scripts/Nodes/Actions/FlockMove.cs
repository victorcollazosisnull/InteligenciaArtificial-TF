using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class FlockMove : ActionNode
{
    public float neighborRadius = 3f;
    [Range(0, 3)] public float separationWeight = 1.5f;
    [Range(0, 3)] public float alignmentWeight = 1.0f;
    [Range(0, 3)] public float cohesionWeight = 1.0f;

    private FlockSystemPato _flockSystem;

    public override void OnAwake()
    {
        base.OnAwake();
        _flockSystem = GetComponent<FlockSystemPato>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || _flockSystem == null)
            return TaskStatus.Failure;

        Vector3 target = _flockSystem.GetFlockMovePosition();

        float dist = Vector3.Distance(transform.position, target);
        if (dist <= 0.5f)
        {
            _AIVehicleLand.StopMoving();
            return TaskStatus.Success;
        }
        _AIVehicleLand.MoveTo(target);
        return TaskStatus.Running;
    }
}
