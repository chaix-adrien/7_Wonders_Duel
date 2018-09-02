using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class S_Wonder : MonoBehaviour {
	public Texture Artwork;
	private bool forSelection = true;
	private Vector3 defaultScale;
	private Outline outline;

	void Awake() {
		defaultScale = transform.localScale;
	}

	void Start () {
		outline = gameObject.transform.GetChild(2).gameObject.AddComponent<Outline>();
		outline.enabled = false;
		Material tmp = new Material(transform.GetChild(2).GetComponent<Renderer>().material);
		tmp.SetTexture("_MainTex", Artwork);
		transform.GetChild(2).GetComponent<Renderer>().material = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResetScale() {
		Debug.Log(defaultScale);
		transform.localScale = defaultScale;
	}

	public void OnMouseOver() {
		if (forSelection) {
			outline.enabled = true;
		}
	}

	public void OnMouseExit() {
		if (forSelection) {
			outline.enabled = false;
		}
	}

	public void OnMouseDown() {
		if (forSelection) {
			forSelection = false;
			GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().SelectWonder(this);
			outline.enabled = false;
		} else {

		}
	}

	public Vector2 GetSize() {
		return new Vector2(transform.GetComponentInChildren<Renderer>().bounds.size.x, transform.GetComponentInChildren<Renderer>().bounds.size.y);
	}
}
