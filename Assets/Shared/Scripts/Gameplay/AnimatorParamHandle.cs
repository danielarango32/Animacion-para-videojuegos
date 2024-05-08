using UnityEngine;

namespace Gameplay
{
    public struct AnimatorParamHandle
    {
        public string name;
        public int hash;

        public AnimatorParamHandle(string name)
        {
            this.name = name;
            hash = Animator.StringToHash(name);
        }
    }
}
