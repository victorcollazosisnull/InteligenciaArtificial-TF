using UnityEngine;
using UnityEngine.AI;
namespace oldStructureVictorGozu
{
    public class MovementController : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 targetPosition)
        {
            if (agent == null || !agent.isOnNavMesh)
                return;

            agent.SetDestination(targetPosition);
        }

        public void StopMoving()
        {
            if (agent == null)
                return;

            agent.ResetPath();
        }

        public bool HasReachedDestination()
        {
            if (agent.pathPending)
                return false;

            return agent.remainingDistance <= agent.stoppingDistance;
        }
    }
}