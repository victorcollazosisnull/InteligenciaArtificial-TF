using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace oldStructureVictorGozu
{
    [TaskCategory("MY_AI / Move")]
    public class MoveToLastKnownPosition : Action
    {
        private Blackboard blackboard;
        private MovementController movement;

        private float searchTimer;
        public float searchDuration = 2f;

        private bool reachedPosition;

        public override void OnAwake()
        {
            blackboard = GetComponent<Blackboard>();
            movement = GetComponent<MovementController>();
        }

        public override void OnStart()
        {
            reachedPosition = false;
            searchTimer = 0f;

            if (blackboard.hasLastKnownPosition)
            {
                movement.MoveTo(blackboard.lastKnownPosition);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!blackboard.hasLastKnownPosition)
            {
                return TaskStatus.Failure;
            }

            if (!reachedPosition)
            {
                if (movement.HasReachedDestination())
                {
                    reachedPosition = true;
                }

                return TaskStatus.Running;
            }

            searchTimer += Time.deltaTime;

            if (searchTimer >= searchDuration)
            {
                blackboard.hasLastKnownPosition = false;

                return TaskStatus.Failure;
            }

            return TaskStatus.Running;
        }
    }
}