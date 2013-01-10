using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    
    class Road:MonoBehaviour
    {
        private string[] _road;

        private int[] _levelLength =                {10,30,40,50,100   };
        private string[] _patternFileName =         {"p1.txt","p2.txt","p3.txt","p4.txt","p5.txt"  };
     
        public int  _roadLength = 0;
        private  int  _currentLength = 0;
        



        void Statr()
        {
            
        }


        public Road()
        {
            foreach(int i in _levelLength)
            {
                _roadLength += i * 10;
            }
            _road = new string[_roadLength];
        }

//         public  void CreateRoad(string patternFile)
//         {
//             _road = new string[100 * 10];
//             int length = 0;
//             Patterns pat = new Patterns();
//             pat.ReadPatternsFile(patternFile);
//             string bottomSlot = pat.GetSlotRandom();
//             for (int i = 0; i < 100; i++)
//             {
//                 string[] pattern = pat.GetPatternRandomWithBottomSlot(bottomSlot);
//                 for (int j = 0; j < 10; ++j)
//                 {
//                     _road[length++] = pattern[j];
//                 }
//                 bottomSlot = _road[length-1];
//             }
//         }

        public string[] CreateRoad()
        {
            string bottomSlot = "@@@@OO";
            int length = 0;
            for (int i = 0; i < _levelLength.Count(); ++i)
            {
                Patterns pat = new Patterns();
                pat.ReadPatternsFile(Application.dataPath + "/" + _patternFileName[i]);
                for (int j = 0; j < _levelLength[i]; j++)
                {
                    string[] pattern = pat.GetPatternRandomWithBottomSlot(bottomSlot);
                    for (int k = 0; k < 10; k++)
                    {
                        _road[length++] = pattern[k];
                    }
                    bottomSlot = _road[length - 1];

                }
            }
            return _road;
        }


     
    
}
