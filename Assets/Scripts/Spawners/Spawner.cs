using Assets.Scripts.Spawners.ObjectsPools;
using System;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    public class Spawner<T>: MonoBehaviour where T : Component, ISpawnObject<T>
    {
        public event Action Spawned;

        public ObjectPoolPrefab<T> PoolFigure { get; private set; }

        protected void Awake()
        {
            PoolFigure = GetComponent<ObjectPoolPrefab<T>>();
        }

        private protected T GetSpawnObject()
        {
            T figure = PoolFigure.Pool.Get();
            Spawned?.Invoke();

            return figure;
        }
    }
}
