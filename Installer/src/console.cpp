// Functions for interacting with the console
// EX - Printing and enabling ansi Codes (On Windows)

#include <string>
#include <iostream>

#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)
#include <windows.h>
#endif

namespace console {
    // Enables ANSI codes in windows
    void enableAnsiCodes() {
#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)
        DWORD l_mode;
        HANDLE hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(hStdout, &l_mode);
        SetConsoleMode(hStdout, l_mode | 0x0004 | 0x0008);
#endif
    }

    // Prints text with color using ansi codes
    void debugPrint(const std::string &text, int colorCode, const std::string &stringEnd = "\n") {
        std::cout << "\x1B[" << colorCode << "m" << text << "\033[0m" << stringEnd;
    }

    // Prints text using DebugPrint and exits the program after a pause
    void errorPrint(const std::string &text, int colorCode, int exitCode = 0) {
        debugPrint(text, colorCode);
        if (exitCode != 0) exit(exitCode);
    }
}