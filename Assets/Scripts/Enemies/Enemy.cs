using UnityEngine;

namespace Enemies {
	public class Enemy : MonoBehaviour {
		private EnemySpawner spawner;
		private new Rigidbody2D rigidbody;
		private new SpriteRenderer renderer;
		public float speed = 2;
		public int health = 10;

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

		public bool damage(int amount) {
			health -= amount;
			if (health <= 0) {
				onDeath();
			}
			return true;
		}

		private void onDeath() {
			spawner.enemyKilled();
			Destroy(gameObject);
		}

		public void setSpawner(EnemySpawner source) {
			spawner = source;
		}
	}
}