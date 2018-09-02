using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerTurn : MonoBehaviour {

	private Material mat;

	private int player;
	// Use this for initialization
	void Start () {
		SetPlayerTurn(1);
		mat = GetComponent<Renderer>().material;
	}
	
	public void SetPlayerTurn(int playerTurn) {
		player = playerTurn;
	}
	
	void Update () {
		if (player == 1 && mat.mainTextureOffset.x < 0.5f)
			mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + Time.deltaTime * 4, 0);
		else if (player == 2 && mat.mainTextureOffset.x > -0.5f)
			mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x - Time.deltaTime * 4, 0);
	}

		
	public void OnMouseOver() {
		GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().EstimateBuildCost();
	}

	public void OnMouseExit() {
		GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().EreaseEstimateBuildCost();
	}

	public void OnMouseDown() {
		GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().Build();
	}
}
