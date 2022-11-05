using UnityEngine;

namespace Enemies {
	public class EnemyTriggerSpawner : EnemySpawner {
		private bool started;
		
		private void OnTriggerEnter2D(Collider2D col) {
			if (started || !col.CompareTag("Player")) {
				return;
			}
			started = true;
			spawnEnemy();
		}

		protected internal override void enemyKilled() {
			StageController.getInstance().enemyKilled();
			Destroy(gameObject);
		}
	}
}