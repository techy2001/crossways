using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons {
	public class ShotgunWeapon : Weapon {
		private new SpriteRenderer renderer;
		private new AudioSource audio;
		private AudioClip shoot;
		private AudioClip empty;
		private AudioClip throwProjectile;
		private AudioClip catchProjectile;
		public bool held = true;
		private int ammo = 6;
		public Bullet bullet;
		public ShotgunProjectile projectile;
		private int fireCooldown;

		private void Awake(){
			renderer = GetComponent<SpriteRenderer>();
			audio = GetComponent<AudioSource>();
		}

		public override void tick() {
			if (fireCooldown > 0) {
				fireCooldown--;
			}
		}

		public override void primaryFire(GameObject owner, Vector3 from, Vector3 direction, string target) {
			if (!held || fireCooldown > 0) {
				return;
			}
			if (ammo <= 0) {
				audio.clip = empty;
				audio.Play();
				return;
			}
			const float speed = 0.6f;
			bullet.setTarget(target);
			const float spread = 0.4f;
			for (int i = 0; i < 9; i++) {
				Vector3 newDirection = direction.normalized;
				newDirection += (new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * spread);
				newDirection *= speed;
				bullet.speed = newDirection;
				GameObject spawnedEnemy = Instantiate(bullet.gameObject);
				spawnedEnemy.transform.position = from;
			}
			audio.clip = shoot;
			audio.Play();
			ammo--;
			fireCooldown = 30;
		}

		public override void secondaryFire(GameObject owner, Vector3 from, Vector3 direction, string target) {
			if (!held) {
				return;
			}
			audio.clip = throwProjectile;
			audio.Play();
			const float speed = 0.8f;
			projectile.speed = direction.normalized * speed;
			projectile.setTarget(target);
			projectile.owner = owner;
			GameObject spawnedEnemy = Instantiate(projectile.gameObject);
			spawnedEnemy.transform.position = from;
			ammo = 6;
			held = false;
			renderer.enabled = held;
		}

		public override SpriteRenderer getRenderer() {
			return renderer;
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.CompareTag("Player")) {
				StageController.getInstance().getPlayer().weapon = this;
			}
		}
	}
}