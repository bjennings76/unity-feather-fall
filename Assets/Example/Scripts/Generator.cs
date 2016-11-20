using UnityEngine;

public class Generator : MonoBehaviour {
	[SerializeField] private GameObject m_Prefab;
	[SerializeField, Range(0.1f, 10f)] private float m_Delay = 2;

	private float m_LastTime;

	private void Update() {
		if (m_LastTime + m_Delay > Time.time) {
			return;
		}

		m_LastTime = Time.time;
		Destroy(Instantiate(m_Prefab, transform, false), 10);
	}
}