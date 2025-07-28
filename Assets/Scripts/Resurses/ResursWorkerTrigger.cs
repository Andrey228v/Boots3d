using Assets.Scripts.Workers;
using System;
using UnityEngine;

namespace Assets.Scripts.Resurses
{
    public class ResursWorkerTrigger: MonoBehaviour
    {
        private Worker _worker;

        public event Action<Worker> OnWorkerTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Worker>(out Worker worker))
            {
                if(worker == _worker)
                {
                    OnWorkerTrigger?.Invoke(_worker);
                }
            }
        }

        public void SetTakerWorker(Worker worker)
        {
            _worker = worker;
        }
    }
}
