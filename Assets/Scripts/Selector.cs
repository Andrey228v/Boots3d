using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Selector : MonoBehaviour
    {
        private Camera _camera;
        
        private ISelectable _lastSelect = null;

        public event Action OnBaseSelect;
        public event Action OnBaseUnSelect;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow, 10f);

                if (Physics.Raycast(ray, out RaycastHit _hitInfo))
                {
                    ISelectable select = _hitInfo.collider.gameObject.GetComponent<ISelectable>();
                    _lastSelect?.UnSelect();

                    if (select != null)
                    {
                        _lastSelect = select;
                        select.Select();
                    }
                }
                else
                {
                    _lastSelect?.UnSelect();
                    OnBaseUnSelect?.Invoke();
                }
            }
        }
    }
}
