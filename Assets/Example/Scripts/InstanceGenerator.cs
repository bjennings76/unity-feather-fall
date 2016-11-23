using UnityEngine;

public class InstanceGenerator : MonoBehaviour {
  [SerializeField] private GameObject m_Prefab;
  [SerializeField, Range(0.1f, 10f)] private float m_Delay = 2;
  [SerializeField, Range(1f, 20f)] private float m_DestroyDelay = 10;
  [SerializeField] private bool m_RotateRandomly;
  [SerializeField] private Vector3 m_InitialVelocity = Vector3.zero;
  private float m_LastTime;

  private void Update() {
    if (m_LastTime + m_Delay > Time.time) {
      return;
    }

    m_LastTime = Time.time;
    GameObject instance = (GameObject)Instantiate(m_Prefab, transform.position, GetRotation());

    if (!m_InitialVelocity.Equals(Vector3.zero)) {
      Rigidbody rb = instance.GetComponent<Rigidbody>();
      rb.velocity = m_InitialVelocity;
    }

    Destroy(instance, m_DestroyDelay);
  }

  private Quaternion GetRotation() {
    return m_RotateRandomly ? Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)) : transform.rotation;
  }
}