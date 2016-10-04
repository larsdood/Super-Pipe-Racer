using UnityEngine;
using System.Collections;

public class RandomPlacer : PipeItemGenerator {

	// Use this for initialization
	public PipeItem[] itemPrefabs;

	public override void GenerateItems (Pipe pipe) {
		float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
		for (int i = 1; i < pipe.CurveSegmentCount-1; i++) {
			if (i%6!=0){
				PipeItem item = Instantiate<PipeItem> (itemPrefabs [Random.Range (0, itemPrefabs.Length)]);
				float pipeRotation = (Random.Range (0, pipe.pipeSegmentCount) + 0.5f) * 360f / pipe.pipeSegmentCount;
				item.Position (pipe, i * angleStep, pipeRotation);
			}

		}
	}
}
