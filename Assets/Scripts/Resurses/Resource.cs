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

        public event Action OnResursTaked;
        public event Action<Resource> DestroedSpawnObject;

        public Renderer Renderer { get; private set; }

        private void Awake()
        {
            Renderer = _bodyResurs.GetComponent<Renderer>();
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
    }
}
