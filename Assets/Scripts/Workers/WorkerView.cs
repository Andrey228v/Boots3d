using Assets.Scripts.Resurses;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Workers
{
    public class WorkerView : MonoBehaviour
    {
        [SerializeField] private Transform _handPoint;

        public event Action<string> ResursTaked;

        private Transform _point;
        public Resurs ObjectTake { get; private set; }

        public bool IsResursTake { get; private set; }

        public void TakeObject(Resurs objectTake)
        {
            ObjectTake = objectTake;
            objectTake.Take();
            objectTake.transform.SetParent(_handPoint);
            objectTake.transform.position = _handPoint.position;
            IsResursTake = true;
            ResursTaked?.Invoke(IsResursTake.ToString());
        }

        public void UploadObject()
        {
            if (ObjectTake != null)
            {
                //ObjectTake.transform.SetParent(pointToUpload);
                //ObjectTake.transform.position = pointToUpload.position;
                ObjectTake = null;
                IsResursTake = false;
                ResursTaked?.Invoke(IsResursTake.ToString());
            }
        }

        //public void UploadObject(Transform pointToUpload)
        //{
        //    if (ObjectTake != null)
        //    {
        //        ObjectTake.transform.SetParent(pointToUpload);
        //        ObjectTake.transform.position = pointToUpload.position;
        //        ObjectTake = null;
        //        IsResursTake = false;
        //        ResursTaked?.Invoke(IsResursTake.ToString());
        //    }
        //}

        public void SetPoint(Transform point)
        {
            _point = point;
        }

        public void SetPosition(float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _point.position, speed * Time.deltaTime);
            transform.LookAt(_point);
        }
    }
}
