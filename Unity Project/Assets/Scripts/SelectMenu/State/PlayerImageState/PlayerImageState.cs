using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectMenu
{
    public interface PlayerImageState
    {
        void Entry(PlayerImage playerImage);

        void Do(PlayerImage playerImage);
    }
}