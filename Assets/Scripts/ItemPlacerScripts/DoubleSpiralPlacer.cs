using UnityEngine;
using System.Collections;

public class DoubleSpiralPlacer : PipeItemGenerator {

	// Use this for initialization
	public PipeItem[] itemPrefabs;

	public override void GenerateItems(Pipe pipe){
		float start = (Random.Range (0, pipe.pipeSegmentCount) + 0.5f);
		float direction = Random.value < 0.5f ? 1f : -1f;

		float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
		for (int i = 1; i < pipe.CurveSegmentCount-1; i++) {
			PipeItem item = Instantiate<PipeItem> (
				itemPrefabs [Random.Range (0, itemPrefabs.Length)]);
			float pipeRotation =
				(start + i * direction) * 360f / pipe.pipeSegmentCount;
			item.Position(pipe, i * angleStep, i%2==0? pipeRotation : pipeRotation+90); 
		}
	}
}
