using Assets.Scripts.Resurses;
using DG.Tweening;
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
    [SerializeField] private float _durationScan = 2f;

    private bool _isScaning = true;
    private HashSet<Resurs> _resursesFounded;
    private HashSet<Resurs> _foundResurses;
    private WaitForSeconds _sleepTime;
    private Collider[] _hitColliders;

    private void Start()
    {
        _resursesFounded = new HashSet<Resurs>();
        _foundResurses = new HashSet<Resurs>();
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
        _hitColliders = Physics.OverlapSphere(_radarAria.position, _radius, _foundType);

        foreach (Collider collider in _hitColliders)
        {
            if (collider.TryGetComponent<Resurs>(out Resurs resurs))
            {
                if (resurs.IsCanTake == true )
                {

                    if (_resursesFounded.Contains(resurs) == false)
                    {
                        _foundResurses.Add(resurs);
                    }

                    _resursesFounded.Add(resurs);
                }
            }
        }
    }

    public void DeliverFoundResurs(Resurs resurs)
    {
        if (_resursesFounded.Contains(resurs))
        {
            _resursesFounded.Remove(resurs);
        }
    }

    public bool TryGetFoundResurs(out Resurs resurs)
    {
        bool isFound = false;
        resurs = null;

        var sortResurs = _foundResurses.OrderBy(n => Vector3.Distance(n.transform.position, transform.position));

        foreach (var res in sortResurs)
        {
            if (res.IsCanTake == true)
            {
                resurs = res;
                isFound = true;
                _foundResurses.Remove(res);
                break;
            }
        }

        return isFound;
    }

    public bool IsFreeResursesHave()
    {
        return _foundResurses.Count > 0;
    }
}
