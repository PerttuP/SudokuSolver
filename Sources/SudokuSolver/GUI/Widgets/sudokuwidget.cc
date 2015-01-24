#include "sudokuwidget.hh"
#include "../Other/sudokutablebuilder.hh"
#include <QTimer>

namespace SudokuGUI {

SudokuWidget::SudokuWidget(QWidget* parent):
    QWidget(parent), squares_(),
    view_( new QGraphicsView(this) ), 
    scene_( new QGraphicsScene(this) )
{   
    // Actual initialition is delayed so that construction will be
    // complete at that time.
    QTimer::singleShot(50, this, SLOT(init()) );
}


SudokuWidget::~SudokuWidget()
{
    scene_->clear(); // Destroyes squares.
    squares_.clear();
    // Scene, view and layout will be automaticly destroyed as children.
}



// Sets squares in their places.
void SudokuWidget::init()
{
    // Create empty squares
    const int WIDTH = view_->width();
    squares_ = SudokuTableBuilder::createSquares(WIDTH);
    
    // Add squares in the scene.
    int x_pos(0), y_pos(0);
    for (int x=0; x<9; ++x){
        for (int y=0; y<9; ++y){
            SudokuSquareItem* sqr = squares_[x][y];
            sqr->setPos(x_pos, y_pos);
            scene_->addItem(sqr);
            y_pos += WIDTH;
        }
        y_pos = 0;
        x_pos += WIDTH;
    }
    
    view_->setScene(scene_);
}


} // Namespace
