using Assets.Scripts.Resurses;
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
    private Collider[] _hitColliders;

    public event Action<Resource> OnFounded;

    private void Awake()
    {
        _sleepTime = new WaitForSeconds(_periodScan);
        int maxColliders = 100;
        _hitColliders = new Collider[maxColliders];
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

        int length = Physics.OverlapSphereNonAlloc(_radarAria.position, _radius, _hitColliders, _foundType);

        for (int i = 0; i < length; i++) 
        {
            if (_hitColliders[i].TryGetComponent(out Resource resurs))
            {
                OnFounded?.Invoke(resurs);
            }
        }
    }
}
