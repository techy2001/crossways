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
			target = newTarget;
		}
		
		private void FixedUpdate() {
			calculateMovement();
			transform.Rotate(0, 0, 15);
			if (returnTimer > 0) {
				returnTimer--;
				if (returnTimer <= 0) {
					dealtDamage = true;
				}
			}
		}

		private void calculateMovement() {
			float distance = GameMath.distanceTo(transform.position, owner.transform.position);
			if (dealtDamage) {
				float power = speed.magnitude;
				Vector3 difference = owner.transform.position - transform.position;
				speed = difference.normalized * MathF.Min(distance, power);
			}
			transform.position += speed;
			if (dealtDamage && distance < 0.4f) {
				PlayerController player = StageController.getInstance().getPlayer();
				if (owner == player.gameObject && player.weapon is ShotgunWeapon shotgunWeapon) {
					shotgunWeapon.held = true;
					shotgunWeapon.getRenderer().enabled = true;
				}
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (dealtDamage) {
				return;
			}
			if (target != null && !col.gameObject.CompareTag(target)) {
				return;
			}
			Damageable damageable = col.gameObject.GetComponent<Damageable>();
			if (!damageable) {
				return;
			}
			damageable.damage(4);
			dealtDamage = true;
		}
	}
}