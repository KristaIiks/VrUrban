using UnityEngine;

namespace ToolsSystem
{
    [RequireComponent(typeof(Collider))]
    public class PaintPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _obj;

        private GazTerritory _territory;
        private Collider _collider;

        private void OnValidate()
        {
            _collider ??= GetComponent<Collider>();
        }

        public void Init(GazTerritory _ter)
        {
            _territory = _ter;
        }

        public void Paint()
        {
            _obj.SetActive(true);

            _territory.Paint();
            _collider.enabled = false;
        }

    }
}
