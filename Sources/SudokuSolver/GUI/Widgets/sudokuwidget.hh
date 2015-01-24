
/*
 * sudokuwidget.hh
 * This is the header file for SudokuWidget class - Custom widget that 
 * represents a sudokutable.
 * 
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 24-Jan-2015
 */


#ifndef SUDOKUWIDGET_HH
#define SUDOKUWIDGET_HH

#include <QWidget>
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QGridLayout>
#include <vector>
#include "../GraphicsItems/sudokusquareitem.hh"


namespace SudokuGUI {


/*!
 * \brief The SudokuWidget class
 * Custom widget that represents a sudoku table.
 */
class SudokuWidget : public QWidget
{
    Q_OBJECT
    
public:
    
    /*!
     * \brief SudokuWidget Constructor
     * \param parent QWidget's parent widget.
     */
    explicit SudokuWidget(QWidget* parent = 0);
    
    /*!
     * \brief ~SudokuWidget Destructor.
     */
    virtual ~SudokuWidget();
    
    
signals:
    
    /*!
     * \brief valueSelected Signals, that user has selected value for a square.
     * \param x Square's x-coordinate.
     * \param y Square's y-coordinate.
     * \param value Selected value.
     */
    void valueSelected(int x, int y, int value);
    
    /*!
     * \brief candidateSelected Signals that user has selected a candidate to
     *  be added to/ removed from a square.
     * \param x Square's x-coordinate.
     * \param y Square's y-coordinate.
     * \param candidate Candidate to be added / removed.
     */
    void candidateSelected(int x, int y, int candidate);
  
    
private:
    
    typedef std::vector<std::vector<SudokuSquareItem*> > SqrVec;
    
    SqrVec squares_;
    QGraphicsView* view_;
    QGraphicsScene* scene_;
    
    
private slots:
    void init();
    void squareClicked(int x, int y, QGraphicsSceneMouseEvent* event);
};

} // Namespace

#endif // SUDOKUWIDGET_HH
