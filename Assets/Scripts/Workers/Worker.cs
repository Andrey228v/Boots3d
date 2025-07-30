using Assets.Scripts.BasesObjects;
using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners;
using Assets.Scripts.Workers;
using Assets.Scripts.Workers.StateWorker;
using System;
using UnityEngine;

[RequireComponent(typeof(WorkerView), typeof(StateMachineWorker), typeof(WorkerResursTrigger))]
public class Worker : MonoBehaviour, ISpawnObject<Worker>
{
    [SerializeField] private float _distanseTakeObject = 1f;
    [SerializeField] private LayerMask _ignoreLayerUnit;

    private StateMachineWorker _stateMachinWorker;
    private WorkerResursTrigger _resursTrigger;

    private Store _store;

    public event Action<Worker> DestroedSpawnObject;
    public event Action<Worker> RealisedWorker;

    public Base BaseOwn { get; private set; }
    public bool IsFree { get; private set; }
    public WorkerView View { get; private set; }
    public Resource TargetResurs { get; private set; }

    private void Awake()
    {
        View = GetComponent<WorkerView>();
        _stateMachinWorker = GetComponent<StateMachineWorker>();
        _resursTrigger = GetComponent<WorkerResursTrigger>();
        _ignoreLayerUnit = ~_ignoreLayerUnit;
        IsFree = true;
        _resursTrigger.OnResourceTrigger += View.TakeObject;
        View.OnTakeResurs += BackToBase;
    }

    private void OnDestroy()
    {
        _resursTrigger.OnResourceTrigger -= View.TakeObject;
        View.OnTakeResurs += BackToBase;
    }

    public void Init(Base baseOwn, Store store, bool isFree)
    {
        BaseOwn = baseOwn;
        _store = store;
        IsFree = isFree;
    }

    public void GetResurs(Resource resurs)
    {
        View.SetPoint(resurs.transform);
        _stateMachinWorker.SelectState(WorkerStateType.Run);
        TargetResurs = resurs;
        _resursTrigger.SetTakeResource(resurs);
    }

    public void UploadObject()
    {
        _store.Append(View.ObjectTake);
        View.UploadObject();
        _stateMachinWorker.SelectState(WorkerStateType.Wait);
    }

    public void BackToBase()
    {
        View.SetPoint(BaseOwn.transform);
        _stateMachinWorker.SelectState(WorkerStateType.Run);
    }

    public void SetIsFree(bool isFree)
    {
        IsFree = isFree;
    }

    public void Despawn()
    {
        DestroedSpawnObject?.Invoke(this);
    }
}
