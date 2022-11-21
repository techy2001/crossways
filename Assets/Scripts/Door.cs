using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour {
	private new SpriteRenderer renderer;
	public Sprite openSprite;
	public Sprite closedSprite;
	public Sprite lockedSprite;
	private BoxCollider2D boxCollider;
	public bool opened;
	private bool locked;
	private bool lockedVisual;
	private bool shouldBeOpen;

	private void Awake() {
		this.boxCollider = this.GetComponents<BoxCollider2D>()[1];
		this.renderer = this.GetComponent<SpriteRenderer>();
		this.lockedVisual = this.locked;
		this.updateSprite();
	}
	
	private void OnTriggerEnter2D(Collider2D col) {
		if (!col.gameObject.CompareTag("Player")) {
			return;
		}

		this.shouldBeOpen = true;
		if (!this.opened && !this.locked) {
			this.setOpened(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		this.shouldBeOpen = false;
		this.setOpened(false);
	}

	public void lockDoor() {
		this.StartCoroutine(this.shutLockDoor());
	}

	private IEnumerator shutLockDoor() {
		this.setOpened(false);
		this.locked = true;
		yield return new WaitForSeconds(0.2f);
		this.lockedVisual = this.locked;
		this.updateSprite();
	}

	public void unlockDoor() {
		this.locked = false;
		this.lockedVisual = this.locked;
		this.updateSprite();
		if (this.shouldBeOpen) {
			this.setOpened(true);
		}
	}

	private void setOpened(bool open) {
		this.opened = open;
		this.boxCollider.enabled = !open;
		this.updateSprite();
	}

	private void updateSprite() {
		this.renderer.sprite = this.opened ? this.openSprite : this.lockedVisual ? this.lockedSprite : this.closedSprite;
	}
}