using UnityEngine;
using System.Collections;

public class GameControlScript : MonoBehaviour {
	public Texture2D border;
	public Texture2D[] bars = new Texture2D[4];
	public Rect[] barPosition = new Rect[4];
	public Rect[] barCrop = new Rect[4];
	public int fadeStep = 0;
	public Color guiColor;

	public bool[] lockBar = new bool[2];
	//public float showingalpha;
	
	void OnGUI()
	{ 
		GUI.color = guiColor;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), border);
		//GUI.color = new Color(0,0,0,250);
		for (int i = 0; i < 4; i++)
		{
			GUI.color = Color.white;
			if (i == 1 || i == 3)
				GUI.BeginGroup(barCrop[i]);
			if (i == 1 && lockBar[0])
				GUI.color = Color.black;
			if (i == 3 && lockBar[1])
				GUI.color = Color.black;
			GUI.DrawTexture(barPosition[i], bars[i]);
			if (i == 1 || i == 3)
				GUI.EndGroup();
			//GUI.DrawTextureWithTexCoords(barPosition[i], bars[i],barCoords[i]);
		}
	}

	IEnumerator Fade (float startLevel, float endLevel, float duration) {
		float speed = 1.0f/duration;  
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime*speed) {
			guiColor.a = Mathf.Lerp(startLevel,endLevel,t);
			if (guiColor.a >= 1.0f)
			{
				fadeStep = 2;
				StopAllCoroutines();
			}
			yield return null;
		}
	}
	IEnumerator UnFade (float startLevel, float endLevel, float duration) {
		float speed = 1.0f/duration;  
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime*speed) {
			float a = Mathf.Lerp(startLevel,endLevel,t);
			guiColor.a = 1.0f - a;
			if (guiColor.a <= 0.0f)
			{
				fadeStep = 0;
				StopAllCoroutines();
			}
			yield return null;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (fadeStep == 0)
			{
				StartCoroutine(Fade(0,250,500.0f));
			}
			else if (fadeStep == 2)
			{
				StartCoroutine(UnFade(0,250,500.0f));
			}
		}
		//////////bar one/////////////////
		if (Input.GetKey(KeyCode.E) && !lockBar[0])
		{
			float cropH = Mathf.Lerp(0,150,Time.deltaTime*0.5f);
			barCrop[1].height -= cropH;
			if (barCrop[1].height <= 0)
			{	lockBar[0] = true; barCrop[1].height = 0;
			}
		}
		else if (barCrop[1].height < 150)
			barCrop[1].height += Mathf.Lerp(0,150,Time.deltaTime*0.1f);
		else if  (lockBar[0])
		{
			if (barCrop[1].height >= 125)
			{	
				lockBar[0] = false; 
				barCrop[1].height = 150;
			}
		}
		////////bar two///////////////
		 if (Input.GetKey(KeyCode.R) && !lockBar[1])
		{
			float cropH = Mathf.Lerp(0,122,Time.deltaTime*0.4f);
			barCrop[3].height -= cropH;
			if (barCrop[3].height <= 29)
			{	lockBar[1] = true; barCrop[3].height = 29;
			}
		}
		else if (barCrop[3].height < 122)
			barCrop[3].height += Mathf.Lerp(0,122,Time.deltaTime*0.08f);
		else if  (lockBar[1])
		{
			if (barCrop[3].height >= 100)
			{	
				lockBar[1] = false; 
				barCrop[3].height = 122;
			}
		}
	}
}
