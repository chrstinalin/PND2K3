using UnityEngine;

/*
 * Default bullet class; shoots straight forward.
 */
public class DefaultMechBullet : MechBullet
{
    protected override void HandleMovement()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
