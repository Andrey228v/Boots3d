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
        public StateMachineWorker MachineWorker { get; private set; }

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
            MachineWorker = GetComponent<StateMachineWorker>();
            IsOnBase = false;
        }

        public void Tracking(Resurs resursTaking, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {
            IsTracking = true;
            StartCoroutine(StartTracking(resursTaking, view, distanseTakeObject, ignoreLayerUnit));
        }

        private IEnumerator StartTracking(Resurs resurs, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {
            while (IsTracking)
            {
                if (resurs.IsCanTake == true)
                {
                    Debug.DrawRay(view.transform.position, view.transform.forward, Color.green, 0.01f);

                    RaycastHit[] hits =  Physics.RaycastAll(view.transform.position, view.transform.forward, distanseTakeObject, ignoreLayerUnit);

                    foreach(RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject == resurs.gameObject)
                        {
                            view.TakeObject(resurs);
                            view.SetPoint(_worker.BaseOwn.transform);
                            MachineWorker.SelectState(StateWorker.WorkerStateType.Run);
                        }
                    }
                }
                else
                {
                    IsTracking = false;
                    view.SetPoint(_worker.BaseOwn.transform);
                    MachineWorker.SelectState(StateWorker.WorkerStateType.Run);
                }

                yield return null;
            }
        }
    }
}
