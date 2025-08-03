using Assets.Scripts;
using Assets.Scripts.BasesObjects;
using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, ISelectable
{
    [SerializeField] private Transform _workerPrefab;
    [SerializeField] private SpawnerWorker _baseRespawn;
    [SerializeField] private Radar _radar;
    [SerializeField] private BaseUI _baseUI;
    [SerializeField] private Color _baseColor;
    [SerializeField] private BaseQueuePosition _baseQueuePosition;
    [SerializeField] private Store _store;
    [SerializeField] private int _countWorkers;
    [SerializeField] private MapStoreResurs _mapStoreResurs;
    [SerializeField] private BaseTriggerOnWorker _triggerOnWorker;
    [SerializeField] private BaseColorChanger _baseColorChanger;
    [SerializeField] private FlagController _flagController;

    private List<Worker> _workersList;
    private Color _colorWorker;

    public CommandCenter CommandCenter { get; private set; }

    private void Awake()
    {
        _workersList = new List<Worker>();
        _colorWorker = UnityEngine.Random.ColorHSV();

        for (int i = 0; i < _countWorkers; i++)
        {
            Worker worker = CreateWorker();
        }

        CommandCenter = new CommandCenter(_workersList, _baseQueuePosition.GetPosition());

        _baseUI.SetCountWorker(_countWorkers);

        _store.OnAppend += _baseUI.SetCountResurses;
        _store.OnAccumulated += BuyUnit;
        _store.OnSpent += NotifyBuy;
        _radar.OnFounded += NotifyResursFound;
        _triggerOnWorker.OnWorkerBackToBase += CommandCenter.SetCommandUploadResurs;
    }

    private void Start()
    {
        UseRadar();
    }

    private void OnDestroy()
    {
        _store.OnAppend -= _baseUI.SetCountResurses;
        _store.OnAccumulated -= BuyUnit;
        _store.OnSpent -= NotifyBuy;
        _radar.OnFounded -= NotifyResursFound;
    }

    public void UseRadar()
    {
        StartCoroutine(_radar.StartScan());
        StartCoroutine(RequestTakeResursPosition());
    }

    public void NotifyResursFound(Resource resurs)
    {
        _mapStoreResurs.AddResurs(resurs);
    }

    public void NotifyBuy(Resource resurs)
    {
        _mapStoreResurs.RemoveResource(resurs);
    }

    public void BuyUnit()
    {
        Worker worker = CreateWorker();
        _store.SpentForBuyWorker();
        CommandCenter.AddFreeWorker(worker);
    }

    private Worker CreateWorker()
    {
        Worker worker = _baseRespawn.Spawn();
        worker.Init(this, _store, true);
        _workersList.Add(worker);
        worker.View.SetColor(_colorWorker);

        return worker;
    }

    public void Select()
    {
        _flagController.SetFlag();
        _baseColorChanger.Select();
    }

    public void UnSelect()
    {
        _flagController.UnSetFlag();
        _baseColorChanger.UnSelect();
    }



    //private IEnumerator RequestGoToFlag()
    //{

    //}


    private IEnumerator RequestTakeResursPosition()
    {
        while (enabled)
        {
            yield return new WaitUntil(CommandCenter.HasFreeWorkers);

            if (_mapStoreResurs.TryGetFreeResurs(transform.position, out Resource resurs))
            {
                Debug.DrawRay(transform.position, resurs.transform.position - transform.position, Color.yellow, 3f);
                CommandCenter.SetCommandTakeResurs(resurs);
            }

            yield return null;
        }
    }
}
