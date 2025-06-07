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
    //Должен быть ещ ё склад...

    [SerializeField] private int _countWorkers;
    [SerializeField] private int _resursesInBase;

    private List<Worker> _workersList;
    private CommandCenter _commandCenter;
 

    private void Awake()
    {
        _workersList = new List<Worker>();
    }

    private void Start()
    {
        for (int i = 0; i < _countWorkers; i++)
        {
            CreateWorker();
        }

        _commandCenter = new CommandCenter(_workersList, _baseQueuePosition.GetPosition());

        _baseUI.SetCountWorker(_countWorkers);
        _baseUI.SetCountResurses(_resursesInBase);

        UseRadar();
    }


    private Worker CreateWorker()
    {
        Worker worker = _baseRespawn.Spawn();
        _workersList.Add(worker);
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
            if (_radar.TryGetFoundResurs(out Resurs resurs))
            {
                Debug.Log("DRAW");
                Debug.DrawRay(transform.position, resurs.transform.position - transform.position, Color.yellow, 1f);

                _commandCenter.SetCommandTakeResurs(resurs);
            }

            yield return new WaitUntil(_commandCenter.IsFreeWorkerHave);
        }
    }

}
