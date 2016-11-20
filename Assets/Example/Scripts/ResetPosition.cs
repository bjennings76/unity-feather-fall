using UnityEngine;

public class ResetPosition : MonoBehaviour {
	private Rigidbody m_Rigidbody;
	private Vector3 m_StartPosition;
	private Quaternion m_StartRotation;

	private void Start() {
		m_StartPosition = transform.position;
		m_StartRotation = transform.rotation;
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	private void Update() {
		if (Input.GetKeyUp(KeyCode.Space)) {
			transform.position = m_StartPosition;
			transform.rotation = m_StartRotation;

			if (m_Rigidbody) {
				m_Rigidbody.angularVelocity = Vector3.zero;
				m_Rigidbody.velocity = Vector3.zero;
			}
		}
	}
}