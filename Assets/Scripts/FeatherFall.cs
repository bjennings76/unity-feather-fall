using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(ConstantForce))]
public class FeatherFall : MonoBehaviour {
	[SerializeField, Range(0.01f, 1f)] private float m_FloatForce = 0.8f;
	[SerializeField, Range(0.01f, 1f)] private float m_SlidePower = 0.2f;
	[SerializeField, Range(0.01f, 1f)] private float m_PuffPower = 0.05f;
	[SerializeField, Range(0.01f, 1f)] private float m_PuffDelayMin = 0.2f;
	[SerializeField, Range(0.01f, 1f)] private float m_PuffDelayMax = 0.3f;

	private Rigidbody m_Rigidbody;
	private BoxCollider m_Collider;
	private Vector3 m_AntigravityForce;
	private float m_LastTime;
	private float m_Delay;
	private Vector3[] m_EdgePoints;
	private Vector3 m_SlideVector;
	private Vector3 m_LastPuffPosition;
	private Vector3 m_LastPuffPower;

	private void Start() {
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Collider = GetComponent<BoxCollider>();
		m_AntigravityForce = GetAntigravityForce();
		GetComponent<ConstantForce>().force = m_AntigravityForce*m_FloatForce;

		Vector3 center = m_Collider.center;
		Vector3 size = m_Collider.size/2;

		m_EdgePoints = new[] {
			new Vector3(      0, 0, -size.z) + center,  //bottom
			new Vector3(      0, 0,  size.z) + center,  //top
			new Vector3(-size.x, 0,       0) + center,  //left
			new Vector3( size.x, 0,       0) + center,  //right
			new Vector3(-size.x, 0, -size.z) + center,  //bottom left
			new Vector3( size.x, 0, -size.z) + center,  //bottom right
			new Vector3(-size.x, 0,  size.z) + center,  //top left
			new Vector3( size.x, 0,  size.z) + center   //top right
		};
	}

	private void Update() {
		UpdateSlide();
		UpdatePuffs();
	}

	private void UpdateSlide() {
		Vector3 normal = transform.up;
		m_SlideVector.x = normal.x*normal.y;
		m_SlideVector.z = normal.z*normal.y;
		m_SlideVector.y = -(normal.x*normal.x) - normal.z*normal.z;
		m_Rigidbody.AddForce(m_SlideVector.normalized*m_SlidePower);
	}

	private void UpdatePuffs() {
		if (m_LastTime + m_Delay < Time.time) {
			Puff();
		}
	}

	private void Puff() {
		float velocity = m_Rigidbody.velocity.y < 0 ? m_Rigidbody.velocity.magnitude : m_Rigidbody.velocity.magnitude * 0.1f;

	  if (velocity < 0.001f) {
	    return;
	  }

	  Vector3 puffPosition = GetPuffPosition();
	  m_LastPuffPosition = transform.InverseTransformPoint(puffPosition);

	  Vector3 pushAngle = Vector3.Dot(transform.up, Vector3.down) > 0 ? -Vector3.up : Vector3.up;
	  m_LastPuffPower = pushAngle*m_AntigravityForce.magnitude*m_PuffPower*velocity/10;

	  Vector3 powerVector = transform.TransformDirection(m_LastPuffPower);
	  m_Rigidbody.AddForceAtPosition(powerVector, puffPosition, ForceMode.Impulse);

	  m_LastTime = Time.time;
	  m_Delay = Random.Range(m_PuffDelayMin, m_PuffDelayMax);
	}

	private Vector3 GetPuffPosition() {
		Vector3 worldOffset = m_Collider.bounds.center;
		List<Vector3> worldEdges = m_EdgePoints.Select<Vector3, Vector3>(transform.TransformPoint).ToList();
		List<Vector3> validEdges = worldEdges.Where(v => v.y <= worldOffset.y).ToList();

		if (validEdges.Count == 0) {
			validEdges = worldEdges;
		}

		int edgeIndex = Random.Range(0, validEdges.Count - 1);
		return validEdges[edgeIndex];
	}

	private Vector3 GetAntigravityForce() {
		float totalMass = transform.GetComponentsInChildren<Rigidbody>().Sum(rb => rb.mass);
		return Physics.gravity*totalMass*-1f;
	}

	private void OnDrawGizmos() {
		if (!Application.isPlaying) {
			return;
		}

		// Draw Slide Vector
		Debug.DrawRay(transform.position, m_SlideVector, Color.blue);

		// Draw Last Puff Vector
		Vector3 puffPosition = transform.TransformPoint(m_LastPuffPosition);
		Color puffColor = Color.white;
		float timeSinceLast = Time.time - m_LastTime;
		puffColor.a = 1 - timeSinceLast/m_Delay;
		Vector3 powerVector = transform.TransformDirection(m_LastPuffPower);
		Debug.DrawRay(puffPosition, powerVector * 10, puffColor);

		// Draw Edge Points
		foreach (Vector3 edgePoint in m_EdgePoints) {
			Vector3 edgePosition = transform.TransformPoint(edgePoint);
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(edgePosition, 0.01f);
		}
	}
}