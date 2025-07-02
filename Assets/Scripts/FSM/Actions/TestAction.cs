using GoopGame.Engine;
using UnityEngine;

namespace GoopGame.FSM
{
    [CreateAssetMenu(fileName = "TestAction", menuName = "GoopGame/FSM/Actions/Create new TestAction")]
    public class TestAction : StateAction
    {
        public string Message;
        public override void Execute(Goop goop)
        {
            Debug.Log($"Executing TestAction: {Message}");
        }
    }
}
