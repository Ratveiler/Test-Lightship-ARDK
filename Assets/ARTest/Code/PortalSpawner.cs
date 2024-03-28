using Niantic.Lightship.AR.Utilities;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Core
{
    public class PortalSpawner : MonoBehaviour
    {
        [SerializeField] private AROcclusionManager _occlusionManager;
        [SerializeField] private Camera _camera;
        [SerializeField] private ARPortal _prefabToSpawn;
        [SerializeField] private GameObject _UIView;
        [SerializeField] private MoonView _moonView;

        private XRCpuImage? _depthimage;
        private bool _canSpawn = true;

        private void Update()
        {
            if (!_occlusionManager.subsystem.running)
            {
                return;
            }

            var displayMat = Matrix4x4.identity;

            if (_occlusionManager.TryAcquireEnvironmentDepthCpuImage(out var image))
            {
                _depthimage?.Dispose();
                _depthimage = image;
            }
            else
            {
                return;
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                var screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#else
        if(Input.touches.Length>0)
        {
            var screenPosition = Input.GetTouch(0).position;
#endif
                if (!_depthimage.HasValue || !_canSpawn) return;
                _canSpawn = false;
                _UIView.SetActive(false);
                var uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
                var eyeDepth = _depthimage.Value.Sample<float>(uv, displayMat);
                var worldPosition =
                    _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, eyeDepth));
                var portal = Instantiate(_prefabToSpawn, worldPosition, Quaternion.identity) as ARPortal;
                portal.Initialize(_camera);
                _moonView.Initialize(portal);
            }
        }
    }
}