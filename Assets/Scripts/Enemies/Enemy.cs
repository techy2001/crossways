using Player;
using UnityEngine;

namespace Enemies {
	public class Enemy : Damageable {
		private EnemySpawner spawner;
		private new Rigidbody2D rigidbody;
		private new SpriteRenderer renderer;
		public float speed = 2;
		private bool isDead;

		private void Awake(){
			this.rigidbody = this.GetComponent<Rigidbody2D>();
			this.renderer = this.GetComponent<SpriteRenderer>();
		}

		private void FixedUpdate() {
			this.calculateMovement();
			this.decideFlip();
		}

		protected virtual void calculateMovement() {
			PlayerController player = StageController.getInstance().getPlayer();
			if (player) {
				Vector3 playerPos = player.gameObject.transform.position;
				Vector3 enemyPos = this.transform.position;
				Vector2 playerAim = new Vector2(playerPos.x - enemyPos.x, playerPos.y - enemyPos.y);
				this.rigidbody.velocity = playerAim.normalized * this.speed;
			} else {
				this.rigidbody.velocity = Vector2.zero;
			}
		}

		protected virtual void decideFlip() {
			this.renderer.flipX = this.rigidbody.velocity.x switch {
				< 0 => true,
				> 0 => false,
				_ => this.renderer.flipX
			};
		}

		private void OnCollisionStay2D(Collision2D col) {
			PlayerController player = StageController.getInstance().getPlayer();
			if (player && col.gameObject.CompareTag("Player")) {
				player.damage(1);
			}
		}

		protected override void onDeath() {
			if (this.isDead) {
				return;
			}

			this.isDead = true;
			this.spawner.enemyKilled();
			Destroy(this.gameObject);
		}

		public void setSpawner(EnemySpawner source) {
			this.spawner = source;
		}
	}
}