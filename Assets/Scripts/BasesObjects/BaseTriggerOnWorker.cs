using System;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class BaseTriggerOnWorker : MonoBehaviour
    {
        public event Action<Worker> OnWorkerBackToBase;

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<Worker>(out Worker worker))
            {
                if (worker.View.IsResursTake == true)
                {
                    OnWorkerBackToBase?.Invoke(worker);
                }
            }
        }
    }
}
