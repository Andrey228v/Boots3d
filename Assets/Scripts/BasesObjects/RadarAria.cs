using Assets.Scripts.Resurses;
using System;
using UnityEngine;

namespace Assets.Scripts.BasesObjects
{
    public class RadarAria : MonoBehaviour
    {
        private Collider[] _hitColliders;
        
        public event Action<Resource> OnFounded;

        public void ScanAria(Vector3 centre, float radius, LayerMask type)
        {
            _hitColliders = Physics.OverlapSphere(centre, radius, type);

            foreach (Collider collider in _hitColliders)
            {
                if (collider.TryGetComponent<Resource>(out Resource resurs))
                {
                    OnFounded?.Invoke(resurs);
                }
            }
        }

    }
}
