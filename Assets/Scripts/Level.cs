using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ColorMixer {

    [CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
    public class Level : GameScene
    {
        //Settings specific to level only
        [Header("Level specific")]
        private Transform[] foodsLevel;

        public string[] nameFoodsLevel;
    }
}
