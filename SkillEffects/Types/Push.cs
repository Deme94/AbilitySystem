using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Efecto de empuje que mueve al target de forma automatica
[CreateAssetMenu(menuName = "SkillEffect/Push")]
public class Push : SkillEffect {

    public float distance;          // Distancia que recorre tras ser empujado
    public float speed;             // Velocidad de empuje

    public override void Apply(GameObject launcher, GameObject target)
    {
        // Aplicable a: Enemy
        target.GetComponent<State>().StartCoroutine(PushTarget(launcher, target, this.distance));
    }

    // Corrutina del efecto Push
    public IEnumerator PushTarget(GameObject launcher, GameObject target, float distance)
    {
        Debug.Log("hit");

        Vector3 initialPosition = target.transform.position;                                // Posicion inicial antes de iterar
        CharacterController enemyController = target.GetComponent<CharacterController>();   // Componente CharacterController del target
        target.layer = 10;                                                                  // Cambiamos su layer a IgnoreCharacter para no colisionar con personajes durante el efecto

        // El bucle sigue mientras no haya recorrido la distancia que alcanza el efecto
        while (Vector3.Distance(initialPosition, target.transform.position) < distance)
        {
            //enemyController.StopMoving();                                                 // Anula el control de movimiento del target
            enemyController.Move(launcher.transform.forward * this.speed * Time.deltaTime);   // Mueve al target

            // Si choca con algo en el recorrido
            if ((enemyController.collisionFlags & CollisionFlags.CollidedSides) != 0)
            {
                yield return new WaitForSeconds(1f);        // Espera 1 segundo (para evitar ser empujado otra vez) y sale del bucle
                break;
            }

            yield return null;              // El bucle itera en cada frame
        }
        // playerController.move = true;
        target.layer = 8;                   // Su layer vuelve a ser el de antes (ya puede colisionar con personajes)
    }
}
