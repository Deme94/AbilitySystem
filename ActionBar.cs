using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {

    [HideInInspector] public Skill[] slots;
    public IEnumerator castingSkill;
    [HideInInspector] public Dictionary<int, int[]> activeSkills;

    public Skill skillPrueba;
    public GameObject player;
    public GameObject actionEmitter;

	// Use this for initialization
	void Start () {
        slots = new Skill[5];
        activeSkills = new Dictionary<int, int[]>();    // KEY = skillHashCode, Value[0] = index, Value[1] = CD 
        castingSkill = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("1"))
        {
            if ((slots[0] != null) && (castingSkill == null) && (!activeSkills.ContainsKey(slots[0].GetHashCode())))
            {
                castingSkill = slots[0].Initiate(this.actionEmitter, this.gameObject);
                if(castingSkill != null)
                    StartCoroutine(castingSkill);
            }
            else
            {
                Debug.Log("COOLDOWN ESPERAAAA");
            }
        }
        else if (Input.GetButtonDown("2"))
        {
            if ((slots[1] != null) && (castingSkill == null) && (!activeSkills.ContainsKey(slots[1].GetHashCode())))
            {
                castingSkill = slots[1].Initiate(this.actionEmitter, this.gameObject);
                StartCoroutine(castingSkill);
            }
        }
        else if (Input.GetButtonDown("3"))
        {
            StartCoroutine(this.skillPrueba.Initiate(this.actionEmitter, this.gameObject));
        }
    }

    // Si la skill existe en la barra de accion, se borra
    public void RemoveRepeated(int skillHashCode, int ignoreIndex)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == ignoreIndex) continue;
            if(slots[i] != null && slots[i].GetHashCode() == skillHashCode)
            {
                Debug.Log("ignore index = " + ignoreIndex);
                Debug.Log("repeated = " + i);
                slots[i] = null;
                player.GetComponent<PlayerGuiManager>().RemoveSlot(i);
            }
        }
    }

    public int DecreaseCooldown(int skillHashCode)
    {
        int cd = --activeSkills[skillHashCode][1];
        if (activeSkills[skillHashCode][0] > -1)
        {
            player.GetComponent<PlayerGuiManager>().SetCooldown(activeSkills[skillHashCode][0], cd);
        }
        return cd;
    }

    public void AddSkill(Skill skill, int index)
    {
        slots[index] = skill;
        if (activeSkills.ContainsKey(skill.GetHashCode()))
        {
            activeSkills[skill.GetHashCode()][0] = index;
            player.GetComponent<PlayerGuiManager>().ActivateCooldown(index, activeSkills[skill.GetHashCode()][1]);
        }
    }

    public void RemoveSkill(int index)
    {
        Debug.Log("Borrando Skill");
        if (activeSkills.ContainsKey(slots[index].GetHashCode()))
        {
            activeSkills[slots[index].GetHashCode()][0] = -1;
            slots[index] = null;
            player.GetComponent<PlayerGuiManager>().DeactivateCooldown(index);
        }
        else
        {
            slots[index] = null;
        }
    }

    public void AddActiveSkill(int skillHashCode, int[] indexCD)
    {
        activeSkills.Add(skillHashCode, indexCD);
        player.GetComponent<PlayerGuiManager>().ActivateCooldown(indexCD[0], indexCD[1]);
    }

    public void RemoveActiveSkill(int skillHashCode)
    {
        if (activeSkills[skillHashCode][0] > -1)
        {
            player.GetComponent<PlayerGuiManager>().DeactivateCooldown(activeSkills[skillHashCode][0]);
        }
        activeSkills.Remove(skillHashCode);
    }

    public int GetIndex(Skill skill)
    {
        int index = -1;
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == skill)
                index = i;
        }
        return index;
    }
}
