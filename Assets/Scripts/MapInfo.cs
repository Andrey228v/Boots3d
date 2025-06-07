using UnityEngine;

public class MapInfo : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Vector3 _bounds;


    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _bounds = _renderer.bounds.size;

        //Debug.Log(_bounds);
        //renderer.bounds.size;
    }
}
