#-------------------------------------------------
#
# Project created by QtCreator 2015-01-16T18:10:52
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = GUI
TEMPLATE = app
CONFIG += c++11

INCLUDEPATH += /Widgets \
               /GraphicsItems \
               /Other

SOURCES += main.cc\
        mainwindow.cc \
    Widgets/sudokuwidget.cc \
    GraphicsItems/sudokusquareitem.cc \
    Other/sudokutablebuilder.cc

HEADERS  += mainwindow.hh \
    Widgets/sudokuwidget.hh \
    GraphicsItems/sudokusquareitem.hh \
    Other/sudokutablebuilder.hh

FORMS    += mainwindow.ui
