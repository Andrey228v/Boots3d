using Assets.Scripts.Resurses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class Store : MonoBehaviour
    {
        [SerializeField] private Transform _storePosition;
        [SerializeField] private Transform _storePoint;
        [SerializeField] private int _unitPrice = 6;

        private int _resursCount = 0;
        private Stack<Resource> _resources;

        public event Action<int> OnAppend;
        public event Action OnAccumulated;
        public event Action<Resource> OnSpent;

        private void Awake()
        {
            _resources = new Stack<Resource>();
        }

        public void Append(Resource resurs)
        {
            resurs.transform.SetParent(_storePosition);
            resurs.transform.position = _storePoint.position;
            resurs.transform.rotation = Quaternion.LookRotation(transform.position);
            _storePoint.position = new Vector3(_storePoint.position.x, _storePoint.position.y + resurs.GetHight(), _storePoint.position.z);
            _resursCount++;
            _resources.Push(resurs);

            if (_resursCount == _unitPrice)
            {
                OnAccumulated?.Invoke();
            }

            OnAppend?.Invoke(_resursCount);
        }

        public void SpentForBuyWorker()
        {
            _resursCount -= _unitPrice;

            for (int i = 0; i < _unitPrice; i++) 
            {
                Resource resource = _resources.Pop();
                resource.Despawn();
                _storePoint.position = new Vector3(_storePoint.position.x, _storePoint.position.y - resource.GetHight(), _storePoint.position.z);
                OnSpent?.Invoke(resource);
            }
        }
    }
}
