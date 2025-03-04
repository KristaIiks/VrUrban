using UnityEngine;

public class TargetViewFollow : MonoBehaviour
{
	[SerializeField] private Transform Target;
	[SerializeField] private Vector3 Vector;
	
	void Update()
	{
		Vector3 vectorTarget = Target.position - transform.position;

		float rotationModifier = 90;
		float speedTurn = 6;
		float angle = Mathf.Atan2(-vectorTarget.z, vectorTarget.x) * Mathf.Rad2Deg - rotationModifier;
		Quaternion q = Quaternion.AngleAxis(angle, Vector);
		transform.localRotation = Quaternion.Lerp(transform.localRotation, q, Time.deltaTime * speedTurn);
	}
}