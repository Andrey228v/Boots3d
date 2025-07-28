using Assets.Scripts.Resurses;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapStoreResurs : MonoBehaviour
    {
        private HashSet<Resource> _freeResurses;
        private HashSet<Resource> _resursesTaked;

        private void Awake()
        {
            _freeResurses = new HashSet<Resource>();
            _resursesTaked = new HashSet<Resource>();
        }

        public void AddResurs(Resource resurs)
        {
            if (_resursesTaked.Contains(resurs) == false)
            {
                _freeResurses.Add(resurs);
            }
        }

        public bool TryGetFreeResurs(Base source, out Resource resurs)
        {
            bool isFound = false;
            resurs = null;

            var sortResurs = _freeResurses.OrderBy(n => Vector3.Distance(n.transform.position, source.transform.position));

            foreach (Resource res in sortResurs)
            {
                resurs = res;
                isFound = true;
                _freeResurses.Remove(res);
                _resursesTaked.Add(res);
                break;
            }

            return isFound;
        }
    }
}
