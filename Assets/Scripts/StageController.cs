using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour {
	private static StageController controller;
	[SerializeField] private GameObject playerSpawn;
	[SerializeField] private PlayerController player;
	[SerializeField] private CinemachineVirtualCamera cameraController;
	[SerializeField] private GameObject failureScreen;
	private bool stageComplete = false;
	private int time = -1;
	private int kills;
	public float cameraAmp;
	public int livingEnemies;
	
	private bool dead;
	private int deadTime;

	private void Awake() {
		controller = this;
		GameObject playerObject = Instantiate(player.gameObject);
		playerObject.transform.position = playerSpawn.transform.position;
		cameraController.Follow = playerObject.transform;
		player = playerObject.GetComponent<PlayerController>();
	}

	private void FixedUpdate() {
		if (!dead) {
			cameraAmp /= 1.1f;
			if (cameraAmp < 0.01) cameraAmp = 0;
			if (livingEnemies == 0) {
				player.health = 6;
			}
		} else {
			deadTime++;
			Time.timeScale = Mathf.Lerp(1, 0.25f, deadTime / 200f);
			if (Input.GetButton("Restart")) {
				restart();
				return;
			}
		}
		cameraController.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cameraAmp;
		if (time > -1 && !stageComplete) {
			time++;
		}
	}

	public static StageController getInstance() {
		return controller;
	}

	public void startTimer() {
		time = 0;
	}

	public void enemyKilled() {
		kills++;
		livingEnemies--;
	}

	public PlayerController getPlayer() {
		return player;
	}

	public CinemachineVirtualCamera getCamera() {
		return cameraController;
	}

	public void playerDead() {
		dead = true;
		failureScreen.SetActive(true);
	}

	private static void restart() {
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}