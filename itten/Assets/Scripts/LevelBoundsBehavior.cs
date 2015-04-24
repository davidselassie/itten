using UnityEngine;
using System.Collections;

public class LevelBoundsBehavior : AbstractLevelBehavior {
	public override void Action () {
		FlowLogic.RestartLevel ();
	}
}
