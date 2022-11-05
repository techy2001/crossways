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
		if (started || !col.CompareTag("Player")) {
			return;
		}
		started = true;
		if (spawnWaves.Count < 1) {
			return;
		}
		startWaves();
	}

	private void startWaves() {
		foreach (Door door in doors) {
			door.lockDoor();
		}
		StartCoroutine(spawnEnemies(0));
	}

	private IEnumerator spawnEnemies(int wave) {
		yield return new WaitForSeconds(0.1f);
		foreach (EnemySpawner spawnPoint in spawnWaves[wave].spawnWave) {
			spawnPoint.setRoom(this);
			spawnPoint.spawnEnemy();
			livingEnemies++;
			yield return new WaitForSeconds(spawnWaves[wave].getDelay());
		}
		lastWave = wave;
	}

	protected internal void enemyKilled() {
		StageController.getInstance().enemyKilled();
		livingEnemies--;
		if (livingEnemies > 0) {
			return;
		}
		Destroy(spawnWaves[lastWave].gameObject);
		if (lastWave >= spawnWaves.Count - 1) {
			StartCoroutine(endRoom());
		} else {
			StartCoroutine(spawnEnemies(lastWave + 1));
		}
	}

	private IEnumerator endRoom() {
		yield return new WaitForSeconds(0.2f);
		foreach (Door door in doors) {
			door.unlockDoor();
		}
		Destroy(gameObject);
	}
}