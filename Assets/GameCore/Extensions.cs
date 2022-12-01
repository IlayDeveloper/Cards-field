using DG.Tweening;
using DG.Tweening.Core;

namespace GameCore
{
    public static class Extensions
    {
        public static async System.Threading.Tasks.Task AsyncWaitForCompletion(this Tween t)
        {
            if (!t.active)
            {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            while (t.active && !t.IsComplete()) await System.Threading.Tasks.Task.Yield();
        }
    }
}