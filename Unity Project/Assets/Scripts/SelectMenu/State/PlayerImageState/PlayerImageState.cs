using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public abstract class PlayerImageState: MonoBehaviour
    {
        public abstract void Entry();

        public abstract void Do();
    }
}