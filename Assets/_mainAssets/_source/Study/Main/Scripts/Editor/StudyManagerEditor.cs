#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace StudySystem
{
    [CustomEditor(typeof(StudyManager))]
    public class StudyManagerEditor : Editor
    {
        private StudyManager _target;

        private SerializedProperty _chapters;
        private SerializedProperty _onStudyComplete;

        private void OnEnable()
        {
            _target = (StudyManager)target;

            _chapters = serializedObject.FindProperty("Chapters");
            _onStudyComplete = serializedObject.FindProperty("OnStudyComplete");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("StudyManager", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            GUILayout.Label("By Kristaliks", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 });
            GUILayout.Label("<<---------------------------------------------------------->>", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 16 });
            GUILayout.Space(10);

            #region Settings

            GUILayout.Label("Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 });
            GUILayout.Label("------------------------------------------------------------------", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 16 });
            GUILayout.Space(10);

            _target.BackgroundMusic = (AudioSource)EditorGUILayout.ObjectField(new GUIContent("BG Audio", "Фоновая музыка"), _target.BackgroundMusic, typeof(AudioSource), true);
            _target._audioSource = (AudioSource)EditorGUILayout.ObjectField(new GUIContent("Study Audio", "Звуки обучения"), _target._audioSource, typeof(AudioSource), true);

            _target._timeDelay = EditorGUILayout.Slider(new GUIContent("Delay", "Задержка до запуска задания"), _target._timeDelay, 1f, 10f);
            _target._bgVolume = EditorGUILayout.Slider(new GUIContent("BG volume", "Громкость фоновой музыки во время обучения"), _target._bgVolume, .01f, 1f);

            #endregion

            #region Chapters

            GUILayout.Space(20);
            GUILayout.Label("Chapters", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 });
            GUILayout.Label("------------------------------------------------------------------", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 16 });

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_chapters);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(_onStudyComplete);

            serializedObject.ApplyModifiedProperties();

            #endregion

            GUILayout.Space(10);
            GUILayout.Label("------------------------------------------------------------------", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 16 });

            #region Save

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            base.serializedObject.ApplyModifiedProperties();

            #endregion

        }
    }
}


#endif