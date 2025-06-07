using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class BaseQueuePosition : MonoBehaviour
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private int _rowCount;
        [SerializeField] private int _columnCount;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _distanceBeetweenPosition;
        [SerializeField] private Transform _prefab;
        [SerializeField] private Transform _parent;

        private List<BaseSlotWorker> _queuePosition;

        private void Awake()
        {
            _queuePosition = new List<BaseSlotWorker>();
        }

        private void Start()
        {
            for (int i = 0; i < _columnCount; i++) 
            {
                for (int j = 0; j < _rowCount; j++) 
                {
                    Vector3 newPosition = _startPosition + new Vector3(_distanceBeetweenPosition.x * i, _distanceBeetweenPosition.y, _distanceBeetweenPosition.z * j * -1);

                    Transform prefab = Instantiate(_prefab);
                    prefab.SetParent(_parent);
                    prefab.localPosition = newPosition;

                    _queuePosition.Add(new BaseSlotWorker(prefab.position, true));
                }
            }
        }

        public List<BaseSlotWorker> GetPosition()
        {
            List<BaseSlotWorker> positions = new(_queuePosition);

            return positions;
        }
    }
}
