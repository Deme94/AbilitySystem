using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillEffect/RawDamage")]
public class RawDamage : SkillEffect {

    public int damagePoints;            // Daño que aplicara el efecto

    public override void Apply(GameObject launcher, GameObject target)
    {
        // Aplicable a: Enemigo, Player
        if (this.applyToTarget)
        {
            this.applyTo = target;
        }
        else
            this.applyTo = launcher;

        this.applyTo.GetComponent<Stats>().DecreaseHP(damagePoints);      // Le hacemos daño
    }
}
