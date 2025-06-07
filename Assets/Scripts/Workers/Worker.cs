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
    private WorkerView _view;
    private AIWorker _aiWorker;
    private Store _store;

    public event Action<Worker> DestroedSpawnObject;
    public event Action DeliveredResurs;
    public event Action<Worker> RealisedWorker;

    public Base BaseOwn { get; private set; }

    private void Awake()
    {
        _view = GetComponent<WorkerView>();
        _stateMachinWorker = GetComponent<StateMachineWorker>();
        _aiWorker = GetComponent<AIWorker>();
        _ignoreLayerUnit = ~_ignoreLayerUnit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == BaseOwn.gameObject)
        {
            if (_view.IsResursTake == true)
            {
                DeliveredResurs?.Invoke();
                _store.Append(_view.ObjectTake);
                _view.UploadObject();
                _stateMachinWorker.SelectState(WorkerStateType.Wait);
                RealisedWorker?.Invoke(this);
                //_view.UploadObject(BaseOwn.transform);
                //_view.UploadObject(_store.Append());
            }
            else
            {
                _stateMachinWorker.SelectState(WorkerStateType.Wait);
                RealisedWorker?.Invoke(this);
            }
        }
    }

    public void SetCoomandGetResurs(Resurs resurs)
    {
        _stateMachinWorker.SelectState(WorkerStateType.Run);
        _view.SetPoint(resurs.transform);
        _aiWorker.Tracking(resurs, _view, _distanseTakeObject, _ignoreLayerUnit);
    }

    public void SetBase(Base baseOwn)
    {
        BaseOwn = baseOwn;
    }

    public void SetStore(Store store)
    {
        _store = store;
    }

    public void Despawn()
    {
        DestroedSpawnObject?.Invoke(this);
    }
}
