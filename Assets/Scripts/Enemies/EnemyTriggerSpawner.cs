using UnityEngine;

namespace Enemies {
	public class EnemyTriggerSpawner : EnemySpawner {
		private bool started;
		
		private void OnTriggerEnter2D(Collider2D col) {
			if (this.started || !col.CompareTag("Player")) {
				return;
			}

			this.started = true;
			this.spawnEnemy();
		}

		protected internal override void enemyKilled() {
			StageController.getInstance().enemyKilled();
			Destroy(this.gameObject);
		}
	}
}