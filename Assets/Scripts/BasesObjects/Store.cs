using Assets.Scripts.Resurses;
using System;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class Store : MonoBehaviour
    {
        [SerializeField] private Transform _storePosition;
        [SerializeField] private Transform _storePoint;

        private int _resursCount = 0;

        public event Action<int> OnAppend;

        public void Append(Resource resurs)
        {
            resurs.transform.SetParent(_storePosition);
            resurs.transform.position = _storePoint.position;
            resurs.transform.rotation = Quaternion.LookRotation(transform.position);
            _storePoint.position = new Vector3(_storePoint.position.x, _storePoint.position.y + resurs.GetHight(), _storePoint.position.z);
            _resursCount++;
            OnAppend?.Invoke(_resursCount);
        }
    }
}
