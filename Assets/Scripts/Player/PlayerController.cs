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
		private Camera mainCamera;

		private void Awake() {
			this.mainCamera = Camera.main;
			this.rigidbody = this.GetComponent<Rigidbody2D>();
			this.renderer = this.GetComponent<SpriteRenderer>();
			this.animator = this.GetComponent<Animator>();
		}

		private void FixedUpdate() {
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 velocity = input.normalized * this.getSpeed();
			this.renderer.flipX = velocity.x switch {
				< 0 => true,
				> 0 => false,
				_ => this.renderer.flipX
			};
			this.animator.SetBool(IsMoving, velocity != Vector2.zero);
			this.rigidbody.velocity = velocity;
			if (this.invulnTicks > 0) this.invulnTicks--;
			if (this.weapon) {
				this.weapon.tick();
				if (this.mainCamera) {
					Vector3 mousePoint = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);
					Vector3 difference = mousePoint - this.transform.position;
					difference = new Vector3(difference.x, difference.y, 0).normalized;
					bool mouseLeft = difference.x < 0;
					this.weapon.transform.position = this.transform.position + new Vector3(mouseLeft ? -0.4f : 0.4f, 0, 0.1f);
					if (Input.GetButton("Fire1")) {
						this.weapon.primaryFire(this.gameObject, this.transform.position, difference * 0.5f, "Enemy");
					}
					if (Input.GetButton("Fire2")) {
						this.weapon.secondaryFire(this.gameObject, this.transform.position, difference * 0.8f, "Enemy");
					}
					float rotZ = Mathf.Atan2(difference.y, mouseLeft ? -difference.x : difference.x) * Mathf.Rad2Deg;
					this.weapon.transform.rotation = Quaternion.Euler(0f, 0f, mouseLeft ? -rotZ : rotZ);
					this.weapon.getRenderer().flipX = mouseLeft;
				}
			}
		}

		private float getSpeed() {
			return BaseSpeed;
		}

		public override bool damage(int amount) {
			if (this.invulnTicks > 0) {
				return false;
			}

			base.damage(amount);
			StageController.getInstance().cameraAmp += 8;
			this.invulnTicks = 20 * amount;
			return true;
		}

		public override void setHealth(float health) {
			base.setHealth(health);
			StageController.getInstance().renderHearts(health, this.maxHealth);
		}

		protected override void onDeath() {
			StageController.getInstance().playerDead();
			StageController.getInstance().cameraAmp += 4;
			base.onDeath();
		}
	}
}