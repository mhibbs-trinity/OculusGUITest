using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

	public GameObject goCanvas;
	public GameObject goTitle;
	public GameObject[,] goLights;

	public Color offColorNormal 	 = new Color (0.2f, 0.2f, 0.2f, 1f);
	public Color offColorHighlighted = new Color (0.25f, 0.25f, 0.25f, 1f);
	public Color offColorPressed 	 = new Color (0.1f, 0.1f, 0.1f, 1f);
	public Color offColorLabel 		 = new Color (0.6f, 0.6f, 0.6f, 1f);

	public Color onColorNormal 	 	= new Color (0.5f, 0.8f, 0.5f, 1f);
	public Color onColorHighlighted = new Color (0.6f, 0.9f, 0.6f, 1f);
	public Color onColorPressed 	= new Color (0.4f, 0.7f, 0.4f, 1f);
	public Color onColorLabel 		= new Color (0.1f, 0.1f, 0.1f, 1f);

	public Font titleFont;
	public Font buttonFont;

	GameObject CreateEmptyUICanvas(string canvName) {
		GameObject goCanv = new GameObject ();
		goCanv.name = "UITester";
		goCanv.AddComponent<Canvas> ();
		goCanv.AddComponent<CanvasScaler> ();
		goCanv.AddComponent<GraphicRaycaster> ();
		Canvas canv = goCanv.GetComponent<Canvas> ();
		canv.renderMode = RenderMode.ScreenSpaceOverlay;
		return goCanv;
	}

	GameObject CreateUIText(string goName, string textVal, GameObject goParent, Vector3 pos, Vector2 sz, 
	                        Font fnt, FontStyle style, Color col, TextAnchor align) {
		GameObject goText = new GameObject ();
		goText.name = goName;
		goText.transform.SetParent (goParent.transform);
		goText.AddComponent<Text> ();
		RectTransform rtText = goText.GetComponent<RectTransform> ();
		rtText.anchoredPosition3D = pos;
		rtText.sizeDelta = sz;
		Text txt = goText.GetComponent<Text> ();
		txt.text = textVal;
		txt.color = col;
		txt.font = fnt;
		txt.resizeTextForBestFit = true;
		txt.fontStyle = style;
		txt.alignment = align;
		return goText;
	}

	GameObject CreateUIButton(string goName, string label, GameObject goParent, Sprite spr,
	                          Vector3 pos, Vector2 sz, Color labelCol) {
		GameObject goBtn = new GameObject ();
		goBtn.AddComponent<RectTransform> ();
		goBtn.AddComponent<Image> ();
		goBtn.AddComponent<Button> ();
		goBtn.name = goName;
		goBtn.transform.SetParent (goParent.transform);
		Image img = goBtn.GetComponent<Image> ();
		img.sprite = spr;
		img.type = Image.Type.Sliced;
		RectTransform rt = goBtn.GetComponent<RectTransform> ();
		rt.anchoredPosition3D = pos;
		rt.sizeDelta = sz;
		CreateUIText ("text_" + goName, label, goBtn, new Vector3 (), sz, buttonFont, 
		             FontStyle.Bold, labelCol, TextAnchor.MiddleCenter);
		return goBtn;
	}

	// Use this for initialization
	void Start () {
		titleFont = Resources.Load<Font> ("Fonts/The 2K12");
		buttonFont = Resources.Load<Font> ("Fonts/LiquidCrystal-Normal");

		//Create GUI Canvas 
		goCanvas = CreateEmptyUICanvas ("LightsOutGUI");

		//Create GUI Title GObject
		goTitle = CreateUIText ("Title", "Lights Out!", goCanvas, new Vector3 (0f, 180f, 0f), new Vector2 (400f, 75f),
		                       titleFont, FontStyle.BoldAndItalic, onColorNormal, TextAnchor.MiddleCenter);

		//Create Array of Buttons
		goLights = new GameObject[3,3];
		for (int r=0; r<3; r++) {
			for (int c=0; c<3; c++) {
				//goLights[r,c] = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/LightButton"));
				goLights[r,c] = CreateUIButton("Light_" + r.ToString() + "_" + c.ToString(), "OFF", goCanvas,
				                               Resources.Load<Sprite>("Images/Buttons/LightButton"),
				                               new Vector3(-105f+105f*c, 105f-105f*r, 0f),
				                               new Vector2(100f,100f), offColorLabel);
				AddToggleListener(goLights[r,c].GetComponent<Button>(), r, c);
				TurnLightOn(r,c);
			}
		}
	}

	public void TurnLightOn(int r, int c) {
		Button btn = goLights[r,c].GetComponent<Button>();
		ColorBlock cb = new ColorBlock ();
		cb.normalColor 		= onColorNormal;
		cb.highlightedColor = onColorHighlighted;
		cb.pressedColor 	= onColorPressed;
		cb.colorMultiplier  = 1f;
		cb.fadeDuration 	= 0.1f;
		Debug.Log (cb);
		btn.colors = cb;
		Text txt = goLights[r,c].GetComponentInChildren<Text>();
		txt.text = "ON";
		txt.color = onColorLabel;
	}

	public void TurnLightOff(int r, int c) {
		Button btn = goLights[r,c].GetComponent<Button>();
		ColorBlock cb = new ColorBlock ();
		cb.normalColor 		= offColorNormal;
		cb.highlightedColor = offColorHighlighted;
		cb.pressedColor 	= offColorPressed;
		cb.colorMultiplier  = 1f;
		cb.fadeDuration 	= 0.1f;
		Debug.Log (cb);
		btn.colors = cb;
		Text txt = goLights[r,c].GetComponentInChildren<Text>();
		txt.text = "OFF";
		txt.color = offColorLabel;
	}

	public void AddToggleListener(Button btn, int r, int c) {
		btn.onClick.AddListener (() => {Toggle (r, c);});
	}

	public void Toggle(int r, int c) {
		ToggleSingleLight (r, c);
		if(r<2) ToggleSingleLight(r+1,c);
		if(r>0) ToggleSingleLight(r-1,c);
		if(c<2) ToggleSingleLight(r,c+1);
		if(c>0) ToggleSingleLight(r,c-1);
	}

	public void ToggleSingleLight(int r, int c) {
		if(goLights[r,c].GetComponentInChildren<Text>().text == "OFF") {
			TurnLightOn(r,c);
		} else {
			TurnLightOff(r,c);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	public void Sample() {
		Debug.Log ("clicked");
	}
}
