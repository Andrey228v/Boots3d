using System;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    public interface ISpawnObject<T> where T : Component
    {
        public event Action<T> DestroedSpawnObject;

        public void Despawn();
    }
}
