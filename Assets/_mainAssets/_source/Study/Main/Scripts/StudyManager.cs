using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	[RequireComponent(typeof(AudioSource))]
	public class StudyManager : MonoBehaviour
	{
		#region Params

		public StudyChapter[] Chapters;
		public AudioSource BackgroundMusic;
		public AudioSource _audioSource;

		public float _timeDelay = 1f;

		public float _bgVolume = .03f;
		private float _bgStandartVolume;

		public UnityEvent OnStudyComplete;

		public static StudyManager Instance;

		private int _currentChapter;
		private int _currentLevel;
		private int _currentStage = -1;

		#endregion

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				_bgStandartVolume = BackgroundMusic.volume;

				StartNextStage(1f);
			}
		}

		public void StartNextStage()
		{
			StartCoroutine(NextStage(3f));
		}

		public void StartNextStage(float time, GameObject _object = null)
		{
			StartCoroutine(NextStage(time, _object));
		}

		private IEnumerator NextStage(float time, GameObject _object = null)
		{
			StudyStage _stage = new StudyStage();

			if (_object != null) // Set to completed
			{
				_stage = Chapters[_currentChapter].Levels[_currentLevel].Stages[_currentStage];

				for (int i = 0; i < _stage.Quests.Length; i++)
				{
					if (_stage.Quests[i].Object == _object)
					{
						_stage.Quests[i]._isCompleted = true;
						break;
					}
				}
			}

			yield return ChangeStage(_stage, time, _object == null);
		}

		private IEnumerator ChangeStage(StudyStage _stage, float time, bool _skip)
		{
			if (_skip || _stage._completed == _stage.Quests.Length)
			{
				if (_currentStage < Chapters[_currentChapter].Levels[_currentLevel].Stages.Length - 1)
				{
					_currentStage++;
				} // New stage
				else if (_currentLevel < Chapters[_currentChapter].Levels.Length - 1)
				{
					//new lvl
					_currentLevel++;
					_currentStage = 0;
				} // New Level
				else
				{
					//new chapter
					if (_currentChapter < Chapters.Length - 1)
					{
						_currentChapter++;
						_currentLevel = 0;
						_currentStage = 0;
					}
					else
					{
						OnStudyComplete?.Invoke();
					}
				} // New chapter or end

				yield return new WaitForSeconds(time);

				StudyStage _newStage = Chapters[_currentChapter].Levels[_currentLevel].Stages[_currentStage];

				_newStage.Audio.StartAudio?.Invoke();

				yield return PlaySound(_newStage.Audio);

				_newStage.Audio.EndAudio?.Invoke();
				_newStage.StartStage();
			}
		}

		private IEnumerator PlaySound(StudyAudio _currentLevel)
		{
			if (_currentLevel.Audio == null) { yield break; }

			_audioSource.PlayOneShot(_currentLevel.Audio);

			foreach (AudioEvent _event in _currentLevel.Events)
			{
				StartCoroutine(StartSoundEvent(_event));
			}

			BackgroundMusic.volume = _bgVolume;

			yield return new WaitForSeconds(_currentLevel.Audio.length + _timeDelay);

			BackgroundMusic.volume = _bgStandartVolume;
		}

		private IEnumerator StartSoundEvent(AudioEvent _event)
		{
			yield return new WaitForSeconds(_event._time);
			_event._events?.Invoke();
		}

	}

	[System.Serializable]
	public struct StudyChapter
	{
		public string ChapterName;
		public StudyLevel[] Levels;
	}

	[System.Serializable]
	public struct StudyLevel
	{
		public string LevelName;
		public StudyStage[] Stages;
	}

	[System.Serializable]
	public struct StudyStage
	{
		public string Name;
		public StudyAudio Audio;
		public Quest[] Quests;

		public int _completed
		{
			get
			{
				int _result = 0;

				foreach (Quest quest in Quests)
				{
					_result += quest._isCompleted ? 1 : 0;
				}
				return _result;
			}
		}

		public void StartStage()
		{
			try
			{
				foreach (Quest _quest in Quests)
				{
					_quest.Object.GetComponent<IStudy>().StartStudy();
				}
			}
			catch
			{
				Debug.LogError("WTF object without Study interface");
			}
		}

	}

	[System.Serializable]
	public struct StudyAudio
	{
		public AudioClip Audio;

		public UnityEvent StartAudio;
		public UnityEvent EndAudio;

		public AudioEvent[] Events;
	}

	[System.Serializable]
	public struct AudioEvent
	{
		public int _time;
		public UnityEvent _events;
	}

	[System.Serializable]
	public struct Quest
	{
		public GameObject Object;
		[HideInInspector]
		public bool _isCompleted;
	}

	public interface IStudy
	{
		public void StartStudy();
	}
}