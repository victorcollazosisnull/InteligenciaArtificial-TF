// VisionSensor.cs
// Sensor de vision con cono 3D visible en escena.
// Detecta enemigos dentro de un angulo y distancia configurables,
// con raycast para no ver a traves de paredes (occlusionLayers).
// Escribe el resultado en AIEyeNPC.Enemy para que el BehaviorTree lo lea.
// Adaptado a la jerarquia del profe: usa AICharacterControl y TypeAgent.
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    private AIEyeNPC      _aiEye;
    private AICharacterControl _ownerAI;
    private HealthBase    _ownerHealth;

    [Header("Parametros de vision")]
    public float distance     = 10f;
    [Range(0, 180)]
    public float angle        = 60f;
    public float height       = 1.0f;
    public int   scanFrequency = 20;      // escaneos por segundo

    [Header("Layers")]
    public LayerMask enemyLayers;         // layer de los NPCs enemigos
    public LayerMask occlusionLayers;     // paredes y obstaculos

    [Header("Ojo (punto de origen del raycast)")]
    public Transform positionEye;         // arrastra aqui un Transform hijo (ej: Eye)

    [Header("Gizmos")]
    public Color meshColor      = Color.red;
    public Color detectedColor  = Color.green;
    public bool  IsDrawGizmo    = false;

    // ── Internos ─────────────────────────────────────────
    Mesh     _mesh;
    float    _scanInterval;
    float    _scanTimer;
    int      _count;
    readonly Collider[] _colliders = new Collider[50];
    float    _halfHeightNeg;
    float    _halfHeightPos;

    // ultimo enemigo detectado (para Gizmos)
    Transform _detected;

    // ── INIT ─────────────────────────────────────────────
    private void Awake()
    {
        _aiEye       = GetComponent<AIEyeNPC>();
        _ownerAI     = GetComponent<AICharacterControl>();
        _ownerHealth = GetComponent<HealthBase>();
    }

    private void Start()
    {
        _scanInterval   = 1f / scanFrequency;
        _scanTimer      = _scanInterval;
        _halfHeightNeg  = -height;
        _halfHeightPos  =  height;
    }

    // ── UPDATE ───────────────────────────────────────────
    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer > 0f) return;
        _scanTimer = _scanInterval;
        Scan();
    }

    // ── ESCANEO ──────────────────────────────────────────
    void Scan()
    {
        _detected        = null;
        _aiEye.Enemy     = null;
        _aiEye.Allied    = null;

        _count = Physics.OverlapSphereNonAlloc(
            transform.position, distance, _colliders, enemyLayers);

        for (int i = 0; i < _count; i++)
        {
            if (_colliders[i] == null) continue;
            if (_colliders[i].gameObject == gameObject) continue;

            HealthBase otherHealth = _colliders[i].GetComponent<HealthBase>();
            if (otherHealth == null) continue;

            // Mismo bando = aliado, distinto bando = enemigo
            bool isEnemy = (_ownerHealth != null)
                           && !_ownerHealth.IsAllied(otherHealth);

            if (!IsInSight(_colliders[i])) continue;

            if (isEnemy && _aiEye.Enemy == null)
            {
                _aiEye.Enemy = _colliders[i].transform;
                _detected    = _colliders[i].transform;
            }
            else if (!isEnemy && _aiEye.Allied == null)
            {
                _aiEye.Allied = _colliders[i].transform;
            }

            // Si ya encontramos enemigo y aliado, paramos
            if (_aiEye.Enemy != null && _aiEye.Allied != null) break;
        }
    }

    // ── CAMPO DE VISION ──────────────────────────────────
    public bool IsInSight(Collider other)
    {
        Vector3 origin    = transform.position;
        Vector3 dest      = other.transform.position;
        Vector3 direction = dest - origin;

        // Filtro vertical
        if (direction.y < _halfHeightNeg || direction.y > _halfHeightPos)
            return false;

        direction.y = 0f;

        // Filtro distancia
        if (direction.sqrMagnitude > distance * distance) return false;

        // Filtro angulo
        if (Vector3.Angle(direction, transform.forward) > angle) return false;

        // Filtro oclusion (paredes)
        Vector3 eyePos = (positionEye != null) ? positionEye.position : transform.position;
        return !Physics.Linecast(eyePos, dest, occlusionLayers);
    }

    public bool IsInSight(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        return col != null && IsInSight(col);
    }

    // ── GIZMOS ───────────────────────────────────────────
    private void OnValidate()
    {
        _mesh          = CreateWedgeMesh();
        _halfHeightNeg = -height;
        _halfHeightPos =  height;
    }

    private void OnDrawGizmos()
    {
        if (!IsDrawGizmo) return;

        if (_mesh != null)
        {
            Gizmos.color = (_detected != null) ? detectedColor : meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }

        Vector3 eyePos = (positionEye != null)
            ? positionEye.position
            : transform.position;

        if (_detected != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(eyePos, _detected.position);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(eyePos, eyePos + transform.forward * 5f);
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < _count; i++)
            if (_colliders[i] != null)
                Gizmos.DrawSphere(_colliders[i].transform.position, 0.3f);
    }

    // ── MALLA DEL CONO ───────────────────────────────────
    Mesh CreateWedgeMesh()
    {
        var wedge = new Mesh();
        const int segments   = 10;
        int numTriangles     = (segments * 4) + 4;
        int numVertices      = numTriangles * 3;
        var vertices         = new Vector3[numVertices];
        var triangles        = new int[numVertices];

        var rotLeft  = Quaternion.Euler(0, -angle, 0);
        var rotRight = Quaternion.Euler(0,  angle, 0);

        Vector3 bottomCenter = Vector3.zero;
        Vector3 topCenter    = Vector3.up * height;
        Vector3 bottomLeft   = rotLeft  * Vector3.forward * distance;
        Vector3 bottomRight  = rotRight * Vector3.forward * distance;
        Vector3 topLeft      = bottomLeft  + Vector3.up * height;
        Vector3 topRight     = bottomRight + Vector3.up * height;

        int vert = 0;

        // Cara lateral izquierda
        vertices[vert++] = bottomCenter; vertices[vert++] = bottomLeft;  vertices[vert++] = topLeft;
        vertices[vert++] = topLeft;      vertices[vert++] = topCenter;    vertices[vert++] = bottomCenter;

        // Cara lateral derecha
        vertices[vert++] = bottomCenter; vertices[vert++] = topCenter;    vertices[vert++] = topRight;
        vertices[vert++] = topRight;     vertices[vert++] = bottomRight;  vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle   = (angle * 2f) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft  = Quaternion.Euler(0, currentAngle,              0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            topLeft     = bottomLeft  + Vector3.up * height;
            topRight    = bottomRight + Vector3.up * height;

            // Cara frontal
            vertices[vert++] = bottomLeft;  vertices[vert++] = bottomRight; vertices[vert++] = topRight;
            vertices[vert++] = topRight;    vertices[vert++] = topLeft;     vertices[vert++] = bottomLeft;

            // Tapa superior
            vertices[vert++] = topCenter;    vertices[vert++] = topLeft;     vertices[vert++] = topRight;

            // Tapa inferior
            vertices[vert++] = bottomCenter; vertices[vert++] = bottomRight; vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++) triangles[i] = i;

        wedge.vertices  = vertices;
        wedge.triangles = triangles;
        wedge.RecalculateNormals();
        return wedge;
    }
}
