using Player;
using UnityEngine;

namespace Enemies {
	public class ShootingEnemy : Enemy {
		public Bullet bulletPrefab;
		public float retreatDistance = 4.0f;
		public float fireRate = 1.0f;
		public float fireTimer;

		protected new void Awake() {
			base.Awake();
		}

		private void FixedUpdate() {
			PlayerController player = StageController.getInstance().getPlayer();
			float distance = Vector3.Distance(this.transform.position, player.transform.position);
			if (distance > this.retreatDistance * 2) {
				this.calculateMovement();
			}
			this.fireTimer -= Time.deltaTime; 
			if (distance < this.retreatDistance) {
				Vector3 playerPos = player.gameObject.transform.position;
				Vector3 enemyPos = this.transform.position;
				Vector2 playerAim = new Vector2(-playerPos.x +enemyPos.x, -playerPos.y + enemyPos.y);
				this.rigidbody.velocity = playerAim.normalized * this.speed;
			} else {
				if (this.fireTimer <= 0.0f) {
					this.fireTimer = this.fireRate;
					Vector3 difference = player.transform.position - this.transform.position;
					difference = new Vector3(difference.x, difference.y, 0).normalized;
					this.bulletPrefab.speed = difference * 0.2f;
					this.bulletPrefab.target = "Player";
					Instantiate(this.bulletPrefab.gameObject, this.transform.position, Quaternion.identity);
				}
			}
			this.decideFlip();
		}

		private void OnCollisionStay2D(Collision2D col) {
			PlayerController player = StageController.getInstance().getPlayer();
			if (player && col.gameObject.CompareTag("Player")) {
				player.damage(1);
			}
		}
	}
}