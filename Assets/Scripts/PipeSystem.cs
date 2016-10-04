using UnityEngine;
using System.Collections;

public class PipeSystem : MonoBehaviour {

	private Player player;

	public Pipe pipePrefab;

	public int pipeCount;

	private int pipeCounter=0;

	private int sectionCounter=1;

	private Pipe[] pipes;

	public int emptyPipeCount;

	bool isEmpty;

	private void Awake () {
		pipes = new Pipe[pipeCount];
		for (int i = 0; i < pipes.Length; i++) {
			Pipe pipe = pipes [i] = Instantiate<Pipe> (pipePrefab);
			pipe.transform.SetParent (transform, false);
			pipe.Generate (0, i > emptyPipeCount);
			if (i > 0) {
				pipe.AlignWith (pipes [i - 1]);
			}

		}
		AlignNextPipeWithOrigin ();
	}

	public Pipe SetupFirstPipe(Player player){
		this.player = player;
		pipeCounter = 1;
		transform.localPosition = new Vector3 (0f, -pipes [1].CurveRadius);
		player.SetNoScore (true);
		return pipes [1];
	}

	public Pipe SetupNextPipe(){
		if (pipeCounter>1)
			player.SetNoScore (false);
		ShiftPipes ();
		AlignNextPipeWithOrigin ();
		if (pipeCounter % 8 == 0) {
			isEmpty = true;
			sectionCounter = (pipeCounter / 8)+1;
		} else {
			isEmpty = false;

		}
		if ((pipeCounter -2) % 8 == 0){
			player.SetSection(sectionCounter);
			if (pipeCounter>7)
				player.SetNoScore (true);
			if (sectionCounter== 7){
				player.SetMaxVel2 ();
			}
			if (sectionCounter == 10){
				player.SetMaxVel3();
			}
		}
		
		pipes [pipes.Length - 1].Generate (sectionCounter, !isEmpty);
		pipes [pipes.Length - 1].AlignWith (pipes [pipes.Length - 2]);
		transform.localPosition = new Vector3 (0f, -pipes [1].CurveRadius);
		pipeCounter++;
		return pipes [1];
	}

	private void ShiftPipes(){
		Pipe temp = pipes [0];
		for (int i = 1; i < pipes.Length; i++) {
			pipes [i - 1] = pipes [i];

		}
		pipes [pipes.Length - 1] = temp;
	}

	private void AlignNextPipeWithOrigin(){
		Transform transformToAlign = pipes [1].transform;
		for (int i = 0; i < pipes.Length; i++) {
			if (i != 1){
				pipes [i].transform.SetParent (transformToAlign);
			}
		}

		transformToAlign.localPosition = Vector3.zero;
		transformToAlign.localRotation = Quaternion.identity;

		for (int i = 0; i < pipes.Length; i++) {
			if (i != 1){
				pipes [i].transform.SetParent (transform);
			}
		}
	}
}
