using Assets.Scripts.Resurses;
using System;
using UnityEngine;

namespace Assets.Scripts.Workers
{
    public class WorkerResursTrigger : MonoBehaviour
    {
        private Resource _resource;

        public event Action<Resource> OnResourceTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Resource resource))
            {
                if(resource == _resource)
                {
                    OnResourceTrigger?.Invoke(resource);
                }
            }
        }

        public void SetTakeResource(Resource resource)
        {
            _resource = resource;
        }
    }
}
