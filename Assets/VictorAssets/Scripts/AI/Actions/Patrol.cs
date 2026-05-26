using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace oldStructureVictorGozu
{
    [TaskCategory("MY_AI / Move")]
    public class Patrol : Action
    {
        private MovementController movement;

        public float patrolRadius = 10f;

        private Vector3 randomPoint;

        public override void OnStart()
        {
            movement = GetComponent<MovementController>();

            GenerateNewPoint();
        }

        public override TaskStatus OnUpdate()
        {
            if (movement.HasReachedDestination())
            {
                GenerateNewPoint();
            }

            return TaskStatus.Running;
        }

        void GenerateNewPoint()
        {
            Vector3 randomDirection =
                Random.insideUnitSphere * patrolRadius;

            randomDirection += transform.position;

            randomDirection.y = transform.position.y;

            randomPoint = randomDirection;

            movement.MoveTo(randomPoint);
        }
    }
}