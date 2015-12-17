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

		public void Init() {
			currentHealth = maxHealth;
		}
	}

	public EnemyStats enemyStats = new EnemyStats ();

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
		enemyStats.Init ();

		if (statusIndicator != null) {
			statusIndicator.setHealth (enemyStats.currentHealth, enemyStats.maxHealth);
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
}
