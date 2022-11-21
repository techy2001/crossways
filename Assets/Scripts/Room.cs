using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Room : MonoBehaviour {
	[SerializeField] private List<EnemyWave> spawnWaves = new List<EnemyWave>();
	[SerializeField] private List<Door> doors = new List<Door>();
	private int lastWave = -1;
	private int livingEnemies;
	private bool started;

	private void OnTriggerEnter2D(Collider2D col) {
		if (this.started || !col.CompareTag("Player")) {
			return;
		}

		this.started = true;
		if (this.spawnWaves.Count < 1) {
			return;
		}

		this.startWaves();
	}

	private void startWaves() {
		foreach (Door door in this.doors) {
			door.lockDoor();
		}

		StageController.getInstance().roomActive = true;
		this.StartCoroutine(this.spawnEnemies(0));
	}

	private IEnumerator spawnEnemies(int wave) {
		yield return new WaitForSeconds(0.1f);
		foreach (EnemySpawner spawnPoint in this.spawnWaves[wave].spawnWave) {
			spawnPoint.setRoom(this);
			spawnPoint.spawnEnemy();
			this.livingEnemies++;
			yield return new WaitForSeconds(this.spawnWaves[wave].getDelay());
		}

		this.lastWave = wave;
	}

	protected internal void enemyKilled() {
		StageController.getInstance().enemyKilled();
		this.livingEnemies--;
		if (this.livingEnemies > 0) {
			return;
		}
		if (this.spawnWaves[this.lastWave]) Destroy(this.spawnWaves[this.lastWave].gameObject);
		if (this.lastWave >= this.spawnWaves.Count - 1) {
			this.StartCoroutine(this.endRoom());
		} else {
			this.StartCoroutine(this.spawnEnemies(this.lastWave + 1));
		}
	}

	private IEnumerator endRoom() {
		yield return new WaitForSeconds(0.2f);
		foreach (Door door in this.doors) {
			door.unlockDoor();
		}
		StageController.getInstance().roomActive = false;
		Destroy(this.gameObject);
	}
}