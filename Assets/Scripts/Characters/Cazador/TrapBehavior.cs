// TrapBehavior.cs
// Opcion C: daño continuo mientras el Orco esté en el trigger.
// La trampa dura un tiempo (lifetime) y luego se destruye sola.
using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    public float damage = 20f;   // daño por tick
    public float tickInterval = 1.5f;  // cada cuantos segundos aplica daño
    public float lifetime = 15f;   // segundos hasta que la trampa desaparece

    private float _tickTimer;
    private float _lifetimeTimer;
    private HealthBase _targetInside;   // el Orco que está dentro del trigger

    private void Start()
    {
        _tickTimer = 0f;      // aplica daño inmediatamente al entrar
        _lifetimeTimer = lifetime;
    }

    private void Update()
    {
        // Cuenta regresiva de vida de la trampa
        _lifetimeTimer -= Time.deltaTime;
        if (_lifetimeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        // Si hay un Orco dentro, aplica daño por tick
        if (_targetInside == null) return;
        if (_targetInside.IsDead)
        {
            _targetInside = null;
            return;
        }

        _tickTimer -= Time.deltaTime;
        if (_tickTimer <= 0f)
        {
            _tickTimer = tickInterval;
            Debug.Log($"[TRAMPA] VIDA ANTES: {_targetInside.HealthPercent}%");
            _targetInside.ApplyDamage(damage);
            Debug.Log($"[TRAMPA] VIDA DESPUES: {_targetInside.HealthPercent}%");
            Debug.Log($"[TRAMPA] Orco recibió {damage} de daño");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetInside != null) return; // ya hay alguien dentro

        HealthBase hp = other.GetComponent<HealthBase>();
        if (hp == null) return;
        if (hp.typeAgent != TypeAgent.Orco || hp.typeAgent != TypeAgent.Unicornio) return;

        _targetInside = hp;
        _tickTimer = 0f; // aplica daño inmediatamente al entrar
        Debug.Log("[TRAMPA] Orco entró en la trampa.");
    }

    private void OnTriggerExit(Collider other)
    {
        HealthBase hp = other.GetComponent<HealthBase>();
        if (hp == null) return;
        if (hp.typeAgent != TypeAgent.Orco || hp.typeAgent != TypeAgent.Unicornio) return;

        _targetInside = null;
        Debug.Log("[TRAMPA] Orco salió de la trampa.");
    }
}