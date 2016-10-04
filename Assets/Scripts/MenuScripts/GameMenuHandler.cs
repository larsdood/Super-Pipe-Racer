using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class GameMenuHandler : MonoBehaviour {
	public void RestartClick(){
		SceneManager.LoadScene ("MainGame");
	}
	public void MainMenuClick(){
		SceneManager.LoadScene("MainMenu");
	}
	public void WatchReplayClick(){
		ReplayHandler.WatchReplay ();
	}
	public void LeaderboardClick(){
		
	}
}
