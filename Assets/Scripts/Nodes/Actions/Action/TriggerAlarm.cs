using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("BosqueEncantado/Action")]
public class TriggerAlarm : ActionNode
{
    public override TaskStatus OnUpdate()
    {
        AlarmSystemPato alarm = GetComponent<AlarmSystemPato>();
        if (alarm == null) return TaskStatus.Failure;

        alarm.TriggerAlarm();
        return TaskStatus.Success;
    }
}
