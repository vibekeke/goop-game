using UnityEngine;

namespace GoopGame.Engine       //In engine namespace since Goop must reference class
{
    public class BaseState : ScriptableObject
    {
        public virtual void EnterState(Goop goop)
        {
            // Default implementation does nothing
        }

        public virtual void ExitState(Goop goop)
        {
            // Default implementation does nothing
        }

        public virtual void Execute(Goop goop)
        {
            // Default implementation does nothing
        }
    }
}
