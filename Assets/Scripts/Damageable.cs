using UnityEngine;

public class Damageable : MonoBehaviour {
	public float maxHealth = 1;
	private float health = 1;
	
	public virtual bool damage(int amount) {
		this.health -= amount;
		if (this.health <= 0) {
			this.onDeath();
		}
		return true;
	}

	public float getHealth() {
		return this.health;
	}

	public virtual void setHealth(float health) {
		this.health = health;
	}
	
	protected virtual void onDeath() {
		Destroy(this.gameObject);
	}
}