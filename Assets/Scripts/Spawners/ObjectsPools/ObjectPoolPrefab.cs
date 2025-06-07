using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Spawners.ObjectsPools
{
    public class ObjectPoolPrefab<T> : MonoBehaviour where T : Component, ISpawnObject<T>
    {
        [SerializeField] private Transform _spawnContainer;
        [SerializeField] private T _typePrefab;

        private int _poolCapacity = 10;
        private int _poolSize = 30;

        public event Action Created;
        public event Action Destroyed;
        public event Action<Transform> Despawned;

        public ObjectPool<T> Pool { get; private set; }

        private void Awake()
        {
            Pool = new ObjectPool<T>(OnCreateFigure, OnTakeFromPool, OnRelease, OnDestroyFigure, false, _poolCapacity, _poolSize);
        }

        private T OnCreateFigure()
        {
            T prefab = Instantiate(_typePrefab, _spawnContainer);
            Created?.Invoke();

            return prefab;
        }

        private void OnTakeFromPool(T figure)
        {
            figure.DestroedSpawnObject += Release;
            figure.gameObject.SetActive(true);
        }

        private void OnRelease(T figure)
        {
            figure.DestroedSpawnObject -= Release;
            figure.gameObject.SetActive(false);
        }

        private void OnDestroyFigure(T figure)
        {
            figure.DestroedSpawnObject -= Release;
            Destroy(figure);
        }

        private void Release(T figure)
        {
            Destroyed?.Invoke();
            Despawned?.Invoke(figure.gameObject.transform);
            Pool.Release(figure);
        }
    }
}
