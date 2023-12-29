using UnityEngine;
using UnityEngine.Events;

namespace StudySystem.Bot
{
    [RequireComponent(typeof(AudioSource))]
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private float MoveSpeed;
        [SerializeField] private float RotateSpeed;
        [SerializeField] private float StudyDistance;
        [SerializeField, HideInInspector] private UnityEvent OnMoveComplete = new UnityEvent();
        private AudioSource _audio;

        private bool _canMove = false;
        private bool _isFinish = false;
        private bool _startNext = false;
        private Transform _target;

        private Quaternion _lookRotation;
        private Vector3 _direction;
        private Transform _player;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (_target == null && _startNext && Vector3.Distance(transform.position, _player.position) <= 10f)
            {
                StudyManager.Instance.StartNextStage(1f);
                _startNext = false;
            }

            if (_target != null && _isFinish)
            {
                _direction = (_target.position - transform.position).normalized;

                _lookRotation = _target.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, RotateSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.eulerAngles.y - _target.eulerAngles.y) <= .1f)
                {
                    _target = null;
                    _isFinish = false;
                    OnMoveComplete?.Invoke();
                }
            }
            else if (_canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, MoveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, _target.position) == 0)
                {
                    _isFinish = true;
                    _canMove = false;
                    _audio.Stop();
                }
            }
            else if (_target != null)
            {
                _direction = (_target.position - transform.position).normalized;

                _lookRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, RotateSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) <= .1f)
                {
                    _canMove = true;
                    _audio.Play();
                }
            }
        }

        public void Move(Transform _point)
        {
            _canMove = false;
            _isFinish = false;

            if (_startNext)
            {
                _startNext = false;
            }

            _target = _point;
            _audio.Stop();
        }

        public void Teleport(Transform _point)
        {
            transform.position = _point.position;
            transform.rotation = _point.rotation;
        }

        public void StudyMove(Transform _point)
        {
            _canMove = false;
            _isFinish = false;

            _target = _point;

            _startNext = true;
            _audio.Stop();
        }

    }
}
