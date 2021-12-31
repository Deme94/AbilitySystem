using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/ProjectileSkill")]
public class ProjectileSkill : Skill
{
    public Rigidbody projectile;    // Proyectil Rigidbody a lanzar
    public float speed;             // Velocidad de movimiento del proyectil
    public float range;             // Alcance del proyectil

    // Llama a la funcion de lanzamiento de proyectil del emisor
    public override void Fire(GameObject emitter)
    {
        emitter.GetComponent<ActionEmitter>().Launch(this.projectile, this.speed, this.range);
    }

}
