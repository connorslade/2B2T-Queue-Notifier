#include <string>
#include <iostream>

#include "common.hpp"
#include "console.hpp"

namespace setup {
    // Im sorry...
    std::string getNewVersion(std::string baseUri, const std::string &name) {
        std::string fileName = ".installer.tmp";
        std::string command = "curl " + baseUri + " -f -A EasyMinecraftDeploy -o " + fileName + " >nul 2>&1";
        int status = system(command.c_str());
        if (status != 0)
            console::errorPrint("[ FAILED ]", 31, -1);
        std::string manifest = common::read_file(fileName);
        remove(fileName.c_str());
        return manifest;
    }

    // Does what it says on the tin...
    std::string downloadUriFromVersionJSON(const std::string &versionJson) {
        std::vector <std::string> out;
        std::string output;

        out = common::tokenize(versionJson, '"');
        for (int i = 0; i < out.size(); i++) if (out[i] == "download") output = out[i + 2];

        return output;
    }

    // Download file from Interwebz
    void downloadFileFromUri(const std::string &uri, const std::string &fileName) {
        std::string command = "curl " + uri + " -f -A 2B2T-Queue-Installer -o " + fileName + " >nul 2>&1";
        int status = system(command.c_str());
        if (status != 0)
            console::errorPrint("[ FAILED ]", 31, -1);
        std::string manifest = common::read_file(fileName);
    }

    // Parse Uri to get Filename
    std::string getFileNameFromUri(const std::string &Uri) {
        std::string output;

        std::vector <std::string> out = common::tokenize(Uri, '/');
        output = out[out.size() - 1];
        return output;
    }
}