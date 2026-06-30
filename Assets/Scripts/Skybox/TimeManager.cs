// TimeManager.cs
// Singleton que expone si es de noche o de dia.
// Se sincroniza con CycleDayNight via el mismo timer.
// Los nodos del BT (IsNight) lo consultan para decidir comportamiento.
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Range(0f, 1f)]
    [Tooltip("0-1, igual que timePercent en CycleDayNight. Se actualiza solo si UseOwnTimer=true.")]
    public float timePercent;

    [Header("Configuracion")]
    [Tooltip("Si está activo, este script avanza su propio timer (independiente).")]
    public bool  useOwnTimer   = false;
    public float cycleDuration = 30f;

    [Header("Umbrales de noche (igual que CycleDayNight)")]
    [Range(0f, 1f)] public float nightStart = 0.75f; // empieza la noche
    [Range(0f, 1f)] public float nightEnd    = 0.25f; // termina la noche (amanecer)

    private float _timer;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (!useOwnTimer) return;
        _timer = (_timer + Time.deltaTime) % cycleDuration;
        timePercent = _timer / cycleDuration;
    }

    /// <summary>true si es de noche (fase 4 o inicio de fase 1 del ciclo).</summary>
    public bool IsNight => timePercent >= nightStart || timePercent < nightEnd;

    /// <summary>Llamado por CycleDayNight para sincronizar el tiempo exacto.</summary>
    public void SetTimePercent(float t) => timePercent = t;
}
