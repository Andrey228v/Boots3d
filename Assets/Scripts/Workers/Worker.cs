using Assets.Scripts.BasesObjects;
using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners;
using Assets.Scripts.Workers;
using Assets.Scripts.Workers.StateWorker;
using System;
using UnityEngine;

[RequireComponent(typeof(WorkerView), typeof(StateMachineWorker), typeof(AIWorker))]
public class Worker : MonoBehaviour, ISpawnObject<Worker>
{
    [SerializeField] private float _distanseTakeObject = 1f;
    [SerializeField] private LayerMask _ignoreLayerUnit;

    private StateMachineWorker _stateMachinWorker;
    private AIWorker _aiWorker;
    private Store _store;

    public event Action<Worker> DestroedSpawnObject;
    public event Action DeliveredResurs;
    public event Action<Worker> RealisedWorker;
    public event Action<Resurs> OnUploadObject;

    public Base BaseOwn { get; private set; }
    public bool IsFree { get; private set; }
    public WorkerView View { get; private set; }

    private void Awake()
    {
        View = GetComponent<WorkerView>();
        _stateMachinWorker = GetComponent<StateMachineWorker>();
        _aiWorker = GetComponent<AIWorker>();
        _ignoreLayerUnit = ~_ignoreLayerUnit;
        IsFree = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_aiWorker.IsOnBase && View.IsResursTake == true && _aiWorker.IsTracking == false && IsFree == false)
        {
            DeliveredResurs?.Invoke();
            OnUploadObject?.Invoke(View.ObjectTake);
            _store.Append(View.ObjectTake);
            View.UploadObject();
            _stateMachinWorker.SelectState(WorkerStateType.Wait);
            RealisedWorker?.Invoke(this);
        }
        else if (_aiWorker.IsOnBase && View.IsResursTake == false && _aiWorker.IsTracking == false && IsFree == false)
        {
            _stateMachinWorker.SelectState(WorkerStateType.Wait);
            RealisedWorker?.Invoke(this);
        }
    }

    public void SetCoomandGetResurs(Resurs resurs)
    {
        View.SetPoint(resurs.transform);
        _stateMachinWorker.SelectState(WorkerStateType.Run);
        _aiWorker.Tracking(resurs, View, _distanseTakeObject, _ignoreLayerUnit);
    }

    public void SetBase(Base baseOwn)
    {
        BaseOwn = baseOwn;
    }

    public void SetStore(Store store)
    {
        _store = store;
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
