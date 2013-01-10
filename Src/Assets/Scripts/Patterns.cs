using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using UnityEngine;


    class Patterns : MonoBehaviour
    {
        // Warning, the oder must the same in patterns.txt
        enum PatternType
        {
            None = -1,
            Straight = 0,
            Joint = 1
        }
       // private static string fileName = "patterns.txt";
        
        private System.Random  _random = new System.Random();
        public  List<List<string[]>> patterns = new List<List<string[]>>(10);
        public  Hashtable patternsTopTable    = new Hashtable();
        public  Hashtable patternsBottomTable = new Hashtable();
        public  int SlotNum = 0;

        private string[] SlotType = new string[100];

        public  string GetSlotRandom()
        {
            return SlotType[_random.Next(SlotNum)];
        }



        private void BuildPatternsTable()
        {
            for (ulong i = 0; i < 64; i++)
            {
                patternsTopTable.Add(Patterns.BitToSlot(i), new List<string[]>());
                patternsBottomTable.Add(Patterns.BitToSlot(i), new List<string[]>());
            }
        }

//         public static string[] GetPatternRandom()
//         {
//             
//             
//         }

        // get a pattern that with downSlot
        public string[] GetPatternRandomWithBottomSlot(string bottomSlot)
        {
            if (patternsBottomTable.ContainsKey(bottomSlot))
            {
                List<string[]> patternList = patternsBottomTable[bottomSlot] as List<string[]>;
                int length = patternList.Count;
                int randomPick = _random.Next(length);
                return patternList[randomPick];
            }
            else
            {
                return GetPatternRandom();
            }

        }

        public string[] GetPatternRandom()
        {
            int length = patterns[1].Count;
            int randomPick = _random.Next(length );
            return patterns[1][randomPick];
        }

        public string[] GetPatternRandomWithTopSlot(string topSlot)
        {
            List<string[]> patternList = patternsTopTable[topSlot] as List<string[]>;
            int length = patternList.Count;
            int randomPick = _random.Next(length);
            return patternList[randomPick];
        }


        private void IndexTopAndBottomTable()
        {
            // enumerate all the patters
            foreach (List<string[]> patternLists in patterns)
            {
                foreach (string[] aPattern in patternLists)
                {
                    List<string[]> bottomList = patternsBottomTable[aPattern[0]] as List<string[]>;
                    List<string[]> topList    = patternsTopTable[aPattern[9]] as List<string[]>;
                    bottomList.Add(aPattern);
                    topList.Add(aPattern);
                }
            }

        }

        private static string BitToSlot(UInt64 val)
        {
            char[] bitStr = new char[6];
            for (int i = 0; i < 6; i++)
            {
                UInt64 j = ((val << (i+58)) & 0x8000000000000000) >> 63;
                if (j == 0)
                {
                    bitStr[i] = 'O';
                }
                else
                {
                    bitStr[i] = '@';
                }
            }
            return new string(bitStr);
        }



        public  void ReadPatternsFile( string filename)
        {
            BuildPatternsTable();
            int         currentType = -1;
            int         patternNum  = -1;
            int         pieceNum    = 0;
            string[] filecontent = File.ReadAllLines(filename);
            foreach (string line in filecontent)
            {
                if (line.Trim() == "")
                {
                    continue;
                }
                // find the tag
                else if (line.Substring(0, 2) == "##")
                {
                    currentType++;
                    patternNum = -1;
                    patterns.Add(new List<string[]>());
                    continue;
                }
                else if (line.Substring(0, 2) == "--") // find the start of pattern
                {
                    patternNum++;
                    pieceNum = 0;
                    patterns[currentType].Add(new string[10]);
                    continue;
                }
                else if (line.Substring(0, 2) == "**")
                {
                    SlotType[SlotNum++] = line.Substring(2, 6);
                }
                else
                {
                    //UnityEngine.Debug.Assert(CheckContent(line.Trim()));
                    patterns[currentType][patternNum][pieceNum] = line.Trim();
                    pieceNum++;
                }
            }
            IndexTopAndBottomTable();
        }

        private bool CheckContent( string line)
        {
            if (line.Length != 6)
                return false;
            foreach (char c in line)
            {
                if (c != 'O' && c != '@')
                    return false;
            }
            return true;
        }

        private string GetBottonSlot(ref string[] pattern)
        {
            return pattern[0];    
        }

        private string GetTopSlot(ref string[] pattern)
        {
            return pattern[9];
        }
    }

