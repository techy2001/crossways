using UnityEngine;

public class Damageable : MonoBehaviour {
	public float health = 1;
		
	public virtual bool damage(int amount) {
		health -= amount;
		if (health <= 0) {
			onDeath();
		}
		return true;
	}

	protected virtual void onDeath() {
		Destroy(gameObject);
	}
}