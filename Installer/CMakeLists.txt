cmake_minimum_required(VERSION 3.16.3)
project(2B2T-Queue-Notifier-Installer)

set(CMAKE_CXX_STANDARD 14)

add_executable(2B2T-Queue-Notifier-Installer src/main.cpp src/console.cpp src/console.hpp src/common.cpp src/common.hpp src/setup.cpp src/setup.hpp)
