using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	public int maxLives = 3;

	private static int _remainingLives;
	public static int RemainingLives {
		get { return _remainingLives; }
	}

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

	[SerializeField]
	private GameObject gameOverUI;

	void Start () {
		if (cameraShake == null) {
			Debug.LogError ("No camera shake referenced!");
		}

		_remainingLives = maxLives;
	}

	public void EndGame() {
		Debug.Log ("GAME OVER");
		gameOverUI.SetActive (true);
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
		_remainingLives--;

		if (_remainingLives <= 0) {
			gm.EndGame ();
		} else {
			gm.StartCoroutine(gm._RespawnPlayer ());
		}
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
