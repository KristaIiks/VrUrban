using StudySystem;
using System;
using UnityEngine;

namespace ToolsSystem
{
    public class GazTerritory : MonoBehaviour, IStudy
    {
        [SerializeField] private PaintPoint[] _blocks;
        private uint _painted = 0;

        [SerializeField] private Action _onComplete;

        private void Awake()
        {
            for (int i = 0; i < _blocks.Length; i++)
            {
                _blocks[i].Init(this);
            }
        }

        public void Paint()
        {
            _painted++;

            if(_painted >= _blocks.Length) { _onComplete?.Invoke(); }
        }

        public void StartStudy()
        {
            _onComplete += FinishStudy;
        }

        private void FinishStudy()
        {
            StudyManager.Instance.StartNextStage();
            _onComplete -= FinishStudy;
        }

    }
}
