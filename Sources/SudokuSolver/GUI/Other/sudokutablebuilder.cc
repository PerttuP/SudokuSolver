#include "sudokutablebuilder.hh"

namespace SudokuGUI{


SudokuTableBuilder::SqrVec SudokuTableBuilder::createSquares(int width)
{
    SqrVec r_val(9);
    
    try
    {
        for (int x=0; x<9; ++x){
            for (int y=0; y<9; ++y){
                // Find background color.
                QColor bg_color(Qt::white);
                if (isDark(x,y)){
                    bg_color = Qt::lightGray;
                }
                
                r_val[x].push_back(new SudokuSquareItem(x+1, y+1, width, 
                                                        bg_color) );
            }
        }
    }
    catch(...){
        for (std::vector<SudokuSquareItem*> sqr_v : r_val){
            for (auto it=sqr_v.begin(); it!=sqr_v.end(); ++it){
                delete *it;
            }
        }
        throw;
    }
    
    return r_val;
}


// Check if given coordinate pair is "dark" in default layout.
bool SudokuTableBuilder::isDark(int x, int y)
{
    if (y>2 && y<6){
        if (x>2 && x<6){
            return true;
        }
    }
    else if (x<3 || x>5){
        return true;
    }
    return false;
}


} // Namespace
