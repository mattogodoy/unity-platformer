using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public void Quit(){
		Debug.Log ("APPLICATION QUIT");
		Application.Quit ();
	}

	public void Retry(){
		// Restart game
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
