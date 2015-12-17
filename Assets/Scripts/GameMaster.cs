using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Start () {
		if(gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public int spawnDelay = 2;
	public Transform spawnPrefab;

	public IEnumerator RespawnPlayer () {
		yield return new WaitForSeconds(spawnDelay);
		GetComponent<AudioSource>().Play ();
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = (Transform)Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy (clone.gameObject, 3f);
	}

	public static void KillPlayer (Player player) {
		Debug.Log ("Player is dead!");
		Destroy (player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer ());
	}



	public static void KillEnemy (Enemy enemy) {
		Debug.Log ("Enemy killed!");
		Destroy (enemy.gameObject);
	}
}
