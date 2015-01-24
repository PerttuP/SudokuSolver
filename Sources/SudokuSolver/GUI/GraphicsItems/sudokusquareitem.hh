
/*
 * sudokusquareitem.hh
 * This header file defines a reimplemented QGraphicsItem calss that represents
 * an individual square on GUI's sudoku table.
 * 
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 19-Jan-2015.
 */


#ifndef SUDOKUSQUAREITEM_HH
#define SUDOKUSQUAREITEM_HH

#include <QObject>
#include <QGraphicsItem>
#include <QColor>
#include <set>


namespace SudokuGUI
{

/*!
 * \brief The SudokuSquareItem class
 *  Graphical representation of individual sudoku square.
 */
class SudokuSquareItem : public QObject, public QGraphicsItem
{
    Q_OBJECT
    Q_INTERFACES(QGraphicsItem)
    
public:
    
    /*!
     * \brief The ValueType enum
     *  ValueType indicates source of squares value, and
     *  determines value's painted color.
     */
    enum ValueType{
        INITIAL,    // Initial value
        USER,       // Value set by the user
        SOLVER,     // Value set by solving algorithm
        CORRECTED   // Value corrected by solving algorithm
    };
    
    /*!
     * \brief SudokuSquareItem Constructor.
     * \param x x-coordinate on the sudoku table.
     * \param y y-coordinate on the sudoku table.
     * \param width Square's width when painted.
     * \param bg_color Square's background color when painted.
     * \param parent QGraphicsItem's parent.
     * 
     * \pre x and y are in range [1,9]. width > 0.
     * \post Square has no value or candidates.
     */
    SudokuSquareItem(int x, int y, int width, const QColor& bg_color, 
                     QGraphicsItem* parent = 0);
    
    /*!
     * \brief ~SudokuSquareItem Destructor.
     */
    virtual ~SudokuSquareItem();
    
    /*!
     * \brief setValue Sets a value to the square.
     * \param value Value to be set.
     * \param type Value source.
     * \pre value is in range [1,9].
     * \post Value is set. Candidates are removed.
     */
    void setValue(int value, ValueType type = INITIAL);
    
    /*!
     * \brief setCandidates Sets candidates to be painted.
     * \param candidates Set of candidates.
     * \pre Each element in candidates is in range [1,9].
     *      Square has no value set (empty() == true).
     * \post Candidates are set. Previous candidates are erased.
     */
    void setCandidates(const std::set<int>& candidates);
    
    /*!
     * \brief addCandidate Adds new candidate to be painted.
     * \param candidate New candidate.
     * \pre Candidate is in range [1,9]. Square is empty.
     * \post New candidate is added. Previous candidates are not affected.
     */
    void addCandidate(int candidate);
    
    /*!
     * \brief removeCandidate Removes given candidate.
     * \param candidate Candidate to be removed.
     * \pre None.
     * \post Candidate is no more in square's candidates.
     */
    void removeCandidate(int candidate);
    
    /*!
     * \brief clear Removes all candidates and value.
     * \pre None.
     * \post Square has no value or candidates.
     */
    void clear();
    
    /*!
     * \brief empty Checks if square is empty.
     * \return True, if square has no value set. Candidates don't matter.
     * \pre None.
     */
    bool empty() const;
    
    /*!
     * \brief boundingRect Implements QGraphicsItem interface.
     * \return Outer-most boundaries of item.
     * \pre None.
     */
    QRectF boundingRect() const;
    
    /*!
     * \brief paint Implements QGraphicsItem interface.
     *  Qt libraries call this when item needs to be painted.
     * \param painter Used painter.
     * \param option Not used here.
     * \param widget Not used here.
     */
    void paint(QPainter* painter, 
               const QStyleOptionGraphicsItem* option, 
               QWidget* widget);
    
    
signals:
    
    /*!
     * \brief clicked Qt-signal that notifies listeners when item is clicked.
     * \param x Square's x-coordinate.
     * \param y Square's y-coordinate.
     * \param event Mouse press event.
     */
    void clicked(int x, int y, QGraphicsSceneMouseEvent* event);
    
    
protected:
    
    /*!
     * \brief mousePressEvent Event handler for mouse press.
     * \param event Mouse press event.
     */
    void mousePressEvent(QGraphicsSceneMouseEvent* event);
    
    
private:
    
    int x_;
    int y_;
    int width_;
    QColor bg_color_;
    int value_;
    ValueType type_;
    std::set<int> candidates_;
    
    void drawValue(QPainter* painter);
    void drawCandidates(QPainter* painter);
};


} // Namespace

#endif // SUDOKUSQUAREITEM_HH
