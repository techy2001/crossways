using UnityEngine;
using Weapons;

namespace Player {
	public class PlayerController : Damageable {
		private new Rigidbody2D rigidbody;
		private new SpriteRenderer renderer;
		private Animator animator;
		private static readonly int IsMoving = Animator.StringToHash("isMoving");
		private const float BaseSpeed = 12f;
		public int invulnTicks;
		public Weapon weapon;
	
		private void Awake() {
			rigidbody = GetComponent<Rigidbody2D>();
			renderer = GetComponent<SpriteRenderer>();
			animator = GetComponent<Animator>();
		}

		private void FixedUpdate() {
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 velocity = input.normalized * getSpeed();
			renderer.flipX = velocity.x switch {
				< 0 => true,
				> 0 => false,
				_ => renderer.flipX
			};
			animator.SetBool(IsMoving, velocity != Vector2.zero);
			rigidbody.velocity = velocity;
			if (invulnTicks > 0) invulnTicks--;
			if (weapon) {
				weapon.tick();
				Camera mainCamera = Camera.main;
				if (mainCamera) {
					Vector3 mousePoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
					Vector3 difference = mousePoint - transform.position;
					difference = new Vector3(difference.x, difference.y, 0).normalized;
					bool mouseLeft = difference.x < 0;
					weapon.transform.position = transform.position + new Vector3(mouseLeft ? -0.4f : 0.4f, 0, 0);
					if (Input.GetButton("Fire1")) {
						weapon.primaryFire(gameObject, transform.position, difference * 0.5f, "Enemy");
					}
					if (Input.GetButton("Fire2")) {
						weapon.secondaryFire(gameObject, transform.position, difference * 0.8f, "Enemy");
					}
					float rotZ = Mathf.Atan2(difference.y, mouseLeft ? -difference.x : difference.x) * Mathf.Rad2Deg;
					weapon.transform.rotation = Quaternion.Euler(0f, 0f, mouseLeft ? -rotZ : rotZ);
					weapon.getRenderer().flipX = mouseLeft;
				}
			}
		}

		private float getSpeed() {
			return BaseSpeed;
		}

		public override bool damage(int amount) {
			if (invulnTicks > 0) {
				return false;
			}
			health -= amount;
			StageController.getInstance().cameraAmp = 8;
			if (health <= 0) {
				onDeath();
				StageController.getInstance().cameraAmp = 4;
				return true;
			}
			invulnTicks = 20 * amount;
			return true;
		}

		protected override void onDeath() {
			StageController.getInstance().playerDead();
			base.onDeath();
		}
	}
}