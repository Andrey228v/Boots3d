using Assets.Scripts.Resurses;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    public class AIWorker : MonoBehaviour
    {
        //private float _distanseTakeObject;

        private Worker _worker;
        private bool _isTracking;

        public StateMachineWorker MachineWorker { get; private set; }


        private void Awake()
        {
            _worker = GetComponent<Worker>();
            MachineWorker = GetComponent<StateMachineWorker>();
        }

        public void Tracking(Resurs resursTaking, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {
            _isTracking = true;
            StartCoroutine(StartTracking(resursTaking, view, distanseTakeObject, ignoreLayerUnit));
        }

        private IEnumerator StartTracking(Resurs resurs, WorkerView view, float distanseTakeObject, LayerMask ignoreLayerUnit)
        {

            while (_isTracking)
            {
               
                if (resurs.IsCanTake == true)
                {
                    Debug.DrawRay(view.transform.position, view.transform.forward, Color.yellow, 0.01f);

                    if (Physics.Raycast(view.transform.position, view.transform.forward, out RaycastHit hit, distanseTakeObject, ignoreLayerUnit))
                    {

                        if (hit.collider.gameObject == resurs.gameObject)
                        {
                            view.TakeObject(resurs);
                            MachineWorker.SelectState(StateWorker.WorkerStateType.Wait);
                            view.SetPoint(_worker.BaseOwn.transform);
                            MachineWorker.SelectState(StateWorker.WorkerStateType.Run);
                        }
                    }
                }
                else
                {
                    Debug.Log("BACK TO BASE");
                    _isTracking = false;
                    view.SetPoint(_worker.BaseOwn.transform);
                    MachineWorker.SelectState(StateWorker.WorkerStateType.Run);
                    //CommandMoveToBase commandMoveToBase = new CommandMoveToBase(view, RadarResurs, MachineUnit);
                    //commandMoveToBase.Execute();
                }

                yield return null;
            }
        }

    }
}
