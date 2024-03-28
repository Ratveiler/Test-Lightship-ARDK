using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class TweenText : MonoBehaviour
    {
        [SerializeField] private Transform _body;

        private void Awake()
        {
            _body.DOScale(1.2f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InCubic);
        }
    }
}