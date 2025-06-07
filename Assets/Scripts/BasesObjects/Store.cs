using Assets.Scripts.Resurses;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class Store : MonoBehaviour
    {
        [SerializeField] private Transform _storePosition;
        [SerializeField] private Transform _storePoint;
        
        public void Append(Resurs resurs)
        {
            resurs.transform.SetParent(_storePosition);
            resurs.transform.position = _storePoint.position;
            resurs.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
            _storePoint.position = new Vector3(_storePoint.position.x, _storePoint.position.y + resurs.GetHight(), _storePoint.position.z);
        }
    }
}
