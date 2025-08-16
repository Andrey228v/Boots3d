using System;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    public class WorkerFlagTrigger : MonoBehaviour
    {
        private Flag _flag;
        private Worker _worker;

        public event Action<Flag, Worker> OnFlagTrigger;

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Flag flag))
            {
                if(flag = _flag)
                {
                    OnFlagTrigger?.Invoke(flag, _worker);
                }
            }
        }

        public void SetFlag(Flag flag)
        {
            _flag = flag;
        }

        public void SetWorker(Worker worker)
        {
            _worker = worker;
        }
    }
}
