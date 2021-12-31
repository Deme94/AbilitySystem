using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillEffect/Teleport")]
public class Teleport : SkillEffect {

    public override void Apply(GameObject launcher, GameObject target)
    {
        // Aplicable a: Player (por ahora)
        Transform playerLookAt = launcher.GetComponent<PlayerController>().lookAt;
        launcher.transform.position = target.transform.position - target.transform.forward*2;
        playerLookAt.forward = target.transform.forward;
        playerLookAt.Rotate(20, -15, 0);
        launcher.GetComponent<PlayerController>().Rotate();
    }
}
