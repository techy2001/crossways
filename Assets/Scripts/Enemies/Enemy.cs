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
			rigidbody = GetComponent<Rigidbody2D>();
			renderer = GetComponent<SpriteRenderer>();
		}

		private void FixedUpdate() {
			calculateMovement();
			decideFlip();
		}

		protected virtual void calculateMovement() {
			PlayerController player = StageController.getInstance().getPlayer();
			if (player) {
				Vector3 playerPos = player.gameObject.transform.position;
				Vector3 enemyPos = transform.position;
				Vector2 playerAim = new Vector2(playerPos.x - enemyPos.x, playerPos.y - enemyPos.y);
				rigidbody.velocity = playerAim.normalized * speed;
			} else {
				rigidbody.velocity = Vector2.zero;
			}
		}

		protected virtual void decideFlip() {
			renderer.flipX = rigidbody.velocity.x switch {
				< 0 => true,
				> 0 => false,
				_ => renderer.flipX
			};
		}

		private void OnCollisionStay2D(Collision2D col) {
			PlayerController player = StageController.getInstance().getPlayer();
			if (player && col.gameObject.CompareTag("Player")) {
				player.damage(1);
			}
		}

		protected override void onDeath() {
			if (isDead) {
				return;
			}
			isDead = true;
			spawner.enemyKilled();
			Destroy(gameObject);
		}

		public void setSpawner(EnemySpawner source) {
			spawner = source;
		}
	}
}