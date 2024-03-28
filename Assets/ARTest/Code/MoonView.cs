using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class MoonView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _moonViewObjects = new();
        [SerializeField] private LayerMask _layer;

        private ARPortal _portal;

#if UNITY_EDITOR
        [ContextMenu("GetComponents")]
        private void GetComponents()
        {
            var objects = transform.GetComponentsInChildren<Transform>().ToList();
            _moonViewObjects = objects;
        }
#endif

        public void Initialize(ARPortal portal)
        {
            _portal = portal;
            transform.position = _portal.transform.position;
        }

        private void Update()
        {
            if (!_portal) return;
            if (_portal.IsCameraInsidePortal())
            {
                ChangeLayer();
                DisablePortal();
            }
        }

        private void DisablePortal()
        {
            _portal.gameObject.SetActive(false);
            _portal = null;
        }

        private void ChangeLayer()
        {
            foreach (var moonObject in _moonViewObjects)
            {
                moonObject.gameObject.layer = _layer;
            }
        }
    }
}