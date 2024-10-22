﻿using UnityEngine;

namespace MonoBehaviours {
  public class MouseWorldPosition : MonoBehaviour {
    public static MouseWorldPosition Instance { get; private set; }
    private void Awake() => Instance = this;

    public Vector3 GetPosition() {
      Ray mouseCameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      Plane plane = new(Vector3.up, Vector3.zero);

      if (plane.Raycast(mouseCameraRay, out float distance)) {
        return mouseCameraRay.GetPoint(distance);
      }

      return Vector3.zero;
    }
  }
}