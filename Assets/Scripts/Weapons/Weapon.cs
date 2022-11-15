using UnityEngine;

namespace Weapons {
	public abstract class Weapon : MonoBehaviour {
		public abstract void tick();
		public abstract void primaryFire(GameObject owner, Vector3 from, Vector3 direction, string target);
		public abstract void secondaryFire(GameObject owner, Vector3 from, Vector3 direction, string target);
		public abstract SpriteRenderer getRenderer();
	}
}