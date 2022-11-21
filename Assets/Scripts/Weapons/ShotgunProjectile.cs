using System;
using Player;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons {
	public class ShotgunProjectile : MonoBehaviour {
		public GameObject owner;
		public Vector3 speed;
		public string target;
		public bool dealtDamage;
		private int returnTimer = 40;

		public void setTarget(string newTarget) {
			this.target = newTarget;
		}
		
		private void FixedUpdate() {
			this.calculateMovement();
			this.transform.Rotate(0, 0, 15);
			if (this.returnTimer > 0) {
				this.returnTimer--;
				if (this.returnTimer <= 0) {
					this.dealtDamage = true;
				}
			}
		}

		private void calculateMovement() {
			float distance = GameMath.distanceTo(this.transform.position, this.owner.transform.position);
			if (this.dealtDamage) {
				float power = this.speed.magnitude;
				Vector3 difference = this.owner.transform.position - this.transform.position;
				this.speed = difference.normalized * MathF.Min(distance, power);
			}

			this.transform.position += this.speed;
			if (this.dealtDamage && distance < 0.4f) {
				PlayerController player = StageController.getInstance().getPlayer();
				if (this.owner == player.gameObject && player.weapon is ShotgunWeapon shotgunWeapon) {
					shotgunWeapon.held = true;
					shotgunWeapon.getRenderer().enabled = true;
				}
				Destroy(this.gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (this.dealtDamage) {
				return;
			}
			if (this.target != null && !col.gameObject.CompareTag(this.target)) {
				return;
			}
			Damageable damageable = col.gameObject.GetComponent<Damageable>();
			if (!damageable) {
				return;
			}
			damageable.damage(4);
			this.dealtDamage = true;
		}
	}
}