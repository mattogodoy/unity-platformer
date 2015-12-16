using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float damage = 10;
	public LayerMask whatToHit;
	public Transform bulletTrailPrefab;
	public Transform muzzleFlashPrefab;
	float timeToSpawn = 0;
	public float spawnRate = 10;
	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("FirePoint transform not found.");
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1 / fireRate;
				shoot ();
			}
		}
	}

	void shoot() {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);

		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

		// Limit the amounth of spawned bullet trails
		if (Time.time >= timeToSpawn) {
			effect ();
			timeToSpawn = Time.time + 1 / spawnRate;
		}

		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red, 1.0f);
			Debug.Log("We hit: " + hit.collider.name + " and did: " + damage + " damage.");
		}
	}

	void effect () {
		Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = (Transform)Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation);
		clone.parent = firePoint;
		float size = Random.Range (0.4f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 0.02f);
	}
}
