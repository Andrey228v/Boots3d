using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class FlagController : MonoBehaviour
    {
        [SerializeField] private Flag _flagPrefab;

        private Coroutine _coroutineSetFlag;
        private Coroutine _coroutineCancel;
        private Flag _flagSet;
        private bool _isActive;

        public event Action<Flag> OnSet;

        public bool IsSet { get; private set; }

        private void Start()
        {
            IsSet = false;
        }

        public bool TrySetFlag()
        {
            _isActive = true;
            _coroutineSetFlag = StartCoroutine(WaitButtonCreate());
            _coroutineCancel = StartCoroutine(WaitButtonCancel());

            return IsSet;
        }

        public bool IsFlagSet()
        {
            return IsSet;
        }

        public void UnSetFlag()
        {
            _isActive = false;
        }

        public void DestroyFlag(Flag flag, Worker worker)
        {
            IsSet = false;
            Destroy(flag.gameObject);
        }

        public Flag GetFlagPosition()
        {
            return _flagSet;
        }

        public IEnumerator WaitButtonCreate()
        {
            while (_isActive) 
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    IsSet = true;

                    if (_flagSet == null)
                    {
                        Flag flag = Instantiate(_flagPrefab);
                        _flagSet = flag;
                        _flagSet.transform.position = hit.point;
                        OnSet?.Invoke(_flagSet);
                    }
                }
            }
        }

        public IEnumerator WaitButtonCancel()
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.anyKeyDown && !Input.GetMouseButtonDown(1));
            Debug.Log("Отмена установки флага");
            StopCoroutine(_coroutineSetFlag);
        }
    }
}
