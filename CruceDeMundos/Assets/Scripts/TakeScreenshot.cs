using UnityEngine;
using System.Collections;

public class TakeScreenshot : MonoBehaviour {

    public Camera cameraToScreen;
    private bool takeShot = false;
	string name;

    public int resWidth = 1600;
    public int resHeight = 1200;

    private string path;

    void Start()
    {        
		resWidth = Screen.width;
		resHeight = Screen.height;
		name = GetComponent<MazeGenerator>().jsonName;
    }
    void OnDestroy()
    {
    }
    
	public void TakeShot()
    {
        takeShot = true;
    }
    void LateUpdate()
    {
        if (takeShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            cameraToScreen.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

            cameraToScreen.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);


            Texture2D image = new Texture2D(resWidth, resHeight);
            image.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            image.Apply();
            RenderTexture.active = rt;
            


            cameraToScreen.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
           
			byte[] bytes = screenShot.EncodeToPNG();


			string filename = Application.dataPath + "/Resources/Maze/screenshots/" + name + ".png";

          //  string filename = ScreenShotName();
           // string filename = path;
            System.IO.File.WriteAllBytes(filename, bytes);
            //Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeShot = false;

			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh ();
			#endif
        }
    }

}
