// VisionSensor.cs - FIX DEFINITIVO
// El problema anterior: Scan() borraba currentTarget=null 20x por segundo,
// compitiendo con el BT que lo leía en el mismo frame.
// Solución: el target se mantiene hasta que realmente se pierde de vista.
// Solo se borra si pasaron X segundos sin verlo (memoryTime).
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

    [Header("Memoria (segundos que recuerda al enemigo)")]
    public float memoryTime = 3f;   // mantiene el target X segundos tras perderlo de vista
    private float _lostTimer = 0f;

    [Header("Layers")]
    public LayerMask enemyLayers;
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
    int _count;
    readonly Collider[] _colliders = new Collider[50];
    float _halfHeightNeg;
    float _halfHeightPos;
    Transform _detected;

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
        Scan();
    }

    void Scan()
    {
        bool foundEnemy = false;

        _count = Physics.OverlapSphereNonAlloc(
            transform.position, distance, _colliders, enemyLayers);

        for (int i = 0; i < _count; i++)
        {
            if (_colliders[i] == null) continue;
            if (_colliders[i].gameObject == gameObject) continue;

            HealthBase otherHealth = _colliders[i].GetComponent<HealthBase>();
            if (otherHealth == null) continue;

            bool isEnemy = (_ownerHealth != null) && !_ownerHealth.IsAllied(otherHealth);
            if (!isEnemy) continue;
            if (!IsInSight(_colliders[i])) continue;

            // ENCONTRÓ enemigo: actualiza todo y resetea el timer de memoria
            Transform enemy = _colliders[i].transform;
            _aiEye.Enemy = enemy;
            _detected = enemy;
            _lostTimer = memoryTime;
            foundEnemy = true;

            if (_blackboard != null)
            {
                _blackboard.currentTarget = enemy;
                _blackboard.lastKnownPosition = enemy.position;
                _blackboard.hasLastKnownPosition = true;
            }
            break;
        }

        if (!foundEnemy)
        {
            // ❌ No ve al enemigo: descuenta el timer de memoria
            _lostTimer -= _scanInterval;

            if (_lostTimer <= 0f)
            {
                // Pasó el tiempo de memoria: borra el target
                _aiEye.Enemy = null;
                _detected = null;
                if (_blackboard != null)
                    _blackboard.currentTarget = null;
                // lastKnownPosition se mantiene para MoveToLastKnownPosition
            }
            // Si todavía hay memoria, NO borra el target — el BT lo sigue usando
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
            Gizmos.color = (_detected != null) ? detectedColor : meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }
        Vector3 eyePos = (positionEye != null) ? positionEye.position : transform.position;
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
