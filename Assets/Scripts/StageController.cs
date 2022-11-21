using System;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour {
	private static StageController controller;
	[SerializeField] private GameObject playerSpawn;
	[SerializeField] private PlayerController player;
	[SerializeField] private CinemachineVirtualCamera cameraController;
	[SerializeField] private GameObject failureScreen;
	[SerializeField] private GameObject heartPosition;
	private readonly List<Heart> hearts = new List<Heart>();
	private bool stageComplete = false;
	private int time = -1;
	private int kills;
	public float cameraAmp;
	
	private int livingEnemies;
	public bool noEnemies = true;
	public bool roomActive = false;
	
	private bool dead;
	private int deadTime;

	[SerializeField] private GameObject heartContainer;
	[SerializeField] private Heart heartPrefab;
	[SerializeField] private Sprite fullHeart;
	[SerializeField] private Sprite halfHeart;
	[SerializeField] private Sprite noHeart;

	private void Awake() {
		controller = this;
		GameObject playerObject = Instantiate(this.player.gameObject);
		playerObject.transform.position = this.playerSpawn.transform.position;
		this.cameraController.Follow = playerObject.transform;
		this.player = playerObject.GetComponent<PlayerController>();
	}

	private void FixedUpdate() {
		if (!this.dead) {
			this.cameraAmp /= 1.1f;
			if (this.cameraAmp < 0.01) this.cameraAmp = 0;
			if (this.noEnemies && !this.roomActive) {
				if (this.player.getHealth() < this.player.maxHealth) {
					this.player.setHealth(this.player.maxHealth);
				}
			}
		} else {
			this.deadTime++;
			Time.timeScale = Mathf.Lerp(1, 0.25f, this.deadTime / 200f);
			if (Input.GetButton("Restart")) {
				restart();
				return;
			}
		}

		this.cameraController.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = this.cameraAmp;
		if (this.time > -1 && !this.stageComplete) {
			this.time++;
		}
	}

	public void renderHearts(float hp, float maxHp) {
		int count = (int) Mathf.Min(hp, maxHp);
		int heartIndex = 0;
		foreach (Heart heart in this.hearts) {
			heart.gameObject.SetActive(false);
		}
		while (this.hearts.Count <= maxHp / 2) {
			GameObject newHeart = Instantiate(this.heartPrefab.gameObject, this.heartContainer.transform);
			Heart heart = newHeart.GetComponent<Heart>();
			heart.rectTransform.SetPositionAndRotation(heart.rectTransform.position + new Vector3(65 * this.hearts.Count, 0, 0), heart.rectTransform.rotation);
			this.hearts.Add(heart);
		}
		while (count >= 2) {
			this.hearts[heartIndex].gameObject.SetActive(true);
			this.hearts[heartIndex].renderer.sprite = this.fullHeart;
			heartIndex++;
			count -= 2;
		}
		while (count >= 1) {
			this.hearts[heartIndex].gameObject.SetActive(true);
			this.hearts[heartIndex].renderer.sprite = this.halfHeart;
			heartIndex++;
			count -= 1;
		}
		while (maxHp > (heartIndex) * 2) {
			this.hearts[heartIndex].gameObject.SetActive(true);
			this.hearts[heartIndex].renderer.sprite = this.noHeart;
			heartIndex++;
		}
	}

	public static StageController getInstance() {
		return controller;
	}

	public void startTimer() {
		this.time = 0;
	}

	public void enemyKilled() {
		this.kills++;
		this.setLivingEnemies(this.livingEnemies - 1);
	}

	private void setLivingEnemies(int count) {
		this.livingEnemies = count;
		this.noEnemies = this.livingEnemies <= 0;
	}

	public PlayerController getPlayer() {
		return this.player;
	}

	public CinemachineVirtualCamera getCamera() {
		return this.cameraController;
	}

	public void playerDead() {
		this.dead = true;
		this.failureScreen.SetActive(true);
	}

	private static void restart() {
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void addLivingEnemy() {
		this.setLivingEnemies(this.livingEnemies + 1);
	}
}