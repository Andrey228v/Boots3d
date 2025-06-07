using Assets.Scripts.Spawners;
using System;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts.Resurses
{
    public class Resurs : MonoBehaviour, ISpawnObject<Resurs>
    {
        [SerializeField] private Transform _pointForTake;
        [SerializeField] private Transform _bodyResurs;
        [SerializeField] private MeshRenderer _renderer;

        public bool IsCanTake { get; private set; }
        public Renderer Renderer { get; private set; }
        public Material Material { get; private set; }

        public event Action<Resurs> DestroedSpawnObject;

        private void Awake()
        {
            Renderer = _bodyResurs.GetComponent<Renderer>();
            Material = Renderer.material;
            IsCanTake = true;
        }

        public void SetCanTaken(bool isCanTake)
        {
            IsCanTake = isCanTake;
        }

        public Transform Take()
        {
            IsCanTake = false;
            //_uiResurs.SetText(IsCanTake.ToString());
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
