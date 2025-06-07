using Assets.Scripts.Resurses;
using Assets.Scripts.Spawners.SpawnPositionType;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    [RequireComponent(typeof(ISpawnPosition))]
    public class SpawnerResurs: Spawner<Resurs>
    {
        [SerializeField] private float _timeSpawn = 1f;

        private ISpawnPosition _spawnPosition;
        private WaitForSeconds _sleepTime;
        private bool _isSpawn = true;
        private float _heightResurs = 0.2f;

        protected new void Awake()
        {
            base.Awake();
            _sleepTime = new WaitForSeconds(_timeSpawn);
            _spawnPosition = GetComponent<ISpawnPosition>();

        }

        private void Start()
        {
            StartCoroutine(StartSpawn());
        }

        public Resurs Spawn()
        {
            Resurs resurs = GetSpawnObject();
            resurs.transform.position = _spawnPosition.GetSpawnPosition();

            return resurs;
        }

        private IEnumerator StartSpawn()
        {
            while (_isSpawn)
            {
                Spawn();

                yield return _sleepTime;
            }
        }
    }
}
