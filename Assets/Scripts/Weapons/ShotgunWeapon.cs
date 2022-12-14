using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons {
	public class ShotgunWeapon : Weapon {
		private new SpriteRenderer renderer;
		private new AudioSource audio;
		public AudioClip shoot;
		public AudioClip empty;
		public AudioClip throwProjectile;
		public AudioClip catchProjectile;
		public bool held = true;
		private int ammo = 6;
		public Bullet bullet;
		public ShotgunProjectile projectile;
		private int fireCooldown;

		private void Awake(){
			this.renderer = this.GetComponent<SpriteRenderer>();
			this.audio = this.GetComponent<AudioSource>();
		}

		public override void tick() {
			if (this.fireCooldown > 0) {
				this.fireCooldown--;
			}
		}

		public override void primaryFire(GameObject owner, Vector3 from, Vector3 direction, string target) {
			if (!this.held || this.fireCooldown > 0) {
				return;
			}
			if (this.ammo <= 0) {
				this.audio.PlayOneShot(this.empty);
				this.fireCooldown = 30;
				return;
			}
			const float speed = 0.6f;
			this.bullet.setTarget(target);
			const float spread = 0.4f;
			for (int i = 0; i < 9; i++) {
				Vector3 newDirection = direction.normalized;
				newDirection += (new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * spread);
				newDirection *= speed;
				this.bullet.speed = newDirection;
				Instantiate(this.bullet.gameObject, from, Quaternion.identity);
			}

			StageController.getInstance().cameraAmp += 1;
			this.audio.PlayOneShot(this.shoot);
			this.ammo--;
			this.fireCooldown = 30;
		}

		public override void secondaryFire(GameObject owner, Vector3 from, Vector3 direction, string target) {
			if (!this.held) {
				return;
			}

			this.audio.PlayOneShot(this.throwProjectile);
			const float speed = 0.8f;
			this.projectile.speed = direction.normalized * speed;
			this.projectile.setTarget(target);
			this.projectile.owner = owner;
			GameObject spawnedEnemy = Instantiate(this.projectile.gameObject);
			spawnedEnemy.transform.position = from;
			this.ammo = 6;
			this.held = false;
			this.renderer.enabled = this.held;
		}

		public override SpriteRenderer getRenderer() {
			return this.renderer;
		}
		
		private void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.CompareTag("Player")) {
				StageController.getInstance().getPlayer().weapon = this;
			}
		}

		public void playCatchSound() {
			this.audio.PlayOneShot(this.catchProjectile);
		}
	}
}