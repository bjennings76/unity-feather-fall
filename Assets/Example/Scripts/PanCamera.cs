using UnityEngine;

public class PanCamera : MonoBehaviour {
  [SerializeField] private int m_PixelsPerUnit = 100;
  private Vector3 m_LastMouse;

  private void Update() {
    if (!Input.GetMouseButton(2)) {
      m_LastMouse = Input.mousePosition;
      return;
    }

    Vector3 mouseMove = Input.mousePosition - m_LastMouse;
    Vector3 translation = new Vector3(-mouseMove.x/m_PixelsPerUnit, -mouseMove.y/m_PixelsPerUnit, 0);
    transform.Translate(translation, Space.Self);
    m_LastMouse = Input.mousePosition;
  }
}