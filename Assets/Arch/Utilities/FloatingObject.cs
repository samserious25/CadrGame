using UnityEngine;

namespace Assets.Arch.Utilities
{
    public class FloatingObject : MonoBehaviour
    {
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private float _speed = 1f;
        private float _tempVal;
        private Vector3 _tempPos;

        private void Start() =>
            _tempVal = transform.localPosition.y;

        private void FixedUpdate()
        {
            _tempPos.y = _tempVal + _amplitude * Mathf.Sin(_speed * Time.time);
            transform.localPosition = _tempPos;
        }
    }
}