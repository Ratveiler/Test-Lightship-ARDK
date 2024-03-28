using UnityEngine;

namespace Core
{
    public class ARPortal : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _distancePoint;
        [SerializeField] private float _checkDistance = 1f;

        public void Initialize(Camera camera)
        {
            _camera = camera;
        }

        public bool IsCameraInsidePortal()
        {
            float distance = (_camera.transform.position - _distancePoint.position).magnitude;
            return distance < _checkDistance;
        }
    }
}