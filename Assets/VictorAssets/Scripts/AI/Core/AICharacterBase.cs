using UnityEngine;
using UnityEngine.AI;

public class AICharacterBase : MonoBehaviour
{
    [Header("Components")]
    public NavMeshAgent agent;
    public Blackboard blackboard;
    public VisionSensor visionSensor;

    [Header("Faction")]
    public Faction faction;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        blackboard = GetComponent<Blackboard>();
        visionSensor = GetComponent<VisionSensor>();
    }
}