using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLib
{
    public class SolverFactory
    {
        public static ISudokuSolver SimpleSolver()
        {
            return new SimpleSolver();
        } 
    }
}
