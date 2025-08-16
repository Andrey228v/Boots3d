using Assets.Scripts;
using Assets.Scripts.BasesObjects;
using Assets.Scripts.BasesObjects.BaseCommands;
using Assets.Scripts.BasesObjects.BaseState;
using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachineBase))]
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

    private Color _colorWorker;
    private StateMachineBase _stateMachine;

    public event Action<Worker> OnWorkerCreated;

    public List<Worker> WorkersList { get; private set; }
    public CommandCenter CommandCenter { get; private set; }
    public Stack<ICommand> Commands { get; private set; }

    private void Awake()
    {
        WorkersList = new List<Worker>();
        _colorWorker = UnityEngine.Random.ColorHSV();

        Commands = new Stack<ICommand>();

        _stateMachine = GetComponent<StateMachineBase>();

        for (int i = 0; i < _countWorkers; i++)
        {
            Worker worker = CreateWorker();
        }

        CommandCenter = new CommandCenter(WorkersList, _baseQueuePosition.GetPosition());
        
        _baseUI.SetCountWorker(_countWorkers);

        _store.OnAppend += _baseUI.SetCountResurses;
        _radar.OnFounded += NotifyResursFound;
        _triggerOnWorker.OnWorkerBackToBase += CommandCenter.SetCommandUploadResurs;
        _flagController.OnSet += StartAccumulate;
    }

    private void Start()
    {
        UseRadar();
        StarRequestCommand();
    }

    private void OnDestroy()
    {
        _store.OnAppend -= _baseUI.SetCountResurses;
        _radar.OnFounded -= NotifyResursFound;
        _triggerOnWorker.OnWorkerBackToBase -= CommandCenter.SetCommandUploadResurs;
        _flagController.OnSet -= StartAccumulate;

        foreach (var worker in WorkersList) 
        {
            worker.FlagTrigger.OnFlagTrigger -= _flagController.DestroyFlag;
        }
    }

    public void UseRadar()
    {
        StartCoroutine(_radar.StartScan());
    }

    public void StarRequestCommand()
    {
        StartCoroutine(RequestCommand());
    }

    public void NotifyResursFound(Resource resurs)
    {
        _mapStoreResurs.AddResurs(resurs);
    }

    public Worker CreateWorker()
    {
        Worker worker = _baseRespawn.Spawn();
        worker.Init(this, _store, true);
        WorkersList.Add(worker);
        worker.View.SetColor(_colorWorker);
        OnWorkerCreated?.Invoke(worker);
        worker.FlagTrigger.OnFlagTrigger += _flagController.DestroyFlag;

        return worker;
    }

    public void Select()
    {
        _baseColorChanger.Select();
        _flagController.TrySetFlag();
    }

    public void UnSelect()
    {
        _flagController.UnSetFlag();
        _baseColorChanger.ResetColor();
    }

    public void StartAccumulate(Flag flag)
    {
        StartCoroutine(RequestAccumulateBase());
    }

    public void SelectState(BaseStateType stateType)
    {
        _stateMachine.SelectState(stateType);
    }

    public IEnumerator RequestAccumulateBase()
    {
        yield return new WaitUntil(_flagController.IsFlagSet);
        yield return new WaitUntil(CommandCenter.HasUndoFreeWorkers);

        _stateMachine.SelectState(BaseStateType.Create);
    }

    public void SetMapStoreResurs(MapStoreResurs mapStoreResurs)
    {
        _mapStoreResurs = mapStoreResurs;
        _stateMachine.SetMapStoreResurs(_mapStoreResurs);
    }

    private IEnumerator RequestCommand()
    {
        while (enabled)
        {
            yield return new WaitUntil(() => Commands.Count > 0);

            if (Commands.Peek().CanExecute())
            {
                Commands.Pop().Execute();
            }
        }
    }
}
