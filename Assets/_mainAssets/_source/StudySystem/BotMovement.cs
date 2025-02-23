using UnityEngine;

namespace StudySystem
{
	public class BotMovement : MonoBehaviour
	{
		[SerializeField] private float MoveSpeed;
		[SerializeField] private float RotateSpeed;

		private bool _canMove = false;
		private bool _isFinish = false;
		
		private Transform _target;
		private float _time;

		private void Update()
		{
			if (_target == null) { return; }
			
			if (_isFinish) // Rotate
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, RotateSpeed * _time);

				if (transform.eulerAngles.y == _target.eulerAngles.y)
				{
					_target = null;
				}
			}
			else if (_canMove) // Not finished? Move to target
			{
				transform.position = Vector3.MoveTowards(transform.position, _target.position, MoveSpeed * Time.deltaTime);

				if (Vector3.Distance(transform.position, _target.position) == 0)
				{
					_isFinish = true;
					_time = 0;
				}
			}
			else if (_target != null) // Rotate to target before start move
			{
				Vector3 direction = _target.position - transform.position;
				
				if (direction == Vector3.zero) { _canMove = true; _time = 0; return; }
				
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotateSpeed * _time);
				
				if (transform.eulerAngles.y == targetRotation.eulerAngles.y)
				{
					_canMove = true;
					_time = 0;
				}
			}
			
			_time += Time.deltaTime;
		}

		public void Move(Transform point)
		{
			_canMove = false;
			_isFinish = false;
			
			_target = point;
		}

		public void Teleport(Transform point)
		{
			_target = null;
			
			transform.position = point.position;
			transform.rotation = point.rotation;
		}
	}
}