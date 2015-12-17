using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;		// Array of all the back and foregrounds to be parallaxed
	public float[] parallaxScales;		// The proportion of the camera's movement to move the backgrounds by
	public float smoothing = 1;			// How smooth the parallax is going to be. Make sure to set this above 0

	private Transform cam;				// Reference to the main camera's transform
	private Vector3 previousCamPos;		// The position of the camera in the previous frame

	// Called before Start()
	void Awake () {
		// Setup camera reference
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		// The previous frame had the curren camera position
		previousCamPos = cam.position;

		parallaxScales = new float[backgrounds.Length];

		// Asigning corresponding parallax scales
		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			// The parallax is the opposite of the camera movement
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			// Set a target exposition which is the current position + the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			// Create a target position which is the background's current position with it's target x position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			// Fade between current position and the target's position using "lerp"
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		// Set the previous camera position to it's position at the end of the frame
		previousCamPos = cam.position;
	}
}
