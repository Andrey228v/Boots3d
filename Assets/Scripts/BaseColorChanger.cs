using UnityEngine;

namespace Assets.Scripts
{
    public class BaseColorChanger : MonoBehaviour
    {
        [SerializeField] private Material _materialSelected;

        private MeshRenderer _meshRenderer;
        private Material _currentMaterial;


        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _currentMaterial = _meshRenderer.material;
        }

        public void Select()
        {
            _meshRenderer.material = _materialSelected;
        }

        public void UnSelect()
        {
            _meshRenderer.material = _currentMaterial;
        }
    }
}
