using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Awake () {
		if(gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public int spawnDelay = 2;
	public Transform spawnPrefab;
	public CameraShake cameraShake;

	void Start () {
		if (cameraShake == null) {
			Debug.LogError ("No camera shake referenced!");
		}
	}

	public IEnumerator _RespawnPlayer () {
		yield return new WaitForSeconds(spawnDelay);
		GetComponent<AudioSource>().Play ();
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = (Transform)Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy (clone.gameObject, 3f);
	}

	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		gm.StartCoroutine(gm._RespawnPlayer ());
	}

	public static void KillEnemy (Enemy enemy) {
		gm._killEnemy (enemy);
	}

	public void _killEnemy(Enemy _enemy) {
		Transform _clone = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy (_clone.gameObject, 3f);
		cameraShake.Shake (_enemy.shakeAmounth, _enemy.shakeLength);
		Destroy (_enemy.gameObject);
	}
}
