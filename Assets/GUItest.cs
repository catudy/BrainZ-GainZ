using UnityEngine;
using System.Collections;

public class GUItest : MonoBehaviour {

	private Vector2 scrollPosition;
	public string testString = "Hey listen. Hey listen! Hey listen. Hey listen! Hey listen. Hey listen! Hey listen. Hey listen! Hey listen. Hey listen!";
	public bool triggerScroller = false;
	public bool triggerGroup = false;
	public bool triggerWindow = false;
	public bool triggerTool = false;
	public int a,b,c,d,w,x,y,z;
	private Rect window1 = new Rect(20,20,120,50);
	private Rect window2 = new Rect(80,20,120,50);
	private int toolbarint = 0;
	private string[] toolbar = new string[] {"Toolbar1","Toolbar2","Toolbar3"};

	public GUISkin myskin;

	// Use this for initialization
	void Start () {
		//scrollPosition = new Vector2 (Screen.width/2,Screen.height/2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		GUI.skin = myskin;
		//Changes tint of all gui elements, content color
		//GUI.backgroundColor = Color.green;
		//GUI.contentColor = Color.red;
		if(triggerScroller)
		{
			//a,b,c,d moves the scroller window and dimmensions
			//w,x,y,z moves the scroller window and dimmensions
			scrollPosition = GUI.BeginScrollView(new Rect(a,b,c,d),scrollPosition, new Rect(w,x,y,z));
			GUI.Button (new Rect (0,0,100,20), "Top-left");
			GUI.Button (new Rect (120,0,100,20), "Top-right");
			GUI.Button (new Rect (0,180,100,20), "Bottom-left");
			GUI.Button (new Rect (120,180,100,20), "Bottom-right");
			//GUI.Label(new Rect(w,x,y,z),testString);
			GUI.EndScrollView();
		}

		if(triggerGroup)
		{
		// Constrain all drawing to be within a 800x600 pixel area centered on the screen.
		GUI.BeginGroup (new Rect (Screen.width / 2 - 400, Screen.height / 2 - 300, 800, 600));
		
		// Draw a box in the new coordinate space defined by the BeginGroup.
		// Notice how (0,0) has now been moved on-screen
		GUI.Button (new Rect (0,0,800,600),
		         "This box is now centered! - here you would put your main menu");
		
		// We need to match all BeginGroup calls with an EndGroup
		GUI.EndGroup ();
		}

		if(triggerWindow)
		{
			window1 = GUI.Window (0, window1, fnctW1, "First");
			window2 = GUI.Window (1, window2, fnctW2, "Second");
		}

		//Use repeat button for handling input as it is active while the user holds it down
		//Textfield and password field for account creation

		if(triggerTool)
		{
			toolbarint = GUI.Toolbar (new Rect(25,25,250,30), toolbarint, toolbar);
		}
	}

	void fnctW1(int windowID)
	{
		if (GUI.Button (new Rect (10,20,100,20), "Put Back"))
			GUI.BringWindowToBack(0);
		GUI.DragWindow(new Rect (0,0, 10000, 20));
	}

	void fnctW2(int windowID)
	{
		if (GUI.Button (new Rect (10,20,100,20), "Put Back"))
			GUI.BringWindowToBack(1);
		GUI.DragWindow(new Rect (0,0, 10000, 20));
	}


}
