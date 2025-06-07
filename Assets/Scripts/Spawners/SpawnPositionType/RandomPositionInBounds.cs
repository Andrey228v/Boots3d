using UnityEngine;

namespace Assets.Scripts.Spawners.SpawnPositionType
{
    [RequireComponent(typeof(Collider))]
    public class RandomPositionInBounds : MonoBehaviour, ISpawnPosition
    {
        private Vector3 _bounds;

        public void Awake()
        {
            _bounds = GetComponent<Collider>().bounds.extents;
        }

        public Vector3 GetSpawnPosition()
        {
            float x = UnityEngine.Random.Range(-_bounds.x + transform.position.x, _bounds.x + transform.position.x);
            float y = transform.position.y;
            float z = UnityEngine.Random.Range(-_bounds.z + transform.position.z, _bounds.z + transform.position.z);

            return new Vector3(x, y, z);
        }
    }
}
