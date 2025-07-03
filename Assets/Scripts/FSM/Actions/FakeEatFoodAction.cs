using GoopGame.Engine;
using UnityEngine;

namespace GoopGame.FSM
{
    [CreateAssetMenu(fileName = "EatFoodAction", menuName = "GoopGame/FSM/Actions/Create new EatFoodAction")]
    public class FakeEatAction : StateAction
    {
        public string Message;

        public override void Execute(Goop goop)
        {
            Debug.Log($"Executing TestAction: {Message}");
            if (goop.TouchedFood != null)
            {
                Destroy(goop.TouchedFood.gameObject); //Food should delete self, no?
                goop.ClearTouchedFood();
                goop.ClearFoodTarget();
                // …play eat animation, then transition back to Idle…
            }
        }
    }
}
