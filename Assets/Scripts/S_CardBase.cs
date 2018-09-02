using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public static class R {
    public static readonly int GOLD = 0;
    public static readonly int WOOD = 1;
    public static readonly int STONE = 2;
    public static readonly int BRICK = 3;
    public static readonly int GLASS = 4;
    public static readonly int PAPYRUS = 5;
}
public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}
public class S_CardBase : MonoBehaviour {
	public enum Type {
		Green, Yellow, Red, Blue, Brown, Grey
	};
	public Type type;
	public Texture Artwork;
	public S_CardBase[] hide = new S_CardBase[2];
	public S_CardBase[] hiddenBy = new S_CardBase[2];

	public bool onBoard = true;
	public bool onDiscard = false;

    [NamedArrayAttribute (new string[] {"Godl", "Wood", "Stone", "Brick", "Glass", "Papyrus"})]
    public int[] cost = {0, 0, 0, 0, 0, 0};
	private bool visible = true;
	private float speedMove = 0f;
	private Vector3 destMove;
	private Outline outline;

	protected void Start () {
		outline = gameObject.transform.GetChild(2).gameObject.AddComponent<Outline>();
		Deselect();
		Material tmp = new Material(transform.GetChild(2).GetComponent<Renderer>().material);
		tmp.SetTexture("_MainTex", Artwork);
		transform.GetChild(2).GetComponent<Renderer>().material = tmp;
	}
	
	protected void Update () {
		if (transform.position != destMove) {
			float step = speedMove * Time.deltaTime;
        	transform.position = Vector3.MoveTowards(transform.position, destMove, step);
		}
	}

	public void OnCardSelected() {
		GameObject.FindGameObjectWithTag("Table").GetComponent<S_GameManager>().SelectCard(this);
	}

	public void OnCardPlayed() {
		onBoard = false;
		if (hide[0])
			hide[0].Reveal();
		if (hide[1])
			hide[1].Reveal();
	}

	public void Select() {
		outline.enabled = true;
		GetComponent<Animation>().enabled = false;
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.05f);
	}

	public void Deselect() {
		outline.enabled = false;
		GetComponent<Animation>().enabled = true;
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.05f);
	}

	public void AddHideCard(S_CardBase toAdd) {
		if (!hide[0])
			hide[0] = toAdd;
		else
			hide[1] = toAdd;
		if (toAdd) {
			toAdd.AddHiddenByCard(GetComponent<S_CardBase>());
		}
	}

	public void AddHiddenByCard(S_CardBase toAdd) {
		if (!hiddenBy[0])
			hiddenBy[0] = toAdd;
		else
			hiddenBy[1] = toAdd;
	}

	public void moveTo(Vector3 destination, float speed) {
		speedMove = speed;
		destMove = destination;
		transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
	}

	private void Reveal() {
		if (visible || IsCardHidden())
			return;
		visible = true;
		GetComponent<Animation>().Play("flip");
	}

	public void Hide() {
		if (!visible)
			return;
		visible = false;
		transform.Rotate(0, 180, 0);	
	}

	public Vector2 GetSize() {
		return new Vector2(transform.GetComponentInChildren<Renderer>().bounds.size.x, transform.GetComponentInChildren<Renderer>().bounds.size.y);
	}

	private bool IsCardHidden() {
		if (hiddenBy[0] && hiddenBy[0].onBoard)
			return true;
		if (hiddenBy[1] && hiddenBy[1].onBoard)
			return true;
		return false;
	}

	public void OnMouseEnter() {
		if (onBoard && visible)
			GetComponent<Animation>().Play("Grow");		
	}

	public void OnMouseExit() {
		if (onBoard && visible)
			GetComponent<Animation>().Play("shrink");
	}

	public void OnMouseDown() {
		if (IsCardHidden())
			return;		
		if (!onBoard)
			return;
		if (onDiscard)
			return;
		OnCardSelected();
	}

}
