using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class S_Player : MonoBehaviour {
	public int index;
	public List<S_Stack> stacks;
	public GameObject wondersAnchor;
	public GameObject estimatedCost;
	public GameObject sampleWonder;
	public S_Player otherPlayer;

    [NamedArrayAttribute (new string[] {"Gold", "Wood", "Stone", "Brick", "Glass", "Papyrus"})]
    public int[] tradeRessources = {0, 0, 0, 0, 0, 0};
	
	private int[] ressources = {0, 0, 0, 0, 0, 0};
	public List<GameObject> wonders = new List<GameObject>();

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {
			ressources[i] = 0;
			tradeRessources[i] = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void YourTurn() {
	}

	public void EndTurn() {
		EstimateBuildCost(null);
	}

	private int GetRessourceCost(int ressource) {
		if (ressource == R.GOLD)
			return 1;
		int cost = 2;
		cost += otherPlayer.tradeRessources[ressource];
		return 2;
	}

	private int GetBuildCost(S_CardBase card) {
		int cost = 0;
		for (int i = 0; i < 5; i++) {
			int dif = card.cost[i] - ressources[i];
			cost += GetRessourceCost(i) * dif;	
		}
		return cost;
	}

	public int EstimateBuildCost(S_CardBase card) {
		if (card == null) {
			estimatedCost.GetComponentInChildren<TextMesh>().text = "";
			estimatedCost.SetActive(false);
		} else {
			int cost = GetBuildCost(card);
			if (cost != 0) {
				estimatedCost.SetActive(true);			
				estimatedCost.GetComponentInChildren<TextMesh>().text = "" + cost;
			}
		}
		return 10;
	}

	public bool BuildCard(S_CardBase card) {
		for (int i = 0; i < 5; i++) {
			if (ressources[i] < card.cost[i])
				return false;
		}
		foreach (S_Stack stack in stacks)
		{
			if (stack.type == card.type) {
				stack.AddCard(card);
			}
		}
		return true;
	}

	public void AddWonder(GameObject wonder) {
		wonder.GetComponent<S_Wonder>().ResetScale();
		wonder.GetComponent<S_Wonder>().forSelection = false;
		wonder.transform.localEulerAngles = new Vector3(0, 0, 90);
		Vector2 size = wonder.GetComponent<S_Wonder>().GetSize();
		Vector3 pos = new Vector3();
		pos.x = wondersAnchor.transform.position.x + ((wonders.Count < 2) ? size.x * 1.05f : 0);
		pos.y = wondersAnchor.transform.position.y + ((wonders.Count % 2 == 0) ? size.y* 1.05f : 0);
		wonder.transform.position = pos;
		wonder.transform.parent = gameObject.transform;
		wonders.Add(wonder);
	}
}
