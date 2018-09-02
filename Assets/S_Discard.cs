using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Discard : MonoBehaviour {
	public Texture Artwork;
	
	public S_BlackBack back;
	private List<S_CardBase> cards = new List<S_CardBase>();
	private bool displaying;


	void Start () {
		Material tmp = new Material(transform.GetChild(2).GetComponent<Renderer>().material);
		tmp.SetTexture("_MainTex", Artwork);
		transform.GetChild(2).GetComponent<Renderer>().material = tmp;
	}

	void Update() {
		
	}

	public void OnMouseDown() {
		GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().Discard();
	}

	public void DisplayDiscardedCards() {
		if (cards.Count > 0) {
			int lineSize = 8;
			Vector2 size = cards[0].GetSize();
			float x;
			int lines = cards.Count / lineSize;
			if (cards.Count % lineSize != 0)
				lines++;
			int j = 0;
			float y = lines / 2 * (size.x * 1.1f);
			if (lines % 2 == 0)
				y -= (0.5f * (size.x * 1.1f));
			for (int line = 0; line < lines; line++) {
				int currentSize = (cards.Count >= lineSize * (line + 1)) ? lineSize : cards.Count % lineSize;
				x = currentSize / 2 * (size.y * -1.1f);
				if (currentSize % 2 == 0)
					x += (0.5f * (size.y * 1.1f));
				for (int col = 0; col < currentSize; col++) {
					S_CardBase card = cards[j];
					card.transform.eulerAngles = new Vector3(0, 0, 0);
					Vector3 pos = new Vector3(x, y, -2);
					card.moveTo(pos, 20);
					j++;
					x += (size.y * 1.1f);
				}
				y -= (size.x * 1.1f);
			}
		}
		back.Enable(true);
	}

	public void QuitDisplayDiscardedCards() {
		foreach (S_CardBase card in cards) {
			moveToDiscard(card);
		}
	}

	private void moveToDiscard(S_CardBase card) {
		card.transform.eulerAngles = new Vector3(0, 0, 90);
		card.moveTo(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f), 20);
	}

	public void AddCard(S_CardBase card) {
		cards.Add(card);
		moveToDiscard(card);
		card.onDiscard = true;
		card.OnCardPlayed();
	}

}
