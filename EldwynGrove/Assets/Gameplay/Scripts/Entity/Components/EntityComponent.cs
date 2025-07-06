using UnityEngine;

namespace EldwynGrove
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class EntityComponent : MonoBehaviour
    {
        protected GameObject Owner { get; private set; }
        protected Transform Transform { get; private set; }
        protected Animator Animator { get; private set; }
        protected Rigidbody2D Rigidbody2D { get; private set; }
        protected BoxCollider2D BoxCollider { get; private set; }

        /*----------------------------------------------------------------
        | --- Awake: Called when the script instance is being loaded --- |
        ----------------------------------------------------------------*/
        protected virtual void Awake()
        {
            Owner = gameObject;
            Transform = transform;
            Animator = GetComponent<Animator>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();

            Utilities.CheckForNull(Animator, $"{gameObject.name}: Animator");
            Utilities.CheckForNull(Rigidbody2D, $"{gameObject.name}: Rigidbody2D");
            Utilities.CheckForNull(BoxCollider, $"{gameObject.name}: BoxCollider2D");
        }
    }
}