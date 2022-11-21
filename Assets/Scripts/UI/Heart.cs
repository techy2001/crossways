using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class Heart : MonoBehaviour {
		public new Image renderer;
		public RectTransform rectTransform;

		public void Awake() {
			this.renderer = this.GetComponent<Image>();
			this.rectTransform = this.GetComponent<RectTransform>();
		}
	}
}