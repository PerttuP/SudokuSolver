
/*
 * mainwindow.hh
 * Header file for the MainWindow class.
 * 
 * Author: Perttu Paarlahti
 * Created: 19-Jan-2015
 * Last modified: 14-Jan-2015
 */


#ifndef MAINWINDOW_HH
#define MAINWINDOW_HH

#include <QMainWindow>

namespace Ui {
class MainWindow;
}


namespace SudokuGUI
{

/*!
 * \brief The MainWindow class
 * Application's main window.
 */
class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    
    /*!
     * \brief MainWindow constructor
     * \param parent Window's parent.
     */
    explicit MainWindow(QWidget *parent = 0);
    
    /*!
     * \brief ~MainWindow Destructor.
     */
    virtual ~MainWindow();
    
    
private:
    Ui::MainWindow *ui;
    

private slots:
    void init();
};


} // Namespace


#endif // MAINWINDOW_HH
