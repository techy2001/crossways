using UnityEngine;

public class Damageable : MonoBehaviour {
	public float maxHealth = 1;
	private float health = 1;
	
	public virtual bool damage(int amount) {
		health -= amount;
		if (health <= 0) {
			onDeath();
		}
		return true;
	}

	protected float getHealth() {
		return health;
	}

	protected virtual void setHealth(float health) {
		this.health = health;
	}
	
	protected virtual void onDeath() {
		Destroy(gameObject);
	}
}