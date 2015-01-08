using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileController : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Color GetColor() {
		return transform.gameObject.GetComponent<Image>().color;
	}
	
	
	public bool Absorb(GameObject s) {
		Color c = s.GetComponent<TileController>().GetColor();
		if (GetColor() == AppColors.Blue) {
			Debug.Log ("wat");
			if (c == AppColors.Red) {
				Debug.Log ("woo");
				SetColor(AppColors.White);
				return true;
			}
		}
		if (GetColor() == AppColors.Red) {
			if (c == AppColors.Blue) {
				SetColor(AppColors.White);
				return true;
			}
		}
		return false;
	}
	
	public void SetColor(Color c) {
		transform.GetComponent<Image>().color = c;
	}
	
	void SetPosition(int i, int j) {
		RectTransform rt = (RectTransform)transform;
		rt.position = new Vector3(j*rt.sizeDelta.x+(rt.sizeDelta.x/2), i*rt.sizeDelta.y+(rt.sizeDelta.y/2));
	}

	
}
