using UnityEngine;
using System.Collections;

public class LevelExitBehavior : AbstractLevelBehavior {
	public override void Action () {
		FlowLogic.AdvanceLevel ();
	}
}
