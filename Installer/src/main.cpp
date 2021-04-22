#include <iostream>
#include <string>

#include "console.hpp"
#include "common.hpp"
#include "setup.hpp"

#define version "0.0.0"
#define versionJsonUri "https://raw.githubusercontent.com/Basicprogrammer10/2B2T-Queue-Notifier/master/version.json"

int main() {
    std::string tmp;

    console::enableAnsiCodes();
    console::setWindowName("Easy Minecraft Deploy");
    console::debugPrint("\x1B[1;32m[*] Welcome to the 2B2T-Queue-Notifier Installer! ", 32, "");
    console::debugPrint(version, 35, "\n\n");

    // Download Version.json to a string then parse it to get ExeUri and Filename
    console::debugPrint("[*] Downloading Version Manifest", 33, " ");
    std::string versionJson = setup::getNewVersion(versionJsonUri, "version.json");
    std::string exeUri = setup::downloadUriFromVersionJSON(versionJson);
    std::string fileName = setup::getFileNameFromUri(exeUri);
    console::debugPrint("[ SUCCESS ]", 32);

    // Download exe from path specified in version.json
    console::debugPrint("[*] Downloading EXE", 33, " ");
    setup::downloadFileFromUri(exeUri, fileName);
    console::debugPrint("[ SUCCESS ]", 32);

    console::debugPrint("[*] Add to start Menu? [ Y / n ] ", 33, "");
    std::getline(std::cin, tmp);
    if (common::stringToLower(tmp) != "n") {
        console::debugPrint("[*] Adding to start Menu", 33, " ");
        char* appdata = getenv("APPDATA");
        std::string path = R"(C:\Users\turtl\AppData\Roaming\Microsoft\Windows\Start Menu\Programs)";


        std::string sProgramsPath = getenv("PROGRAMDATA");
        std::string sShortcutPath = sProgramsPath += R"(\Microsoft\Windows\Start Menu\Programs\)" + fileName;

        if (!common::moveFile(fileName, fileName+ "s")) {
            std::cout << "error ;(";
        }
        return 0;
    }

    return 0;
}

/*
 X Add Setup stuff to a setup.cpp
 * Cleanup Common.cpp
 X this is really bad
 */