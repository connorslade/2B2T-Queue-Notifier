#include <iostream>
#include <string>

#include "console.hpp"
#include "common.hpp"

#define version "0.0.0"

int main() {
    console::enableAnsiCodes();
    console::debugPrint("\x1B[1;32m[*] Welcome to the 2B2T-Queue-Notifier Installer! ", 32, "");
    console::debugPrint(version, 35, "\n\n");

    // DDownload Version.json to a string
    std::string versionJson = common::getNewVersion("https://raw.githubusercontent.com/Basicprogrammer10/2B2T-Queue-Notifier/master/version.json", "version.json");
    std::cout << common::downloadUriFromVersionJSON(versionJson);

    return 0;
}

/*
 * Add Setup stuff to a setup.cpp
 * Cleanup Common.cpp
 * this is really bad
 */