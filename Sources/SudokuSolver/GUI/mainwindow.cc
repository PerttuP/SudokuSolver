
/*
 * mainwindow.cc
 * Implementation file for the MainWindow class defined in mainwindow.hh.
 * 
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 14-Jan-2015
 */


#include "mainwindow.hh"
#include "ui_mainwindow.h"
#include <QTimer>

namespace SudokuGUI 
{

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    QTimer::singleShot(200, this, SLOT(init()) );
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::init()
{
    ui->setupUi(this);
}


} // Namespace
