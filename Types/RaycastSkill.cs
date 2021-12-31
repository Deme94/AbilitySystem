using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/RaycastSkill")]
public class RaycastSkill : Skill {

    public float range;     // Longitud del rayo, alcance de la habilidad
    public bool requiresDetectionToCast;

    public override void Fire(GameObject emitter)
    {
        emitter.GetComponent<ActionEmitter>().Cast(new Ray(), this, true);
    }

    public override IEnumerator Initiate(GameObject emitter, GameObject actionBar)
    {
        if (!requiresDetectionToCast)
        {
            yield return base.Initiate(emitter, actionBar);
        }
        else
        {
            GameObject target = null;
            this.actionBar = actionBar;
            target = DetectTarget(emitter);
            Debug.Log("Requiere deteccion");
            if(target != null)
            {
                yield return Cast(emitter);
                ApplyEffects(emitter.transform.parent.gameObject, target);
                yield return ExitCast(emitter);
                yield return Cooldown();
            } else
            {
                actionBar.GetComponent<ActionBar>().castingSkill = null;
                Debug.Log("Target no encontrado");
            }
        }
    }

    private GameObject DetectTarget(GameObject emitter)
    {
        return emitter.GetComponent<ActionEmitter>().Cast(new Ray(), this, false);
    }
}
