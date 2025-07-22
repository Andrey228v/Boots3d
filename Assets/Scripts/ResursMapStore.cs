using Assets.Scripts.Resurses;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


namespace Assets.Scripts
{
    public class MapStoreResurs : MonoBehaviour
    {
        [SerializeField] private List<Base> _radarsOnMap;

        private Dictionary<Base, HashSet<Resource>> _resursesFounded;

        public event Action OnNatifyWorkers;

        private void Awake()
        {
            _resursesFounded = new Dictionary<Base, HashSet<Resource>>();

            for(int i = 0;  i < _radarsOnMap.Count; i++)
            {
                HashSet<Resource> resursesHash = new HashSet<Resource>();
                _resursesFounded.Add(_radarsOnMap[i], resursesHash);
                _radarsOnMap[i].OnNotify += AddResurs;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _radarsOnMap.Count; i++)
            {
                _radarsOnMap[i].OnNotify -= AddResurs;
            }
        }

        public void AddResurs(Base source, Resource resurs)
        {
            if (_resursesFounded.ContainsKey(source))
            {
                if(_resursesFounded.TryGetValue(source, out HashSet<Resource> resurses))
                {
                    resurses.Add(resurs);
                    _resursesFounded[source] = resurses;
                }
            }
            else
            {
                HashSet<Resource> resursesHash = new HashSet<Resource>();
                resursesHash.Add(resurs);
                _resursesFounded.Add(source, resursesHash);

            }
        }

        public bool TryGetFreeResurs(Base source, out Resource resurs)
        {
            bool isFound = false;
            resurs = null;

            if (_resursesFounded.TryGetValue(source, out HashSet<Resource> resurses))
            {
                var sortResurs = resurses.OrderBy(n => Vector3.Distance(n.transform.position, source.transform.position));

                foreach (Resource res in sortResurs) 
                {
                    resurs = res;
                    isFound = true;
                    resurses.Remove(res);
                    break;
                }
            }

            return isFound;
        }

        public void TakeResurs(Resource resurs)
        {
            foreach (Base key in _resursesFounded.Keys)
            {
                _resursesFounded[key].Remove(resurs);
                
                if(key.CommandCenter.TryFindWorkerTrackingResurs(resurs, out Worker worker))
                {
                    key.CommandCenter.SetCommandBackToBase(worker);
                }
            }
        }

        public void UploadResurs(Resource resurs)
        {
            foreach(Base key in _resursesFounded.Keys)
            {
                _resursesFounded[key].Remove(resurs);
            }
        }
    }
}
