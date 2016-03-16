using System;
using System.Xml.Linq;
using System.Collections.Generic;
using SudokuLib;
using System.IO;

namespace SudokuGameEngineLib
{
    /// <summary>
    /// Class for saving and loading games.
    /// </summary>
    internal class GameFileManager
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameFileManager() { }


        /// <summary>
        /// Read game data from xml-file.
        /// </summary>
        /// <param name="fileName">Xml file path.</param>
        /// <returns>GameData read from xml-file.</returns>
        /// <remarks>Returns an invalid GameData if reading fails or data is invalid.</remarks>
        public GameData ReadGame(string fileName)
        {
            try
            {
                string content = File.ReadAllText(fileName);
                XElement root = XElement.Parse(content);
                return ParseGameData(root);
            }
            catch (Exception)
            {
                return new GameData();
            }
        }


        /// <summary>
        /// Write game data to a xml-file.
        /// </summary>
        /// <param name="fileName">Target file path.</param>
        /// <param name="data">GameData to be saved.</param>
        /// <returns>True, if game data is valid and was successfully saved.</returns>
        public bool WriteGame(string fileName, GameData data)
        {
            if (data.IsValid())
            {
                XElement root = CreateRootElement(data);
                root.Save(fileName);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Parse GameData object represented by the root element.
        /// </summary>
        /// <param name="root">SudokuGame root element.</param>
        /// <returns>GameData parsed from XElement.</returns>
        /// <remarks>Returns an invalid GameData if XElement contains invalid data.</remarks>
        private GameData ParseGameData(XElement root)
        {
            Dictionary<Coordinate, int> initVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> userVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, int> solverVals = new Dictionary<Coordinate, int>();
            Dictionary<Coordinate, List<int>> candidates = new Dictionary<Coordinate, List<int>>();
            SquareRegions layout = new SquareRegions();

            if (!root.Name.LocalName.Equals("SudokuGame"))
            {
                return new GameData();
            }

            IEnumerable<XElement> children = root.Elements();
            foreach (XElement e in children)
            {
                if (e.Name.LocalName.Equals("SudokuTable"))
                {
                    layout = SquareRegions.Parse(e.Attribute("Layout").Value);
                    IEnumerable<XElement> squares = e.Elements("Square");
                    foreach (XElement sqr in squares)
                    {
                        string source = sqr.Attribute("Provider").Value;
                        Coordinate location = Coordinate.Parse(sqr.Attribute("Location").Value);
                        int number = int.Parse(sqr.Attribute("Number").Value);
                        if (!source.Equals("Empty") && (number <= 0 || number > 9 || location == null))
                        {
                            return new GameData();
                        }
                        if (source.Equals("Initial"))
                        {
                            initVals.Add(location, number);
                        }
                        else if (source.Equals("User"))
                        {
                            userVals.Add(location, number);
                        }
                        else if (source.Equals("Solver"))
                        {
                            solverVals.Add(location, number);
                        }
                        else if (source.Equals("Empty"))
                        {
                            // Get candidates
                            string cands = sqr.Attribute("Candidates").Value;
                            if (cands.Length == 0)
                            {
                                continue;
                            }
                            List<string> nums_str = new List<string>(cands.Split(','));
                            List<int> nums = new List<int>();
                            foreach (string str in nums_str)
                            {
                                nums.Add(Int32.Parse(str));
                            }
                            candidates.Add(location, nums);
                        }
                        else
                        {
                            return new GameData();
                        }
                    }
                }
            }
            return new GameData(initVals, userVals, solverVals, candidates, layout);
        }


        /// <summary>
        /// Create xml root element from the GameData object.
        /// </summary>
        /// <param name="data">Game data to be saved.</param>
        /// <returns>Root element containing the game data.</returns>
        private XElement CreateRootElement(GameData data)
        {
            XElement root = new XElement("SudokuGame");

            // Save table.
            XElement table = new XElement("SudokuTable");
            table.Add(new XAttribute("Layout", data.Layout.ToString()));
            foreach (KeyValuePair<Coordinate, int> pair in data.InitialValues)
            {
                XElement square = new XElement("Square");
                square.Add(new XAttribute("Provider", "Initial"));
                square.Add(new XAttribute("Location", pair.Key));
                square.Add(new XAttribute("Number", pair.Value));
                square.Add(new XAttribute("Candidates", ""));
                table.Add(square);
            }
            foreach (KeyValuePair<Coordinate, int> pair in data.UserValues)
            {
                XElement square = new XElement("Square");
                square.Add(new XAttribute("Provider", "User"));
                square.Add(new XAttribute("Location", pair.Key));
                square.Add(new XAttribute("Number", pair.Value));
                square.Add(new XAttribute("Candidates", ""));
                table.Add(square);
            }
            foreach (KeyValuePair<Coordinate, int> pair in data.SolverValues)
            {
                XElement square = new XElement("Square");
                square.Add(new XAttribute("Provider", "Solver"));
                square.Add(new XAttribute("Location", pair.Key));
                square.Add(new XAttribute("Number", pair.Value));
                square.Add(new XAttribute("Candidates", ""));
                table.Add(square);
            }
            foreach (KeyValuePair<Coordinate, List<int>> pair in data.Candidates)
            {
                XElement square = new XElement("Square");
                square.Add(new XAttribute("Provider", "Empty"));
                square.Add(new XAttribute("Location", pair.Key));
                square.Add(new XAttribute("Number", 0));
                square.Add(new XAttribute("Candidates", string.Join(",", pair.Value)));
                table.Add(square);
            }

            root.Add(table);
            return root;
        }
    }
}
