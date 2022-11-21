using UnityEngine;

public class Bullet : MonoBehaviour {
	private new SpriteRenderer renderer;
	public Sprite playerBullet;
	public Sprite enemyBullet;
	public Vector3 speed;
	public string target;
	private int lifetime = 100;
	
	private void Awake() {
		if (!this.renderer) {
			this.renderer = this.GetComponent<SpriteRenderer>();
		}
	}

	public void setTarget(string newTarget) {
		if (!this.renderer) {
			this.renderer = this.GetComponent<SpriteRenderer>();
		}
		if (!this.renderer) {
			return;
		}

		this.target = newTarget;
		this.renderer.sprite = this.target switch {
			"Player" => this.enemyBullet,
			"Enemy" => this.playerBullet,
			_ => this.renderer.sprite
		};
	}

	private void FixedUpdate() {
		this.calculateMovement();
		this.lifetime--;
		if (this.lifetime <= 0) {
			Destroy(this.gameObject);
		}
	}

	private void calculateMovement() {
		Transform transform1 = this.transform;
		Vector3 position = transform1.position;
		position += this.speed;
		transform1.position = position;
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (this.target != null && !col.gameObject.CompareTag(this.target)) {
			return;
		}
		Damageable damageable = col.gameObject.GetComponent<Damageable>();
		if (!damageable) {
			return;
		}
		damageable.damage(1);
		Destroy(this.gameObject);
	}
}