using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Efecto de proteccion, cambia la vida del target por la del protector, absorbiendo asi su daño
[CreateAssetMenu(menuName = "SkillEffect/Buff/Protection")]
public class Protection : Buff
{

    public override IEnumerator Coroutine(GameObject launcher, GameObject target)
    {

        target.GetComponent<State>().SetProtector(launcher);            // Protege al target

        yield return base.Coroutine(launcher, target);

        // El buffo termina
        target.GetComponent<State>().SetProtector(target);              // El target queda desprotegido
        target.GetComponent<State>().RemoveBuff(this);                  // Borramos el buffo del target

    }
}
