using UnityEngine;

namespace Enemies {
	public class EnemySpawner : MonoBehaviour {
		[SerializeField] protected Enemy enemyReference;
		private Room room;

		public void setRoom(Room newRoom) {
			this.room = newRoom;
		}

		public void spawnEnemy() {
			GameObject spawnedEnemy = Instantiate(this.enemyReference.gameObject);
			spawnedEnemy.GetComponent<Enemy>().setSpawner(this);
			spawnedEnemy.transform.position = this.transform.position;
			StageController.getInstance().addLivingEnemy();
		}
		
		protected internal virtual void enemyKilled() {
			this.room.enemyKilled();
			Destroy(this.gameObject);
		}
	}
}