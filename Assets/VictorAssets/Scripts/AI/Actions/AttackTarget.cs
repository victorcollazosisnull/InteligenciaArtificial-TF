using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AttackTarget : Action
{
    public float damage = 10f;

    public float attackCooldown = 1.5f;

    private float timer;

    private VisionSensor vision;

    public override void OnAwake()
    {
        vision = GetComponent<VisionSensor>();
    }

    public override void OnStart()
    {
        timer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (vision.ViewPlayer == null)
        {
            return TaskStatus.Failure;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Health health = vision.ViewPlayer.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);

                Debug.Log("Orco atacó a " + vision.ViewPlayer.name);
            }

            timer = attackCooldown;
        }

        return TaskStatus.Running;
    }
}