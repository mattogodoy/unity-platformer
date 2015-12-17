using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Camera cam;

	float shakeAmount = 0;

	void Wake () {
		if (cam == null) {
			cam = Camera.main;
		}
	}

	public void Shake(float amount, float length) {
		shakeAmount = amount;
		InvokeRepeating ("BeginShake", 0, 0.01f);
		Invoke ("StopShake", length);
	}

	void BeginShake() {
		if (shakeAmount > 0) {
			Vector3 camPosition = cam.transform.position;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

			camPosition.x += offsetX;
			camPosition.y += offsetY;

			cam.transform.position = camPosition;
		}
	}

	void StopShake() {
		CancelInvoke ("BeginShake");
		cam.transform.localPosition = Vector3.zero;
	}
}
