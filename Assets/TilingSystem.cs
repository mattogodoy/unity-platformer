using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class TilingSystem : MonoBehaviour {

	public int offsetX = 2;

	// To check if we need to instatiate new sprites
	public bool hasRightBuddy = false;
	public bool hasLeftBuddy = false;

	public bool reverseScale = false; // Used when the object is not tileable

	private float spriteWidth = 0f; // Width of the element
	private Camera cam;
	private Transform myTransform;

	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>(); // Returns the first one
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		// Does it still need buddies? If not, do nothing
		if (hasLeftBuddy == false || hasRightBuddy == false) {
			// Calculate the camera extend (half the width)
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			// Calculate the X position where the camera can see the edge of the sprite
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;
		
			// Check if we can see the edge of the element and then creating a new buddy
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasRightBuddy == false) {
				makeNewBuddy (1);
				hasRightBuddy = true;
			} else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasLeftBuddy == false) {
				makeNewBuddy (-1);
				hasLeftBuddy = true;
			}
		}
	}

	// Creates a buddy on the required side
	// NOTE: A buddy is a copy of a sprite to give landscape continuity
	void makeNewBuddy (int rightOrLeft) {
		// Calculating the new position for the new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		// Instantiate new buddy
		Transform newBuddy = (Transform) Instantiate (myTransform, newPosition, myTransform.rotation);

		// If not tileable, reverse the X scale of the sprite
		if (reverseScale == true) {
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = myTransform.parent;
		if (rightOrLeft > 0) {
			newBuddy.GetComponent<TilingSystem>().hasLeftBuddy = true;
		} else {
			newBuddy.GetComponent<TilingSystem>().hasRightBuddy = true;
		}
	}
}
