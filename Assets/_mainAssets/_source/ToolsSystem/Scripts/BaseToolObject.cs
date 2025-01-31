using System;
using StudySystem;
using UnityEngine;

namespace ToolsSystem
{
    public abstract class BaseToolObject : MonoBehaviour, IStudyObject
    {
        public abstract void StartDefaultStudy(Action OnComplete = null);
        public abstract void Restart(bool canContinue = true);
    }
}