using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _periodScan;
    [SerializeField] private Transform _radarAria;
    [SerializeField] private Vector3 _radiusScan;
    [SerializeField] private float _radius = 50f;
    [SerializeField] private LayerMask _foundType;
    [SerializeField] private float _durationScan = 2f;

    private bool _isScaning = true;
    private WaitForSeconds _sleepTime;

    public event Action<Vector3, float, LayerMask> AriaDrawed;

    private void Start()
    {
        _sleepTime = new WaitForSeconds(_periodScan);
    }

    public IEnumerator StartScan()
    {
        while (_isScaning)
        {
            _radarAria.DOScale(_radiusScan, _durationScan)
                .OnComplete(() => ScanComplite());

            yield return _sleepTime;
        }
    }

    private void ScanComplite()
    {
        _radarAria.DORewind();
        AriaDrawed?.Invoke(_radarAria.position, _radius, _foundType);
    }
}
