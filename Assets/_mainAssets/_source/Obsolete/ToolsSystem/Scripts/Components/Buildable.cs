using UnityEngine;
using StudySystem;

namespace ToolsSystem
{
    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    public class Buildable : MonoBehaviour, IStudy
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Collider _collider;

        private MeshRenderer _renderer;
        private bool _isStudy = false;

        private void OnValidate()
        {
            _collider ??= GetComponent<Collider>();
            _renderer ??= GetComponent<MeshRenderer>();
        }

        public void Place()
        {
            _panel.SetActive(true);
            _renderer.enabled = false;
            _collider.enabled = false;

            if (_isStudy)
            {
                FinishStidy();
            }
        }

        public void StartStudy()
        {
            _isStudy = true;
        }

        private void FinishStidy()
        {
            StudyManager.Instance.StartNextStage(3f, gameObject);
            _isStudy = false;
        }
    }
}
