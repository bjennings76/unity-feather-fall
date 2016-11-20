using UnityEngine;

public class ToggleScript : MonoBehaviour {
  [SerializeField] private Behaviour m_Target;
  [SerializeField] private KeyCode m_ToggleKey = KeyCode.F;

  // Update is called once per frame
  private void Update() {
    if (Input.GetKeyUp(m_ToggleKey)) {
      m_Target.enabled = !m_Target.enabled;
    }
  }
}