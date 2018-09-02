using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BlackBack : MonoBehaviour {
	private bool displaying = false;
	private bool fromDiscard = false;
	private Collider col;
	private Material mat;
	public S_Discard discard;
	public float alpha = 0.5f;


	void Start() {
		col = GetComponent<Collider>();
		col.enabled = false;
		mat = GetComponent<Renderer>().material;
	}

	void Update() {
		float a = mat.color.a;
		if (displaying && a < alpha)
			mat.SetColor("_Color", new Color(0, 0, 0, (a + Time.deltaTime * 3) > alpha ? alpha : (a + Time.deltaTime * 3)));
		else if (!displaying && a > 0)
			mat.SetColor("_Color", new Color(0, 0, 0, (a - Time.deltaTime * 3 < 0f) ? 0 : a - (Time.deltaTime * 3)));
	}

	public void Enable(bool isFromDiscard = false) {
		fromDiscard = isFromDiscard;
		displaying = true;
		col.enabled = true;
	}

	public void Disable() {
		col.enabled = false;
		displaying = false;
	}

	public void OnMouseDown() {
		if (fromDiscard) {
			Disable();
			discard.QuitDisplayDiscardedCards();
		}
	}
}
