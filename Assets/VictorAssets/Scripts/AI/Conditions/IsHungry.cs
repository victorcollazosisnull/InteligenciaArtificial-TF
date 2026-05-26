using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace oldStructureVictorGozu
{
    [TaskCategory("MY_AI / Food")]
    public class IsHungry : Conditional
    {
        private Blackboard blackboard;

        public override void OnAwake()
        {
            blackboard = GetComponent<Blackboard>();
        }

        public override TaskStatus OnUpdate()
        {
            return blackboard.isHungry ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}