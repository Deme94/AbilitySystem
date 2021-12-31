using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Efecto de correr (automaticamente) y golpear al enemigo, pudiendo arrollarlo o chocar contra el
[CreateAssetMenu(menuName = "SkillEffect/RunAndHit")]
public class RunAndHit : SkillEffect
{

    public bool rollUp;         // True si queremos arrollar sin parar, false si queremos chocar y parar
    public float distance;      // Distancia a recorrer
    public float speed;         // Velocidad de correr

    public Push push;           // Efecto de empuje tras colisionar

    public override void Apply(GameObject launcher, GameObject target)
    {
        // Aplicable a: Player
        launcher.GetComponent<State>().StartCoroutine(Run(launcher));
    }

    // Corutina del efecto RunAndHit
    private IEnumerator Run(GameObject launcher)
    {
        Vector3 initialPosition = launcher.transform.position;                              // Posicion inicial antes de empezar
        PlayerController playerController = launcher.GetComponent<PlayerController>();      // Script del controller del player

        // El bucle sigue mientras no haya recorrido la distancia que alcanza el efecto
        while (Vector3.Distance(initialPosition, launcher.transform.position) < this.distance)
        {
            playerController.move = false;                                                                  // Anula el control de movimiento del player
            playerController.controller.Move(launcher.transform.forward * this.speed * Time.deltaTime);     // Mueve al player

            // Si choca con algo en el recorrido
            if ((playerController.controller.collisionFlags & CollisionFlags.CollidedSides) != 0)
            {
                // Si no es enemigo salimos del bucle, fin del efecto
                if (playerController.hit.tag != "Enemy") break; 
                // Si es enemigo...
                else
                {
                    launcher.GetComponent<State>().ActivateTarget(playerController.hit);
                    // Si el efecto es de arrollar
                    if (this.rollUp)
                    {
                        playerController.hit.GetComponent<State>().StartCoroutine(        
                            this.push.PushTarget(launcher, playerController.hit, this.distance -
                                Vector3.Distance(initialPosition, launcher.transform.position) + 0.5f));  // Le aplicamos el efecto empuje con la distancia restante y seguimos iterando, seguimos el efecto Run
                    }
                    // Si el efecto es de chocar
                    else
                    {
                        this.push.Apply(launcher, playerController.hit);  // Le aplicamos el efecto empuje (el cual tendra una distancia determinada) y detenemos el bucle, nos paramos
                        break;
                    }
                }
            }

            yield return null;      // El bucle itera en cada frame
        }
        playerController.move = true;       // Devolvemos el control de movimiento al player
    }
}
