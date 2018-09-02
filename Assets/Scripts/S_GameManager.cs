using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GameManager : MonoBehaviour {
	public S_Player player1;
	public S_Player player2;
	public S_Discard discard;
	public Transform cardDispositionAnchor;
	public GameObject sampleCard;
	public S_BlackBack back;
	public S_PlayerTurn playerTurnCoin;
	public List<GameObject> wondersAvialable;

	private S_Player currentPlayer;
	private S_CardBase selectedCard;
	private List<List<GameObject>> cardsFromAge = new List<List<GameObject>>();

	//1=VISIBLE / -1=HIDE / 0=NOCARD
	private int[,,] ageDisposition = new int[3,7,6] {
		{
			{1,1,1,1,1,1},
			{0,-1,-1,-1,-1,-1},
			{0,1,1,1,1,0},
			{0,0,-1,-1,-1,0},
			{0,0,1,1,0,0},
			{0,0,0,0,0,0},
			{0,0,0,0,0,0},

		},{
			{0,0,1,1,0,0},
			{0,0,-1,-1,-1,0},
			{0,1,1,1,1,0},
			{0,-1,-1,-1,-1,-1},
			{1,1,1,1,1,1},
			{0,0,0,0,0,0},
			{0,0,0,0,0,0},

		},{
			{0,0,1,1,0,0},
			{0,0,-1,-1,-1,0},
			{0,1,1,1,1,0},
			{0,0,-1,0,-1,0},
			{0,1,1,1,1,0},
			{0,0,-1,-1,-1,0},
			{0,0,1,1,0,0},
		},
	};

	private S_CardBase[,] cards = new S_CardBase[7,6];

	void Start () {
		SelectWonderDebug();
		//DisplayWondersSelection();
		currentPlayer = player1;
		currentPlayer.YourTurn();
		player2.EndTurn();
		for (int i = 0; i < 3; i++)
			cardsFromAge.Add(new List<GameObject>());
		for (int age = 1; age < 4; age++) {
			Object[] loaded = Resources.LoadAll("Age" + age);
			foreach (GameObject card in loaded)
				cardsFromAge[age - 1].Add(card);
		}
		setCardForAge(1);
	}
	
	public void SelectWonderDebug() {
		for (int i = 0; i < 8; i++) {
			player1.AddWonder(Instantiate(wondersAvialable[i], Vector3.zero, Quaternion.identity));
		}
		for (int i = 4; i < 8; i++) {
			player2.AddWonder(Instantiate(wondersAvialable[i], Vector3.zero, Quaternion.identity));
		}
	}

	private void DisplayWondersSelection() {
		back.Enable();
		Vector2 size = Vector2.zero;
		for (int i = 0; i < 4; i++)
		{
			GameObject toPut = wondersAvialable[Random.Range(0, wondersAvialable.Count)];
			wondersAvialable.Remove(toPut);
			GameObject wonder = Instantiate(toPut, Vector3.zero, Quaternion.identity);
			Debug.Log(wonder.transform.localScale);
			wonder.transform.localScale = new Vector3(wonder.transform.localScale.x * 1.5f, wonder.transform.localScale.y * 1.5f ,wonder.transform.localScale.z);
			size = wonder.GetComponent<S_Wonder>().GetSize();
			wonder.transform.position = new Vector3((size.x * -1.65f) + (size.x * 1.1f * i), 0, -2);
		}
		playerTurnCoin.transform.position = new Vector3(0, playerTurnCoin.transform.position.y, -2);
	}

	public void SelectWonder(S_Wonder wonder) {
		currentPlayer.AddWonder(wonder.gameObject);
		if (currentPlayer == player1 && currentPlayer.wonders.Count == 2)
			NextTurn();
		if (currentPlayer == player2 && currentPlayer.wonders.Count == 4)
			NextTurn();
		if (currentPlayer == player2 && currentPlayer.wonders.Count == 2)
			DisplayWondersSelection();
		if (currentPlayer == player1 && currentPlayer.wonders.Count == 4)
			back.Disable();
	}

	private void NextTurn() {
		currentPlayer.EndTurn();
		currentPlayer = (currentPlayer == player1) ? player2 : player1;
		currentPlayer.YourTurn();
		playerTurnCoin.SetPlayerTurn(currentPlayer.index);
	}


	public void SelectCard(S_CardBase card) {
		if (selectedCard != card && selectedCard)
			selectedCard.Deselect();
		selectedCard = card;
		card.Select();
	}

	public void Discard() {
		if (selectedCard) {
			selectedCard.Deselect();
			discard.AddCard(selectedCard);
			NextTurn();
		} else {
			discard.DisplayDiscardedCards();
		}
		selectedCard = null;
	}

	public void Build() {
		if (!selectedCard)
			return;
		if (currentPlayer.BuildCard(selectedCard)) {
			selectedCard.Deselect();
			selectedCard.OnCardPlayed();
			NextTurn();
			selectedCard = null;
		}
	}

	public void EstimateBuildCost() {
		if (!selectedCard)
			return;
		currentPlayer.EstimateBuildCost(selectedCard);
	}

	public void EreaseEstimateBuildCost() {
		currentPlayer.EstimateBuildCost(null);
	}

	private GameObject getRandomCardFromAge(int age) {
		GameObject card = cardsFromAge[age][Random.Range(0, cardsFromAge.Count - 1)];
		//cardsFromAge[age].Remove(card);
		return card;
	}

	private void setCardForAge(int age) {
		age = age - 1;
		Vector2 cardSize = sampleCard.GetComponent<S_CardBase>().GetSize();
		cardSize.x = cardSize.x * 1.1f;
		cardSize.y = cardSize.y * 0.6f;
		Vector2Int pos = Vector2Int.zero;
		for (pos.y = 0; pos.y < 7; pos.y++) {
			for (pos.x = 0; pos.x < 6; pos.x++) {
				if (ageDisposition[age, pos.y, pos.x] != 0) {
					Vector3 toPutCoord = new Vector3(
						cardDispositionAnchor.position.x + pos.x * cardSize.x + ((pos.y % 2 == 0) ? cardSize.x / 2 : 0),
						cardDispositionAnchor.position.y + pos.y * cardSize.y, cardDispositionAnchor.position.z - 0.025f * (7 - pos.y)
					);
					GameObject card = Instantiate(getRandomCardFromAge(age), toPutCoord, Quaternion.identity);
					if (ageDisposition[age, pos.y, pos.x] == -1)
						card.GetComponent<S_CardBase>().Hide();
					cards[pos.y, pos.x] = card.GetComponent<S_CardBase>();
					if (pos.y >= 1) {
						if (cards[pos.y - 1, pos.x])
							cards[pos.y - 1, pos.x].AddHideCard(card.GetComponent<S_CardBase>());
						if (pos.y % 2 == 1) {
							if (pos.x >= 1 && cards[pos.y - 1, pos.x - 1])
								cards[pos.y - 1, pos.x - 1].AddHideCard(card.GetComponent<S_CardBase>());
						} else {
							if (pos.x <= 5 && cards[pos.y - 1, pos.x + 1])
								cards[pos.y - 1, pos.x + 1].AddHideCard(card.GetComponent<S_CardBase>());
						}
					}
				}
			}
		}
	}
}