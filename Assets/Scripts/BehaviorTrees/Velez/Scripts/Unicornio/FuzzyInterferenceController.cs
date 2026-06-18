using UnityEngine;

public class FuzzyInferenceController : MonoBehaviour
{
    private HealthIALandUnicornio _health;
    private VisionSensor _vision;
    private Blackboard _blackboard;

    // Guarda el último resultado para que el nodo de condición lo consulte de forma directa
    [SerializeField] private float _ultimaTendenciaCalculada;

    void Awake()
    {
        _health = GetComponent<HealthIALandUnicornio>();
        _vision = GetComponent<VisionSensor>();
        _blackboard = GetComponent<Blackboard>();
    }

    void Update()
{
    if (_health == null || _vision == null) return;

        // 1. Verificar primero si realmente hay un enemigo
        Transform enemigo = _blackboard.enemyTarget;
    
    if (enemigo == null)
    {
        // Si no hay enemigo, la tendencia es neutral (50) o el valor que decidas
        _ultimaTendenciaCalculada = 50f; 
        return; // Nos salimos temprano y ahorramos procesamiento
    }

    // 2. Si llegamos aquí, es porque SÍ hay un enemigo detectado
    float saludPropia = _health.GetHealthPercentage();
    float distancia = _vision.GetDistanceToClosestEnemy();
    
    float saludEnemigo = 100f;
    var h = enemigo.GetComponent<HealthBase>();
    if (h != null) saludEnemigo = h.HealthPercent;

    // 3. Procesamiento de Lógica Difusa seguro
    _ultimaTendenciaCalculada = CalcularLogicaDifusa(saludPropia, saludEnemigo, distancia);
}

    /// <summary>
    /// Método público que el nodo CheckFuzzyDirect llamará en cada frame.
    /// </summary>
    public float GetTendenciaActual()
    {
        return _ultimaTendenciaCalculada;
    }

    private float CalcularLogicaDifusa(float saludPropia, float saludEnemigo, float distancia)
    {
        // --- PASO 1: FUSIFICACIÓN (Grados de pertenencia de 0.0 a 1.0) ---
        float saludBaja = GradoSaludBaja(saludPropia);
        float saludMedia = GradoSaludMedia(saludPropia);
        float saludAlta = GradoSaludAlta(saludPropia);

        float enemiBaja = GradoSaludEnemigoBaja(saludEnemigo);

        float distCerca = GradoDistanciaCerca(distancia);
        float distMedio = GradoDistanciaMedio(distancia);

        // --- PASO 2: INFERENCIA (Evaluación de Reglas con AND/OR) ---
        // Huir: Si estoy débil (independientemente del enemigo) y está cerca o a distancia media
        float pesoHuir = Mathf.Max(Mathf.Min(saludBaja, distCerca), Mathf.Min(saludBaja, distMedio));

        // Defender: Si estoy en salud media o la situación está controlada
        float pesoDefender = saludMedia;

        // Atacar: Si tengo salud alta Y el enemigo está débil Y está en rango corto (Cerca)
        float pesoAtacar = Mathf.Min(saludAlta, Mathf.Min(enemiBaja, distCerca));

        // --- PASO 3: DEFUSIFICACIÓN (Media Ponderada de Sugeno) ---
        float sumaPesos = pesoHuir + pesoDefender + pesoAtacar;

        // Si no se activó ninguna regla por la combinación de valores, vuelve a un estado neutral (Defensa)
        if (sumaPesos <= 0.001f) return 50f;

        // Valores singleton asignados: Huir = 20, Defender/Cautela = 50, Atacar = 80
        return ((pesoHuir * 20f) + (pesoDefender * 50f) + (pesoAtacar * 80f)) / sumaPesos;
    }

    // ====================================================================================
    // FUNCIONES DE PERTENENCIA LINGÜÍSTICA (Triángulos y Rampas usando InverseLerp)
    // ====================================================================================

    // Salud propia (Rango 0 - 100%)
    private float GradoSaludBaja(float s) => Mathf.Clamp01(Mathf.InverseLerp(40f, 0f, s));
    private float GradoSaludMedia(float s)
    {
        if (s <= 25f || s >= 75f) return 0f;
        return (s <= 50f) ? Mathf.InverseLerp(25f, 50f, s) : Mathf.InverseLerp(75f, 50f, s);
    }
    private float GradoSaludAlta(float s) => Mathf.Clamp01(Mathf.InverseLerp(60f, 100f, s));

    // Salud enemigo (Rango 0 - 100%)
    private float GradoSaludEnemigoBaja(float s) => Mathf.Clamp01(Mathf.InverseLerp(45f, 0f, s));

    // Distancia enemigo (Rango en Metros de Unity)
    private float GradoDistanciaCerca(float d) => Mathf.Clamp01(Mathf.InverseLerp(8f, 0f, d));
    private float GradoDistanciaMedio(float d)
    {
        if (d <= 5f || d >= 18f) return 0f;
        return (d <= 11f) ? Mathf.InverseLerp(5f, 11f, d) : Mathf.InverseLerp(18f, 11f, d);
    }
}