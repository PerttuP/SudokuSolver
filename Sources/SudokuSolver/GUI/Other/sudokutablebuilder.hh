/*
 * sudokutablebuilder.hh
 * Header file for the SudokuBuilder class that is responsible for creating
 * SudokuWidget's member squares properly.
 *
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 14-Jan-2015
 */

#ifndef SUDOKUTABLEBUILDER_HH
#define SUDOKUTABLEBUILDER_HH

#include <vector>
#include "../GraphicsItems/sudokusquareitem.hh"


namespace SudokuGUI
{

/*!
 * \brief The SudokuTableBuilder class
 * Class that is responsible for creating SudokuSquareItem objects for
 * SudokuWidget.
 */
class SudokuTableBuilder
{
public:
    
    //! Typedef for createSquares-method's return value.
    typedef std::vector<std::vector<SudokuSquareItem*>> SqrVec;
    
    //! Constructor
    SudokuTableBuilder() = default;
    
    //! Destructor
    ~SudokuTableBuilder() = default;
    
    /*!
     * \brief createSquares Creates 81 SudokuSquareItem objects.
     * \param width Square width.
     * \return New objects in vectors. The caller takes the ownership over
     * new objects.
     */
    static SqrVec createSquares(int width);
    
    
private:
    
    static bool isDark(int x, int y);
};


} // Namespace

#endif // SUDOKUTABLEBUILDER_HH
