using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace Manual
{
    public class RuleBook2 : RuleBook
    {
        Dictionary<string, MovieCotroller> movieControllerDictionary = new Dictionary<string, MovieCotroller>();

        // Start is called before the first frame update
        void Start()
        {
        }

        public override void Entry()
        {

        }

        public override void Do()
        {
        }

        public override void Exit()
        {
            foreach(var movieController in movieControllerDictionary.Values)
            {
                movieController.MovieStop();
            }
        }
    }
}
