using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Stack : MonoBehaviour {

	public S_CardBase.Type type;
	private List<S_CardBase> cards = new List<S_CardBase>();
	public void AddCard(S_CardBase card) {
		cards.Add(card);
		Vector2 cardSize = card.GetSize();
		card.moveTo(new Vector3(transform.position.x, transform.position.y - (cardSize.y * 0.25f) * cards.Count, -0.025f * cards.Count), 20f);
	}
}
