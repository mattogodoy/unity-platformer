using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;

		private int _currentHealth;
		public int currentHealth {
			get { return _currentHealth; }
			set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
		}

		public int damage = 40;

		public void Init() {
			currentHealth = maxHealth;
		}
	}

	public EnemyStats enemyStats = new EnemyStats ();
	public Transform deathParticles;
	public float shakeAmounth = 0.1f;
	public float shakeLength = 0.1f;

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
		enemyStats.Init ();

		if (statusIndicator != null) {
			statusIndicator.setHealth (enemyStats.currentHealth, enemyStats.maxHealth);
		}

		if (deathParticles == null) {
			Debug.LogError ("No death particles referenced!");
		}
	}

	public void DamageEnemy(int damage) {
		enemyStats.currentHealth -= damage;
		if (enemyStats.currentHealth <= 0) {
			GameMaster.KillEnemy (this);
		}

		if (statusIndicator != null) {
			statusIndicator.setHealth (enemyStats.currentHealth, enemyStats.maxHealth);
		}
	}

	void OnCollisionEnter2D(Collision2D _collInfo) {
		Player _player = _collInfo.collider.GetComponent<Player> ();
		if (_player != null) {
			_player.DamagePlayer (enemyStats.damage);
			DamageEnemy (99999);
		}
	}
}
