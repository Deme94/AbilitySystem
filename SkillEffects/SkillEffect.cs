using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Efecto de una habilidad
public abstract class SkillEffect : ScriptableObject{

    public bool applyToTarget;      // Si es True, se aplica sobre el target, si es False, sobre el player
    protected GameObject applyTo;   // Objeto al que se aplicara el efecto, target o player (es una variable temporal de ayuda)

    // Metodo abstracto que aplica el efecto
    public abstract void Apply(GameObject launcher, GameObject target);
}
