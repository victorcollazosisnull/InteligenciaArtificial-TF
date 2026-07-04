using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("BosqueEncantado/Condition")]
public class IsAlarmActive : ConditionNode
{
    public override TaskStatus OnUpdate()
    {
        AlarmSystemPato alarm = GetComponent<AlarmSystemPato>();
        if (alarm == null) return TaskStatus.Failure;
        return alarm.IsAlarmed ? TaskStatus.Success : TaskStatus.Failure;
    }
}
