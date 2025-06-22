using Assets.Scripts.Spawners;
using System;
using UnityEngine;

namespace Assets.Scripts.Resurses
{
    public class Resurs : MonoBehaviour, ISpawnObject<Resurs>
    {
        [SerializeField] private Transform _pointForTake;
        [SerializeField] private Transform _bodyResurs;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Collider _collider;

        public event Action OnResursTaked;
        public event Action<Resurs> DestroedSpawnObject;

        public bool IsCanTake { get; private set; }
        public Renderer Renderer { get; private set; }
        public Material Material { get; private set; }

        private void Awake()
        {
            Renderer = _bodyResurs.GetComponent<Renderer>();
            Material = Renderer.material;
            IsCanTake = true;
        }

        private void OnEnable()
        {
            IsCanTake = true;
        }

        public void SetCanTaken(bool isCanTake)
        {
            IsCanTake = isCanTake;
        }

        public Transform Take()
        {
            IsCanTake = false;
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
