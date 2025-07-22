using Assets.Scripts.Resurses;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    public class AIWorker : MonoBehaviour
    {
        private Worker _worker;
        
        public bool IsTracking { get; private set; }
        public bool IsOnBase { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == _worker.BaseOwn.gameObject)
            {
                IsOnBase = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _worker.BaseOwn.gameObject)
            {
                IsOnBase = false;
            }
        }

        private void Awake()
        {
            _worker = GetComponent<Worker>();
            IsOnBase = false;
        }

        public void Tracking(Resource resursTaking, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {
            IsTracking = true;
            StartCoroutine(StartTracking(resursTaking, view, distanseTakeObject, ignoreLayerUnit));
        }

        private IEnumerator StartTracking(Resource resurs, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {
            while (IsTracking)
            {

                Debug.DrawRay(view.transform.position, view.transform.forward, Color.green, 0.01f);

                RaycastHit[] hits = Physics.RaycastAll(view.transform.position, view.transform.forward, distanseTakeObject, ignoreLayerUnit);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject == resurs.gameObject)
                    {
                        view.TakeObject(resurs);
                        _worker.BackToBase();
                    }
                }

                yield return null;
            }
        }

        public void SetIsTracking(bool isTracking)
        {
            IsTracking = isTracking;
        }
    }
}
