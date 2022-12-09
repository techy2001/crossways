using UnityEngine;

public class Damageable : MonoBehaviour {
	private AudioSource audioSource;
	public AudioClip hitSound;
	public float maxHealth = 1;
	private float health = 1;

	public virtual bool damage(int amount) {
		this.setHealth(this.getHealth() - amount);
		this.playHurtSound();
		if (this.getHealth() <= 0) {
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

	private void playHurtSound() {
		if (!this.audioSource) {
			this.audioSource = this.GetComponent<AudioSource>();
		}
		if (this.audioSource) {
			this.audioSource.Stop();
			this.audioSource.PlayOneShot(this.hitSound);
		}
	}
}