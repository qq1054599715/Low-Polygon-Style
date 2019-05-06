using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesImageProcess : MonoBehaviour
{
	public Texture2D srcTexture;

	public float progress = 1;

	public PossionStyle PossionStyle;
	
	public Vector2 minCellSize = new Vector2(10,10);
	public Vector2 maxCellSize = new Vector2(100,100);
	
	// Use this for initialization
	void Start ()
	{
		PossionStyle = new PossionStyle();
	}
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			Texture2D dstTexture = PossionStyle.makePossionStyle(srcTexture,minCellSize,maxCellSize,progress);
			gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(dstTexture,new Rect(0,0,dstTexture.width,dstTexture.height),new Vector2(0.5f,0.5f));
			progress -= 0.05f;
			if (progress < 0)
			{
				progress = 0;
			}
		}
	}
}
