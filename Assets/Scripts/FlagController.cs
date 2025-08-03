using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class FlagController : MonoBehaviour
    {
        [SerializeField] private Transform _flagPrefab;

        private Coroutine _coroutine;
        private Transform _flagSet;
        private bool _isActive;

        public event Action<Transform> OnSet;

        public void SetFlag()
        {
            _isActive = true;
            _coroutine = StartCoroutine(WaitButton());
        }

        public void UnSetFlag()
        {
            _isActive = false;
            StopCoroutine(_coroutine);
        }

        public void DestroyFlag()
        {
            Destroy(_flagSet.gameObject);
        }

        public Transform GetFlagPosition()
        {
            return _flagPrefab;
        }

        public IEnumerator WaitButton()
        {
            while (_isActive) 
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (_flagSet != null)
                    {
                        DestroyFlag();
                    }

                    Transform objectHit = hit.transform;
                    Transform flag = Instantiate(_flagPrefab);
                    _flagSet = flag;
                    flag.position = hit.point;
                    OnSet?.Invoke(_flagSet);
                }
            }
        }
    }
}
