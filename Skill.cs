using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

    public string skillName;

    public bool moveWhileCasting;
    public float moveSpeedPenalty;
    public bool rotateWhileCasting;

    public float castTime;
    public float exitTime;
    public bool fireBeforeCast;

    public int cooldown;

    public string target;       // Target de la skill

    protected GameObject actionBar;

    public SkillEffect[] effects;

    public AudioClip shootSound;

    // Dispara la skill
    public abstract void Fire(GameObject emitter);

    public virtual IEnumerator Initiate(GameObject emitter, GameObject actionBar)
    {
        this.actionBar = actionBar;

        if (fireBeforeCast){
            Fire(emitter);
            yield return Cast(emitter);
            yield return ExitCast(emitter);
            yield return Cooldown();
        } else
        {
            yield return Cast(emitter);
            Fire(emitter);
            yield return ExitCast(emitter);
            yield return Cooldown();
        }
    }

    protected virtual IEnumerator Cast(GameObject emitter)
    {
        if (!this.moveWhileCasting)
        {
            emitter.GetComponentInParent<PlayerController>().move = false;
        }
        else
        {
            emitter.GetComponentInParent<Stats>().DecreaseSpeedMultiplier(moveSpeedPenalty);
        }
        if (this.rotateWhileCasting)
        {
            emitter.GetComponentInParent<PlayerController>().rotate = true;
        }
        emitter.GetComponentInParent<Animator>().Play(this.skillName);
        yield return new WaitForSeconds(this.castTime);           // Tiempo de activacion
        emitter.GetComponentInParent<AudioSource>().PlayOneShot(shootSound);
    }

    protected virtual IEnumerator ExitCast(GameObject emitter)
    {
        yield return new WaitForSeconds(this.exitTime);

        if (!this.moveWhileCasting)
        {
            emitter.GetComponentInParent<PlayerController>().move = true;
        }
        else
        {
            emitter.GetComponentInParent<Stats>().IncreaseSpeedMultiplier(moveSpeedPenalty);
        }
        if (this.rotateWhileCasting)
        {
            emitter.GetComponentInParent<PlayerController>().rotate = false;
        }

        actionBar.GetComponent<ActionBar>().castingSkill = null;
    }

    protected virtual IEnumerator Cooldown()
    {
        int[] indexCD = new int[2];

        indexCD[0] = actionBar.GetComponent<ActionBar>().GetIndex(this);                          // Se añade a skills activas (en enfriamiento)
        indexCD[1] = this.cooldown + 1;
        actionBar.GetComponent<ActionBar>().AddActiveSkill(this.GetHashCode(), indexCD);

        while (actionBar.GetComponent<ActionBar>().DecreaseCooldown(this.GetHashCode()) > -1)                                            // Cooldown
        {
            yield return new WaitForSeconds(1);
        }
        actionBar.GetComponent<ActionBar>().RemoveActiveSkill(this.GetHashCode());
        Debug.Log("Lista de nuevo");
    }


    public void ApplyEffects(GameObject launcher, GameObject target) {

        if (target != launcher && launcher.tag == "Player")
        {
            launcher.GetComponent<State>().ActivateTarget(target);
        }

        if(this.target == "Enemy")
        {
            target.GetComponent<EnemyAI>().Fight(launcher.transform);
        }

        foreach ( SkillEffect effect in effects)
        {
            effect.Apply(launcher, target);
        }
    }
}
