using Assets.Scripts.Spawners;
using System;
using UnityEngine;

namespace Assets.Scripts.Resurses
{
    public class Resource : MonoBehaviour, ISpawnObject<Resource>
    {
        [SerializeField] private Transform _pointForTake;
        [SerializeField] private Transform _bodyResurs;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Collider _collider;

        private ResursWorkerTrigger _workerTrigger;

        public event Action OnResursTaked;
        public event Action<Resource> DestroedSpawnObject;

        public Renderer Renderer { get; private set; }

        private void Awake()
        {
            Renderer = _bodyResurs.GetComponent<Renderer>();
            _workerTrigger = GetComponent<ResursWorkerTrigger>();
            _workerTrigger.OnWorkerTrigger += GetResurs;
        }

        public Transform Take()
        {
            OnResursTaked?.Invoke();
            _collider.enabled = false;

            return _pointForTake;
        }

        public void Despawn()
        {
            if (DestroedSpawnObject != null) 
            {
                DestroedSpawnObject?.Invoke(this);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public float GetHight()
        {
            Vector3 size = _renderer.bounds.size;

            return size.y;
        }

        public void SetWorker(Worker worker)
        {
            _workerTrigger.SetTakerWorker(worker);
        }

        public void GetResurs(Worker worker)
        {
            worker.View.TakeObject(this);
            worker.BackToBase();
        }
    }
}
