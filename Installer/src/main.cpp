#include <iostream>
#include <string>

#include "console.hpp"
#include "setup.hpp"

#define version "0.0.0"

int main() {
    console::enableAnsiCodes();
    console::setWindowName("Easy Minecraft Deploy");
    console::debugPrint("\x1B[1;32m[*] Welcome to the 2B2T-Queue-Notifier Installer! ", 32, "");
    console::debugPrint(version, 35, "\n\n");

    // Download Version.json to a string then parse it to get ExeUri and Filename
    console::debugPrint("[*] Downloading Version Manifest", 33, " ");
    std::string versionJson = setup::getNewVersion("https://raw.githubusercontent.com/Basicprogrammer10/2B2T-Queue-Notifier/master/version.json", "version.json");
    std::string exeUri = setup::downloadUriFromVersionJSON(versionJson);
    std::string fileName = setup::getFileNameFromUri(exeUri);
    console::debugPrint("[ SUCCESS ]", 32);

    console::debugPrint("[*] Downloading EXE", 33, " ");
    setup::downloadFileFromUri(exeUri, fileName);
    console::debugPrint("[ SUCCESS ]", 32);

    return 0;
}

/*
 * Add Setup stuff to a setup.cpp
 * Cleanup Common.cpp
 * this is really bad
 */