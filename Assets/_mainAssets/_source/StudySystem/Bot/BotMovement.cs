using UnityEngine;

namespace StudySystem
{
	public class BotMovement : MonoBehaviour
	{
		[SerializeField] private float MoveSpeed;
		[SerializeField] private float RotateSpeed;
		
		private Transform _target;
		private MovingState _state;
		
		private float _time;

		private void Update()
		{
			if (_target == null)
				return;
			
			switch (_state)
			{
				case MovingState.ViewToTarget:
				
					Vector3 direction = _target.position - transform.position;
					if (Rotate(Quaternion.LookRotation(direction)))
						_state = MovingState.Move;
					
					break;
				case MovingState.Move:
				
					transform.position = Vector3.MoveTowards(transform.position, _target.position, MoveSpeed * Time.deltaTime);

					if (Vector3.Distance(transform.position, _target.position) == 0)
					{
						_time = 0;
						_state = MovingState.RotateToPoint;
					}
				
					break;
				case MovingState.RotateToPoint:
				
					if (Rotate(_target.rotation))
						_state = MovingState.None;
					
					break;
				default:
					break;
			}
			
			_time += Time.deltaTime;
		}

		public void Move(Transform point)
		{
			_target = point;
			_time = 0;
			
			_state = MovingState.ViewToTarget;
		}

		public void Teleport(Transform point)
		{
			_target = null;
			
			transform.position = point.position;
			transform.rotation = point.rotation;
		}

		private bool Rotate(Quaternion target)
		{			
			transform.rotation = Quaternion.Lerp(transform.rotation, target, RotateSpeed * _time);
			
			if (transform.eulerAngles.y == target.eulerAngles.y)
			{
				_time = 0;
				return true;
			}
			
			return false;
		}
	}
}