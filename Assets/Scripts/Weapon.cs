using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public int damage = 10;
	public LayerMask whatToHit;
	public Transform bulletTrailPrefab;
	public Transform hitPrefab;
	public Transform muzzleFlashPrefab;
	float timeToSpawn = 0;
	public float spawnRate = 10;

	// Handle camera shaking
	public float camShakeAmount = 0.05f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;

	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("FirePoint transform not found.");
		}
	
	}

	void Start () {
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		if (camShake == null) {
			Debug.LogError ("No camera shake found!");
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

		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red, 1.0f);

			Enemy enemy = hit.collider.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.DamageEnemy (damage);
				// Debug.Log("We hit: " + hit.collider.name + " and did: " + damage + " damage.");
			}
		}

		// Limit the amounth of spawned bullet trails
		if (Time.time >= timeToSpawn) {
			Vector3 hitPos;
			Vector3 hitNormal;

			if (hit.collider == null) {
				hitPos = (mousePosition - firePointPosition) * 30;
				hitNormal = new Vector3 (9999, 9999, 9999);
			} else {
				hitPos = hit.point;
				hitNormal = hit.normal;
			}

			Effect (hitPos, hitNormal);
			timeToSpawn = Time.time + 1 / spawnRate;
		}
	}

	void Effect (Vector3 hitPos, Vector3 hitNormal) {
		Transform trail = (Transform)Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation);
		LineRenderer lr = trail.GetComponent<LineRenderer> ();

		if (lr != null) {
			lr.SetPosition (0, firePoint.position);
			lr.SetPosition (1, hitPos);
		}
		Destroy (trail.gameObject, 0.04f);

		if (hitNormal != new Vector3 (9999, 9999, 9999)) {
			Transform hitParticle = (Transform)Instantiate (hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
			Destroy (hitParticle.gameObject, 1f);
		}

		Transform clone = (Transform)Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation);
		clone.parent = firePoint;
		float size = Random.Range (0.4f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 0.02f);

		// Shake the camera
		camShake.Shake(camShakeAmount, camShakeLength);
	}
}
