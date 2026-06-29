using System.Collections.Generic;
using UnityEngine;

public class AlarmSystemPato : MonoBehaviour
{
    [Header("Alarma")]
    public float alarmRadius = 10f;
    public float alarmDuration = 5f;

    public bool IsAlarmed { get; private set; }

    private int _currentAlarmId;
    private float _alarmTimer;

    private static List<AlarmSystemPato> _allPatos = new List<AlarmSystemPato>();
    private static int _nextAlarmId = 1;

    private void OnEnable() { if (!_allPatos.Contains(this)) _allPatos.Add(this); }
    private void OnDisable() { _allPatos.Remove(this); }

    private void Update()
    {
        if (!IsAlarmed) return;
        _alarmTimer -= Time.deltaTime;
        if (_alarmTimer <= 0f)
            IsAlarmed = false;
    }

    public void TriggerAlarm()
    {
        int alarmId = _nextAlarmId++;
        IsAlarmed = true;
        _alarmTimer = alarmDuration;
        _currentAlarmId = alarmId;
        PropagateAlarm(alarmId);
    }

    private void ReceiveAlarm(int alarmId)
    {
        if (_currentAlarmId == alarmId) return;
        IsAlarmed = true;
        _alarmTimer = alarmDuration;
        _currentAlarmId = alarmId;
        PropagateAlarm(alarmId);
    }

    private void PropagateAlarm(int alarmId)
    {
        for (int i = 0; i < _allPatos.Count; i++)
        {
            var pato = _allPatos[i];
            if (pato == null || pato == this) continue;
            float dist = Vector3.Distance(transform.position, pato.transform.position);
            if (dist <= alarmRadius)
                pato.ReceiveAlarm(alarmId);
        }
    }
}
