using UnityEngine;
using System.Collections;
using System;

public static class ReplayHandler {
	public static bool BeginRecording(){
		if (/*Everyplay.ReadyForRecording() &&*/ Everyplay.IsSupported()){
			if ((RecordReplaySettingType)PlayerPrefs.GetInt(GloVar.RecordReplayChoicePrefName,0)==RecordReplaySettingType.Enabled){
				Everyplay.SetLowMemoryDevice (true);
				Everyplay.StartRecording ();
				return true;
			}
			try{
				
			}
			catch(Exception e){
				return false;
			}
		}
		else{
			Debug.Log("No recording on this device");
			return false;
		}
		return false;
	}

	public static void StopRecording(){
		if (Everyplay.IsRecording ()){
			Everyplay.StopRecording ();
		}
		else{
			Debug.Log ("Unable to stop: Not recording");
		}
	}
	public static void WatchReplay(){
		Debug.Log ("Replay clicked!");
		try {
			Everyplay.PlayLastRecording ();
		}
		catch(Exception e){
			Debug.Log ("No last file to be played, error: " + e);
		}
	}
	public static void PauseRecording(){
		if (Everyplay.IsRecording()){
			Everyplay.PauseRecording ();
		}
	}
	public static void ResumeRecording(){
		if (Everyplay.IsPaused ()){
			Everyplay.ResumeRecording ();
		}
	}
	public static void SetMetaData(GameMode gameMode, int score, int section){
		Everyplay.SetMetadata ("Game Mode", gameMode);
		Everyplay.SetMetadata ("Score", score);
		Everyplay.SetMetadata ("Section", section); 
	}
}
