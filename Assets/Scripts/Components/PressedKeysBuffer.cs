﻿using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    class PressedKeysBuffer
    {
        public List<KeyCode> pressedKeys;

        public PressedKeysBuffer()
        {
            pressedKeys = new List<KeyCode>();
        }
    }
}