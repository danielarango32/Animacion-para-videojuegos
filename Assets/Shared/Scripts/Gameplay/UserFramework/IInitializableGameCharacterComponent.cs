using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IInitializableGameCharacterComponent
    {
        public void OnOwned();
    }
}
