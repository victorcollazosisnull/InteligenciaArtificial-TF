using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    public float damage = 20f;

    private bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        HealthBase hp =
            other.GetComponent<HealthBase>();

        if (hp == null) return;

        if (hp.typeAgent != TypeAgent.Orco)
            return;

        activated = true;

        Debug.Log(
        $"[TRAMPA] VIDA ANTES: {hp.HealthPercent}%");
        hp.ApplyDamage(damage);
        Debug.Log(
        $"[TRAMPA] VIDA DESPUES: {hp.HealthPercent}%");

        Debug.Log(
            $"[TRAMPA] {other.name} recibió {damage} de daño");

        Destroy(gameObject);
    }
}