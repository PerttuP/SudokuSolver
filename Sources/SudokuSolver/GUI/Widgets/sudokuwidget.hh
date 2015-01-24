#ifndef SUDOKUWIDGET_HH
#define SUDOKUWIDGET_HH

#include <QWidget>
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QGridLayout>
#include <vector>
#include "../GraphicsItems/sudokusquareitem.hh"


namespace SudokuGUI {

class SudokuWidget : public QWidget
{
public:
    
    SudokuWidget(QWidget* parent);
    
    virtual ~SudokuWidget();
    
signals:
    
    void valueSelected(int x, int y, int value);
    void candidateSelected(int x, int y, int candidate);
    
private:
    
    typedef std::vector<std::vector<SudokuSquareItem*> > SqrVec;
    
    SqrVec squares_;
    QGraphicsView* view_;
    QGraphicsScene* scene_;
    
    
private slots:
    void init();
};

} // Namespace

#endif // SUDOKUWIDGET_HH
