// HealthIALand.cs
// Clase intermedia para NPCs terrestres de combate.
// Hereda de HealthCombat y agrega la gestion del NavMeshAgent.
// Tus clases concretas (HealthIALandDuende, etc.) heredan de esta.
using UnityEngine;
using UnityEngine.AI;

public class HealthIALand : HealthCombat
{
    // ── NavMeshAgent ─────────────────────────────────────
    protected NavMeshAgent agent;
    public    NavMeshAgent Agent => agent;

    // ── INICIALIZACION ───────────────────────────────────
    private void Awake() => LoadComponent();

    public override void LoadComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        base.LoadComponent();
    }

    // ── MUERTE: detiene la navegacion ────────────────────
    public override void Death()
    {
        if (agent != null)
            agent.enabled = false;
        base.Death();
    }

    // ── REAPARICION: reactiva el agente ──────────────────
    public override void Respawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        if (agent != null)
            agent.enabled = true;
        Active();
    }

    public override void Active()
    {
        if (agent != null)
            agent.enabled = true;
        base.Active();
    }

    // ── GIZMOS (editor) ──────────────────────────────────
    private void OnDrawGizmos()
    {
        if (!ShowGizmo) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCenter(), 0.08f);
    }
}
