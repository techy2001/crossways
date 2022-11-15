using UnityEngine;

public class Bullet : MonoBehaviour {
	private new SpriteRenderer renderer;
	public Sprite playerBullet;
	public Sprite enemyBullet;
	public Vector3 speed;
	public string target;
	private int lifetime = 100;
	
	private void Awake() {
		if (!renderer) {
			renderer = GetComponent<SpriteRenderer>();
		}
	}

	public void setTarget(string newTarget) {
		if (!renderer) {
			renderer = GetComponent<SpriteRenderer>();
		}
		if (!renderer) {
			return;
		}
		target = newTarget;
		renderer.sprite = target switch {
			"Player" => enemyBullet,
			"Enemy" => playerBullet,
			_ => renderer.sprite
		};
	}

	private void FixedUpdate() {
		calculateMovement();
		lifetime--;
		if (lifetime <= 0) {
			Destroy(gameObject);
		}
	}

	private void calculateMovement() {
		Transform transform1 = transform;
		Vector3 position = transform1.position;
		position += speed;
		transform1.position = position;
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (target != null && !col.gameObject.CompareTag(target)) {
			return;
		}
		Damageable damageable = col.gameObject.GetComponent<Damageable>();
		if (!damageable) {
			return;
		}
		damageable.damage(1);
		Destroy(gameObject);
	}
}