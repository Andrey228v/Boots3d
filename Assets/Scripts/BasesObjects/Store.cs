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
        [SerializeField] private int _basePrice = 12;

        private int _resursCount = 0;
        private Stack<Resource> _resources;

        public event Action<int> OnAppend;
        public event Action OnAccumulatedForUnit;
        public event Action OnAccumulatedForBase;
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
                OnAccumulatedForUnit?.Invoke();
            }

            if (_resursCount == _basePrice)
            {
                OnAccumulatedForBase?.Invoke();
            }


            OnAppend?.Invoke(_resursCount);
        }

        public void SpentForBuyWorker()
        {
            _resursCount -= _unitPrice;
            DestroyResurs(_unitPrice);
        }

        public void SpentForBuyBase()
        {
            _resursCount -= _basePrice;
            DestroyResurs(_basePrice);
        }

        private void DestroyResurs(int resursCount)
        {

            for (int i = 0; i < resursCount; i++)
            {
                Resource resource = _resources.Pop();
                _storePoint.position = new Vector3(_storePoint.position.x, _storePoint.position.y - resource.GetHight(), _storePoint.position.z);
                OnSpent?.Invoke(resource);
                resource.Despawn();
            }
        }
    }
}
