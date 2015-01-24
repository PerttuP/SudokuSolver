#ifndef SUDOKUTABLEBUILDER_HH
#define SUDOKUTABLEBUILDER_HH

#include <vector>
#include "../GraphicsItems/sudokusquareitem.hh"

namespace SudokuGUI
{

class SudokuTableBuilder
{
public:
    
    typedef std::vector<std::vector<SudokuSquareItem*>> SqrVec;
    
    SudokuTableBuilder() = default;
    ~SudokuTableBuilder() = default;
    
    static SqrVec createSquares(int width);
    
private:
    
    static bool isDark(int x, int y);
};


} // Namespace

#endif // SUDOKUTABLEBUILDER_HH
