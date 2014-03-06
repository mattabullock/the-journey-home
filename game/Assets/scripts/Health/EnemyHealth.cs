using UnityEngine;
using System.Collections;

public class EnemyHealth : HealthBase {

	[RPC]
	public override void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}
		
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			Die ();
		}
	}

}
