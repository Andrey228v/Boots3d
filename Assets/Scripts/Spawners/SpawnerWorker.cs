using Assets.Scripts.Spawners.SpawnPositionType;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    [RequireComponent(typeof(ISpawnPosition))]
    public class SpawnerWorker : Spawner<Worker>
    {
        private ISpawnPosition _spawnPosition;

        protected new void Awake()
        {
            base.Awake();
            _spawnPosition = GetComponent<ISpawnPosition>();
        }

        public Worker Spawn()
        {
            Worker worker = GetSpawnObject();
            worker.transform.position = _spawnPosition.GetSpawnPosition();

            return worker;
        }
    }
}
