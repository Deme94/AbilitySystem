using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/SelfCastSkill")]
public class SelfCastSkill : Skill {

    public override void Fire(GameObject emitter)
    {
        ApplyEffects(emitter.transform.parent.gameObject, emitter.transform.parent.gameObject);
    }
}
