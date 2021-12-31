using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Efecto Buffo que tiene una duracion en el target (veneno, sangrado, aumento de fuerza, velocidad...)
public abstract class Buff : SkillEffect {

    public Sprite buffIcon;                         // Icono del buffo 
    public string buffName;                         // Nombre del buffo
    public string buffDescription;                  // Descripcion del buffo                  

    public int buffDuration;                        // Duracion del buffo
    public bool isDebuff;                           // Si es True, es un Debuff, si es False, es un Buff   

    public override void Apply(GameObject launcher, GameObject target)
    {
        if (this.applyToTarget)
        {
            this.applyTo = target;
        } else
        {
            this.applyTo = launcher;
        }
            target.GetComponent<State>().AddBuff(this, launcher);     // Le añadimos el buffo al aliado
    }

    // Corutina que se aplica durante el tiempo de vida del buffo
    public virtual IEnumerator Coroutine(GameObject launcher, GameObject target)
    {
        int timeToLive = this.buffDuration;                     // El tiempo de vida es la duracion del buffo

        // Mientras el buffo tenga tiempo de vida y el target siga teniendo el buffo
        while (timeToLive > 0)
        {
            target.GetComponent<State>().SetTTL(this, timeToLive);

            yield return new WaitForSeconds(1f);        // Itera cada segundo
            --timeToLive;                               // El tiempo de vida disminuye en 1
        }
    }
}
