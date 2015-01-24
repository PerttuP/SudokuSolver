
/*
 * sudokusquare.cc
 * This implementation file implements SudokuSquareItem class defined
 * in sudokusquareitem.cc.
 * 
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 19-Jan-2015
 */  


#include "sudokusquareitem.hh"
#include <QDebug>
#include <QPainter>

namespace SudokuGUI {


SudokuSquareItem::SudokuSquareItem(int x, int y, int width, 
                                   const QColor& bg_color, 
                                   QGraphicsItem* parent) :
    
    QObject(), QGraphicsItem(parent),
    x_(x), y_(y), width_(width), bg_color_(bg_color), 
    value_(0), type_(INITIAL), candidates_()
{
}


SudokuSquareItem::~SudokuSquareItem()
{    
}


void SudokuSquareItem::setValue(int value, SudokuSquareItem::ValueType type)
{
    Q_ASSERT(value >= 0 && value <= 9);
    value_ = value;
    type_ = type;
    candidates_.clear();
}


void SudokuSquareItem::setCandidates(const std::set<int>& candidates)
{
    for (int c : candidates){
        Q_ASSERT(c > 0 && c <10);
    }
    
    candidates_ = candidates;
}


void SudokuSquareItem::addCandidate(int candidate)
{
    Q_ASSERT(candidate > 0 && candidate <= 9);
    candidates_.insert(candidate);
}


void SudokuSquareItem::removeCandidate(int candidate)
{
    candidates_.erase(candidate);
}


void SudokuSquareItem::clear()
{
    candidates_.clear();
    value_ = 0;
}


bool SudokuSquareItem::empty() const
{
    return value_ == 0;
}


QRectF SudokuSquareItem::boundingRect() const
{
    return QRectF(0, 0, width_, width_);
}


void SudokuSquareItem::paint(QPainter* painter, 
                             const QStyleOptionGraphicsItem* option,
                             QWidget* widget)
{
    Q_UNUSED(option); Q_UNUSED(widget);
    
    //Draw bounding rect and fill it with bg_color
    QPen pen(Qt::black);
    pen.setWidth(1);
    painter->setPen(pen);
    painter->setBrush( QBrush(bg_color_) );
    painter->drawRect( this->boundingRect() );
    
    // Draw value / candidates.
    if (value_ != 0){
        this->drawValue(painter);
    }
    else{
        this->drawCandidates(painter);
    }
}

void SudokuSquareItem::mousePressEvent(QGraphicsSceneMouseEvent *event)
{
    emit clicked(x_, y_, event);
}


// Paints current value into square.
void SudokuSquareItem::drawValue(QPainter* painter)
{
    // Set text font
    QFont font;
    font.setPixelSize(width_ - 4);
    painter->setFont(font);
    QPen pen;
    
    // Select text color after value type.
    switch (type_) {
    case INITIAL:
        pen.setColor(Qt::black);
        break;
    case USER:
        pen.setColor(Qt::green);
        break;
    case SOLVER:
        pen.setColor(Qt::blue);
        break;
    case CORRECTED:
        pen.setColor(Qt::red);
        break;
    default:
        Q_ASSERT(false);
    }
    
    painter->setPen(pen);
    painter->drawText(this->boundingRect(), QString::number(value_));
}


// Paints current candidates into square
void SudokuSquareItem::drawCandidates(QPainter* painter)
{
    // Set font and color
    QFont font;
    font.setPixelSize(width_/3 - 4);
    painter->setFont(font);
    painter->setPen( QPen(Qt::black) );
    
    int on_row (0), x_pos(0), y_pos(0);
    const int STEP = width_/3;
    
    for (int c : candidates_){
        // Find position for candidate.
        if (on_row == 3){
            // Change row
            y_pos += STEP;
            x_pos = 0;
            on_row = 1;
        }
        else {
            x_pos += STEP;
            ++on_row;
        }
        
        // Draw candidate and a bounding box for it.
        QRectF box(x_pos, y_pos, STEP, STEP);
        painter->drawRect(box);
        painter->drawText(box, QString::number(c) );
    }
}



} // Namespace
