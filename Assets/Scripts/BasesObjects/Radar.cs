using Assets.Scripts.Resurses;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _periodScan;
    [SerializeField] private Transform _radarAria;
    [SerializeField] private Vector3 _radiusScan;
    [SerializeField] private float _radius = 50f;
    [SerializeField] private LayerMask _foundType;

    private bool _isScaning = true;
    private HashSet<Resurs> _foundResurses;
    private WaitForSeconds _sleepTime;
    private Collider[] _hitColliders;
    private bool _isFoundedResursEmpty = true;
    //private string _prevAction = "FindEvent";

    public event Action Founded;
    public event Action NotFounded;
    public event Action<HashSet<Resurs>> FoundedResurs;

    private void Start()
    {
        _foundResurses = new HashSet<Resurs>();
        _sleepTime = new WaitForSeconds(_periodScan);
        //_foundType = ~_foundType;
    }

    public IEnumerator StartScan()
    {
        while (_isScaning)
        {
            _radarAria.DOScale(_radiusScan, 2f)
                .OnComplete(() => ScanComplite());

            yield return _sleepTime;
        }
    }

    private void ScanComplite()
    {
        _radarAria.DORewind();
        _hitColliders = Physics.OverlapSphere(_radarAria.position, _radius, _foundType);

        foreach (Collider collider in _hitColliders)
        {
            if (collider.TryGetComponent<Resurs>(out Resurs resurs))
            {
                if(resurs.IsCanTake == true)
                {
                    _foundResurses.Add(resurs);
                }
                else
                {
                    _foundResurses.Remove(resurs); // нужно ли это....
                }
            }
        }

        _isFoundedResursEmpty = _foundResurses.Count == 0;

        if (_isFoundedResursEmpty == false)
        {
            FoundedResurs?.Invoke(_foundResurses);
        }
    }

    public bool TryGetFoundResurs(out Resurs resurs)
    {
        bool isFound = false;
        resurs = null;

        var sortResurs = _foundResurses.OrderBy(n => Vector3.Distance(n.transform.position, transform.position));

        resurs = null;

        foreach (var res in sortResurs)
        {
            if (res.IsCanTake == true)
            {
                resurs = res;
                _foundResurses.Remove(res); // ???? Скорее всего из-за этого ошибка может быть... 
                break;
            }
        }

        if (resurs != null)
        {
            isFound = true;
        }
        else
        {
            
        }

        return isFound;
    }
}
