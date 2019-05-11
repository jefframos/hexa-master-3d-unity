using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonLoader : Singleton<JsonLoader>
{
    public string LoadFromStreaming(string path)
    {
        string dataAsJson;
        
        string filePath = Path.Combine(Application.streamingAssetsPath, path);
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
#if UNITY_EDITOR
        filePath = Path.Combine(Application.streamingAssetsPath, path);

#elif UNITY_IOS
        string filePath = Path.Combine (Application.dataPath + "/Raw", path);
 
#elif UNITY_ANDROID
        filePath = "jar:file://" + Application.dataPath + "!/assets/" + path;
        filePath = Path.Combine (Application.streamingAssetsPath + "/", path);
#endif
        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(filePath);
#if UNITY_EDITOR || UNITY_IOS
            dataAsJson = File.ReadAllText(filePath);

#elif UNITY_ANDROID
            WWW reader = new WWW (filePath);
                    while (!reader.isDone) {
                    }
                    dataAsJson = reader.text;
              //          if (filePath.Contains ("://") || filePath.Contains (":///")) {
		            //	//debugText.text += System.Environment.NewLine + filePath;
		            //	Debug.Log ("UNITY:" + System.Environment.NewLine + filePath);
		            //	UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get (filePath);
		            //	//yield return www.Send ();
		            //	dataAsJson = www.downloadHandler.text;
		            //} else {
		            //	dataAsJson = File.ReadAllText (filePath);
		            //}
#endif
            return dataAsJson;
        }

        return null;
    }
}
