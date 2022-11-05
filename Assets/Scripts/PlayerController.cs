using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;
	private Animator animator;
	private static readonly int IsMoving = Animator.StringToHash("isMoving");
	private const float BaseSpeed = 12f;

	public int health = 6;
	public int invulnTicks;

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}
    
	private void Start() {
        
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
	}

	private float getSpeed() {
		return BaseSpeed;
	}

	public bool damage(int amount) {
		if (invulnTicks > 0) {
			return false;
		}
		health -= amount;
		StageController.getInstance().cameraAmp = 8;
		if (health <= 0) {
			onDeath();
			return true;
		}
		invulnTicks = 20 * amount;
		return true;
	}

	private void onDeath() {
		StageController.getInstance().playerDead();
		Destroy(gameObject);
	}
}