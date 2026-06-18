using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    private AIEyeNPC _aiEye;
    private AICharacterControl _ownerAI;
    private HealthBase _ownerHealth;
    private Blackboard _blackboard;

    [Header("Parametros de vision")]
    public float distance = 10f;
    [Range(0, 180)]
    public float angle = 60f;
    public float height = 1.5f;
    public int scanFrequency = 20;

    [Header("Memoria (segundos que recuerda al objetivo)")]
    public float memoryTime = 3f;
    private float _lostEnemyTimer = 0f;
    private float _lostAllyTimer = 0f;

    [Header("Layers")]
    public LayerMask enemyLayers;
    public LayerMask allyLayers;
    public LayerMask occlusionLayers;

    [Header("Ojo")]
    public Transform positionEye;

    [Header("Gizmos")]
    public Color meshColor = Color.red;
    public Color detectedColor = Color.green;
    public bool IsDrawGizmo = false;

    Mesh _mesh;
    float _scanInterval;
    float _scanTimer;

    // Arrays para solapamiento
    readonly Collider[] _colliders = new Collider[50];

    float _halfHeightNeg;
    float _halfHeightPos;

    // Referencias visuales para Gizmos
    Transform _detectedEnemy;
    Transform _detectedAlly;

    private void Awake()
    {
        _aiEye = GetComponent<AIEyeNPC>();
        _ownerAI = GetComponent<AICharacterControl>();
        _ownerHealth = GetComponent<HealthBase>();
        _blackboard = GetComponent<Blackboard>();
    }

    private void Start()
    {
        _scanInterval = 1f / scanFrequency;
        _scanTimer = _scanInterval;
        _halfHeightNeg = -height;
        _halfHeightPos = height;
    }

    private void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer > 0f) return;
        _scanTimer = _scanInterval;

        ScanEnemies();
        ScanAllies();
    }

    void ScanEnemies()
    {
        bool foundEnemy = false;
        int count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, enemyLayers);

        for (int i = 0; i < count; i++)
        {
            if (_colliders[i] == null || _colliders[i].gameObject == gameObject) continue;

            HealthBase otherHealth = _colliders[i].GetComponent<HealthBase>();
            if (otherHealth == null) continue;

            bool isEnemy = (_ownerHealth != null) && !_ownerHealth.IsAllied(otherHealth);
            if (!isEnemy || !IsInSight(_colliders[i])) continue;

            // Enemigo encontrado
            Transform enemy = _colliders[i].transform;
            if (_aiEye != null) _aiEye.Enemy = enemy;
            _detectedEnemy = enemy;
            _lostEnemyTimer = memoryTime;
            foundEnemy = true;

            if (_blackboard != null)
            {
                _blackboard.enemyTarget = enemy;
                _blackboard.lastKnownPosition = enemy.position;
                _blackboard.hasLastKnownPosition = true;
            }
            break;
        }

        if (!foundEnemy)
        {
            _lostEnemyTimer -= _scanInterval;
            if (_lostEnemyTimer <= 0f)
            {
                if (_aiEye != null) _aiEye.Enemy = null;
                _detectedEnemy = null;
                if (_blackboard != null) _blackboard.enemyTarget = null;
            }
        }
    }

    void ScanAllies()
    {
        bool foundAlly = false;
        int count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, allyLayers);

        for (int i = 0; i < count; i++)
        {
            if (_colliders[i] == null || _colliders[i].gameObject == gameObject) continue;

            HealthBase otherHealth = _colliders[i].GetComponent<HealthBase>();
            if (otherHealth == null) continue;

            bool isAlly = (_ownerHealth != null) && _ownerHealth.IsAllied(otherHealth);
            if (!isAlly || !IsInSight(_colliders[i])) continue;

            // Aliado encontrado
            Transform ally = _colliders[i].transform;
            if (_aiEye != null) _aiEye.Allied = ally;
            _detectedAlly = ally;
            _lostAllyTimer = memoryTime;
            foundAlly = true;

            if (_blackboard != null) _blackboard.allyTarget = ally;
            break;
        }

        if (!foundAlly)
        {
            _lostAllyTimer -= _scanInterval;
            if (_lostAllyTimer <= 0f)
            {
                if (_aiEye != null) _aiEye.Allied = null;
                _detectedAlly = null;
                if (_blackboard != null) _blackboard.allyTarget = null;
            }
        }
    }

    public bool IsInSight(Collider other)
    {
        Vector3 origin = transform.position;
        Vector3 dest = other.transform.position;
        Vector3 direction = dest - origin;

        if (direction.y < _halfHeightNeg || direction.y > _halfHeightPos) return false;
        direction.y = 0f;
        if (direction.sqrMagnitude > distance * distance) return false;
        if (Vector3.Angle(direction, transform.forward) > angle) return false;

        Vector3 eyePos = (positionEye != null) ? positionEye.position : transform.position;
        return !Physics.Linecast(eyePos, dest, occlusionLayers);
    }

    public bool IsInSight(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        return col != null && IsInSight(col);
    }

    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        _halfHeightNeg = -height;
        _halfHeightPos = height;
    }

    private void OnDrawGizmos()
    {
        if (!IsDrawGizmo) return;

        if (_mesh != null)
        {
            // Si detecta a cualquiera de los dos, cambia el color de la malla
            Gizmos.color = (_detectedEnemy != null || _detectedAlly != null) ? detectedColor : meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }

        Vector3 eyePos = (positionEye != null) ? positionEye.position : transform.position;

        // Dibuja línea azul hacia el enemigo
        if (_detectedEnemy != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(eyePos, _detectedEnemy.position);
        }

        // Dibuja línea cyan/celeste hacia el aliado
        if (_detectedAlly != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(eyePos, _detectedAlly.position);
        }

        // Si no hay ninguno, dibuja una línea amarilla apuntando hacia adelante
        if (_detectedEnemy == null && _detectedAlly == null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(eyePos, eyePos + transform.forward * 5f);
        }
    }
    public Transform GetClosestEnemy()
    {
        return _detectedEnemy; // Devuelve el enemigo que el sensor detectó actualmente
    }

    public float GetDistanceToClosestEnemy()
    {
        if (_detectedEnemy != null)
        {
            return Vector3.Distance(transform.position, _detectedEnemy.position);
        }
        return 30f; // Retorna la distancia máxima si no hay nadie, para que el sistema asuma "Lejos"
    }

    Mesh CreateWedgeMesh()
    {
        var wedge = new Mesh();
        const int segs = 10;
        int numTri = (segs * 4) + 4;
        int numVert = numTri * 3;
        var vertices = new Vector3[numVert];
        var triangles = new int[numVert];
        var rotL = Quaternion.Euler(0, -angle, 0);
        var rotR = Quaternion.Euler(0, angle, 0);

        Vector3 bC = Vector3.zero, tC = Vector3.up * height;
        Vector3 bL = rotL * Vector3.forward * distance;
        Vector3 bR = rotR * Vector3.forward * distance;
        Vector3 tL = bL + Vector3.up * height;
        Vector3 tR = bR + Vector3.up * height;

        int v = 0;
        vertices[v++] = bC; vertices[v++] = bL; vertices[v++] = tL;
        vertices[v++] = tL; vertices[v++] = tC; vertices[v++] = bC;
        vertices[v++] = bC; vertices[v++] = tC; vertices[v++] = tR;
        vertices[v++] = tR; vertices[v++] = bR; vertices[v++] = bC;

        float ca = -angle, da = (angle * 2f) / segs;
        for (int i = 0; i < segs; i++)
        {
            bL = Quaternion.Euler(0, ca, 0) * Vector3.forward * distance;
            bR = Quaternion.Euler(0, ca + da, 0) * Vector3.forward * distance;
            tL = bL + Vector3.up * height;
            tR = bR + Vector3.up * height;

            vertices[v++] = bL; vertices[v++] = bR; vertices[v++] = tR;
            vertices[v++] = tR; vertices[v++] = tL; vertices[v++] = bL;
            vertices[v++] = tC; vertices[v++] = tL; vertices[v++] = tR;
            vertices[v++] = bC; vertices[v++] = bR; vertices[v++] = bL;
            ca += da;
        }

        for (int i = 0; i < numVert; i++) triangles[i] = i;

        wedge.vertices = vertices;
        wedge.triangles = triangles;
        wedge.RecalculateNormals();

        return wedge;
    }
}