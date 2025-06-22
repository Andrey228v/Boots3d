using Assets.Scripts.BasesObjects;
using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _workers;
    [SerializeField] private SpawnerWorker _baseRespawn;
    [SerializeField] private Radar _radar;
    [SerializeField] private BaseUI _baseUI;
    [SerializeField] private Color _baseColor;
    [SerializeField] private BaseQueuePosition _baseQueuePosition;
    [SerializeField] private Store _store;
    [SerializeField] private int _countWorkers;

    private List<Worker> _workersList;
    private CommandCenter _commandCenter;
    private Color _colorWorker;
    private float _minRangeColor = 0f;
    private float _maxRangeColor = 1f;
 
    private void Awake()
    {
        _workersList = new List<Worker>();

        _colorWorker.r = Random.Range(_minRangeColor, _maxRangeColor);
        _colorWorker.g = Random.Range(_minRangeColor, _maxRangeColor);
        _colorWorker.b = Random.Range(_minRangeColor, _maxRangeColor);

        for (int i = 0; i < _countWorkers; i++)
        {
            Worker worker = CreateWorker();
            _workersList.Add(worker);
            worker.View.SetColor(_colorWorker);
            worker.OnUploadObject += _radar.DeliverFoundResurs;
        }

        _commandCenter = new CommandCenter(_workersList, _baseQueuePosition.GetPosition());

        _baseUI.SetCountWorker(_countWorkers);

        _store.OnAppend += _baseUI.SetCountResurses;
    }

    private void Start()
    {
        UseRadar();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _workersList.Count; i++)
        {
            _workersList[i].OnUploadObject -= _radar.DeliverFoundResurs;
        }
        _commandCenter.Dispose();
    }

    private Worker CreateWorker()
    {
        Worker worker = _baseRespawn.Spawn();
        worker.SetBase(this);
        worker.SetStore(_store);

        return worker;
    }

    public void UseRadar()
    {
        StartCoroutine(_radar.StartScan());
        StartCoroutine(RequestTakeResursPosition());
    }

    private IEnumerator RequestTakeResursPosition()
    {
        while (true)
        {
            yield return new WaitUntil(_commandCenter.IsFreeWorkerHave);
            yield return new WaitUntil(_radar.IsFreeResursesHave);

            if (_radar.TryGetFoundResurs(out Resurs resurs))
            {
                Debug.DrawRay(transform.position, resurs.transform.position - transform.position, Color.yellow, 3f);

                _commandCenter.SetCommandTakeResurs(resurs);
            }

            yield return null;
        }
    }
}
