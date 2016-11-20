using UnityEngine;

public class FlyCamera : MonoBehaviour {
	[SerializeField] private float m_MainSpeed = 100.0f; //regular speed
	[SerializeField] private float m_ShiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	[SerializeField] private float m_MaxShift = 1000.0f; //Maximum speed when holdin gshift
	[SerializeField] private float m_CamSens = 0.25f; //How sensitive it with mouse

	private Vector3 m_LastMouse = new Vector3(255, 255, 255);
		//kind of in the middle of the screen, rather than at the top (play)

	private float m_TotalRun = 1.0f;

	private void OnEnable() {
		m_LastMouse = Input.mousePosition;
	}

	private void Update() {
		m_LastMouse = Input.mousePosition - m_LastMouse;
		m_LastMouse = new Vector3(-m_LastMouse.y*m_CamSens, m_LastMouse.x*m_CamSens, 0);
		m_LastMouse = new Vector3(transform.eulerAngles.x + m_LastMouse.x, transform.eulerAngles.y + m_LastMouse.y, 0);
		transform.eulerAngles = m_LastMouse;
		m_LastMouse = Input.mousePosition;
		//Mouse  camera angle done.  

		//Keyboard commands
		Vector3 p = GetBaseInput();
		if (Input.GetKey(KeyCode.LeftShift)) {
			m_TotalRun += Time.deltaTime;
			p = p*m_TotalRun*m_ShiftAdd;
			p.x = Mathf.Clamp(p.x, -m_MaxShift, m_MaxShift);
			p.y = Mathf.Clamp(p.y, -m_MaxShift, m_MaxShift);
			p.z = Mathf.Clamp(p.z, -m_MaxShift, m_MaxShift);
		} else {
			m_TotalRun = Mathf.Clamp(m_TotalRun*0.5f, 1f, 1000f);
			p = p*m_MainSpeed;
		}

		p = p*Time.deltaTime;
		Vector3 newPosition = transform.position;
		if (Input.GetKey(KeyCode.Space)) {
			//If player wants to move on X and Z axis only
			transform.Translate(p);
			newPosition.x = transform.position.x;
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		} else {
			transform.Translate(p);
		}
	}

	private static Vector3 GetBaseInput() {
		//returns the basic values, if it's 0 than it's not active.
		Vector3 velocity = new Vector3();
		if (Input.GetKey(KeyCode.W)) {
			velocity += new Vector3(0, 0, 1);
		}
		if (Input.GetKey(KeyCode.S)) {
			velocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.A)) {
			velocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.D)) {
			velocity += new Vector3(1, 0, 0);
		}
		return velocity;
	}
}