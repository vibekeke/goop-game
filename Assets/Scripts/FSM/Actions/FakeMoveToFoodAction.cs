using GoopGame.Engine;
using UnityEngine;

namespace GoopGame.FSM
{
    [CreateAssetMenu(fileName = "MoveToFoodAction", menuName = "GoopGame/FSM/Actions/Create new MoveToFoodAction")]
    public class FakeMoveToFoodAction : StateAction
    {
        public string Message;
        public float moveSpeed = 5f;

        public override void Execute(Goop goop)
        {
            //Debug.Log($"Executing TestAction: {Message}");
            
            goop.FindNearestFood();
            if (goop.CurrentFoodTarget == null)
            {
                Debug.Log($"{goop.name} tried to move, but has no food target.");
                return;
            }

            Vector2 dir = (goop.CurrentFoodTarget.position - goop.transform.position).normalized;
            goop.transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
            Debug.Log($"{goop.name} is moving toward {goop.CurrentFoodTarget.name}");
        }
    }
}
