using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

namespace Manual
{
    public class RuleBook1 : RuleBook
    {
        Dictionary<string, MovieCotroller> movieControllerDictionary = new Dictionary<string, MovieCotroller>();
        private readonly string slideObjectName = "SlideMovie";

        // Start is called before the first frame update
        void Start()
        {
            var slideMovieObject = transform.Find(slideObjectName).gameObject;
            var slideMovieController = slideMovieObject.GetComponent<MovieCotroller>();
            movieControllerDictionary.Add(slideObjectName, slideMovieController);

        }

        public override void Entry()
        {

        }

        public override void Do()
        {
            if(GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
            {
                if(movieControllerDictionary[slideObjectName].IsMoviePlay == false)
                {
                    movieControllerDictionary[slideObjectName].MoviePlay();
                }
                else
                {
                    movieControllerDictionary[slideObjectName].MovieStop();
                }
            }
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
