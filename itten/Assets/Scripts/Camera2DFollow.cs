using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
	public Transform Target;
	public float XDamping = 0.5f;
	public float YDamping = 0.1f;
	public float LookAheadFactor = 3.0f;
	public float LookAheadReturnSpeed = 0.5f;
	public float LookAheadMoveThreshold = 0.1f;

	private float OffsetZ;
	private Vector3 LastTargetPosition;
	private Vector3 XCurrentVelocity;
	private Vector3 YCurrentVelocity;
	private Vector3 LookAheadPos;

	private void Start () {
		LastTargetPosition = Target.position;
		OffsetZ = (transform.position - Target.position).z;
		transform.parent = null;
	}

	private void Update () {
		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (Target.position - LastTargetPosition).x;

		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadMoveThreshold;

		if (updateLookAheadTarget) {
			LookAheadPos = LookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		} else {
			LookAheadPos = Vector3.MoveTowards(LookAheadPos, Vector3.zero, Time.deltaTime * LookAheadReturnSpeed);
		}

		Vector3 aheadTargetPos = Target.position + LookAheadPos + Vector3.forward * OffsetZ;
		Vector3 newPosX = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref XCurrentVelocity, XDamping);
		Vector3 newPosY = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref YCurrentVelocity, YDamping);

		transform.position = new Vector3(newPosX.x, newPosY.y, transform.position.z);

		LastTargetPosition = Target.position;
	}
}
