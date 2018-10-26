using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettlementPiece : MonoBehaviour {

	public int x;
	public int y;
	public Settlement settlement;
	public Color color;

	public void SetValues(int xPos, int yPos, Settlement s, Color c){
		x = xPos;
		y = yPos;
		settlement = s;
		color = c;
		GetComponent<Image>().color = c;
	}



}
