﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace TTT.Scripts.City
{

    public class LSystemGenerator : MonoBehaviour
    {
        
        public Rule[] rules;
        public string rootSentence;
        [Range(0,10)]
        public int iterationLimit = 1;
        private void Start(){
            Debug.Log(GenerateSententce());
        }
        public string GenerateSententce(string word = null)
        {
            if (word == null)
            {
                word = rootSentence;
            }
            return Recurse(word);
        }

        public string Recurse(string word, int iteration = 0)
        {
            if (iteration >= iterationLimit)
            {
                return word;
            }

            StringBuilder newWord = new StringBuilder();
            foreach (var c in word)
            {
                newWord.Append(c);
                foreach (var rule in rules)
                {
                    if (rule.letter == c.ToString())
                    {
                        newWord.Append(Recurse(rule.GetResult(), iteration + 1));
                    }
                }
            }

            return newWord.ToString();
        }
    }
}
