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
		boxCollider = GetComponents<BoxCollider2D>()[1];
		renderer = GetComponent<SpriteRenderer>();
		lockedVisual = locked;
		updateSprite();
	}
	
	private void OnTriggerEnter2D(Collider2D col) {
		if (!col.gameObject.CompareTag("Player")) {
			return;
		}
		shouldBeOpen = true;
		if (!opened && !locked) {
			setOpened(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		shouldBeOpen = false;
		setOpened(false);
	}

	public void lockDoor() {
		StartCoroutine(shutLockDoor());
	}

	private IEnumerator shutLockDoor() {
		setOpened(false);
		locked = true;
		yield return new WaitForSeconds(0.2f);
		lockedVisual = locked;
		updateSprite();
	}

	public void unlockDoor() {
		locked = false;
		lockedVisual = locked;
		updateSprite();
		if (shouldBeOpen) {
			setOpened(true);
		}
	}

	private void setOpened(bool open) {
		opened = open;
		boxCollider.enabled = !open;
		updateSprite();
	}

	private void updateSprite() {
		renderer.sprite = opened ? openSprite : lockedVisual ? lockedSprite : closedSprite;
	}
}