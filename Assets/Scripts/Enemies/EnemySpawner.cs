using UnityEngine;

namespace Enemies {
	public class EnemySpawner : MonoBehaviour {
		[SerializeField] protected Enemy enemyReference;
		private Room room;

		public void setRoom(Room newRoom) {
			room = newRoom;
		}

		public void spawnEnemy() {
			GameObject spawnedEnemy = Instantiate(enemyReference.gameObject);
			spawnedEnemy.GetComponent<Enemy>().setSpawner(this);
			spawnedEnemy.transform.position = transform.position;
			StageController.getInstance().livingEnemies += 1;
		}
		
		protected internal virtual void enemyKilled() {
			room.enemyKilled();
			Destroy(gameObject);
		}
	}
}