using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
	public class EnemyWave : MonoBehaviour {
		public List<EnemySpawner> spawnWave = new List<EnemySpawner>();
		[SerializeField] private float delay;

		public float getDelay() {
			return delay;
		}
	}
}