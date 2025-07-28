using Assets.Scripts.Resurses;
using System;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    public class WorkerView : MonoBehaviour
    {
        [SerializeField] private Transform _handPoint;
        [SerializeField] private Renderer _renderer;

        private Transform _point;

        public event Action<Resource> OnTakeResurs;

        public Resource ObjectTake { get; private set; }
        public bool IsResursTake { get; private set; }

        public void TakeObject(Resource objectTake)
        {
            ObjectTake = objectTake;
            objectTake.Take();
            objectTake.transform.SetParent(_handPoint);
            objectTake.transform.position = _handPoint.position;
            IsResursTake = true;
            OnTakeResurs?.Invoke(objectTake);
        }

        public void SetPoint(Transform point)
        {
            _point = point;
        }

        public void UploadObject()
        {
            ObjectTake = null;
            IsResursTake = false;
        }

        public void SetPosition(float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _point.position, speed * Time.deltaTime);
            transform.LookAt(_point);
        }

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}
