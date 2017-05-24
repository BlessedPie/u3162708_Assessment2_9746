using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {

	public void Quit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // If this is running in the unity editor, clicking the quit button will work
#else    
    Application.Quit(); 
#endif // Quit the application
    }
}
