// Common misc functions used in multiple places

#include <sys/stat.h>
#include <algorithm>
#include <fstream>
#include <chrono>
#include <thread>
#include <vector>

#include "console.hpp"

namespace common {
    // Split a string into a vector by a certain char
    std::vector<std::string> tokenize(std::string const &str, const char delim) {
        std::vector<std::string> out;
        size_t start;
        size_t end = 0;

        while ((start = str.find_first_not_of(delim, end)) != std::string::npos) {
            end = str.find(delim, start);
            out.push_back(str.substr(start, end - start));
        }
        return out;
    }

    // Remove quotes and newlines from config values
    std::string cleanConfigString(const std::string &configString) {
        std::vector<std::string> quotesSplit = tokenize(configString, '"');
        std::string final;
        if (quotesSplit.size() >= 2) final = quotesSplit[1];

        return final;
    }

    // Overwrite a files content
    bool updateFile(const std::string &fileName, const std::string &text) {
        std::fstream fileHandler;
        std::string myLine;
        fileHandler.open(fileName, std::ios::in | std::ios::out);
        if (fileHandler.fail()) return false;
        fileHandler << text;
        fileHandler.close();

        return true;
    }

    // Check if file exists
    bool exists(const std::string &name) {
        if (name.empty()) return false;
#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)
        struct stat buffer{};
        return (stat(name.c_str(), &buffer) == 0);
#else
        std::ifstream file(name);
        return file.is_open();
#endif
    }

    // Returns the current date as a string (YYYY-MM-DD)
    std::string getDateAsString() {
        std::time_t t = std::time(nullptr);
        std::tm *now = std::localtime(&t);
        std::string time = std::to_string(now->tm_year + 1900) + "-" +
                           std::to_string(now->tm_mon + 1) + "-"
                           + std::to_string(now->tm_mday);


        return time;
    }

    // Create a file with content
    bool createFile(const std::string &path, const std::string &outData) {
        std::ofstream outfile(path);
        if (outfile.fail()) return false;
        outfile << outData;
        outfile.close();
        return true;
    }

    // sleep for n milliseconds
    void sleep(int ms) {
        std::this_thread::sleep_for(std::chrono::milliseconds(ms));
    }

    // Get the os null Pipe (/dev/null or >nul 2>&1)
    std::string getOsNullPipe() {
#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)
        return ">nul 2>&1";
#else
        return "> /dev/null 2>&1";
#endif
    }

    // Remove chars that users could use to execute commands
    std::string cleanUserInput(const std::string &input) {
        char badChar[3] = {';', '&', '|'};
        std::string working = input;
        for (char i : badChar) working.erase(remove(working.begin(), working.end(), i), working.end());
        return working;
    }

    // Run commands and return true if successfully (false if not)
    bool runSystemCommand(const std::string &command) {
        int result = system(command.c_str());
        if (result != 0) return false;
        return true;
    }

    // Read File to a String
    auto read_file(const std::string& path) -> std::string {
        constexpr auto read_size = std::size_t{4096};
        auto stream = std::ifstream{path.data()};
        stream.exceptions(std::ios_base::badbit);

        auto out = std::string{};
        auto buf = std::string(read_size, '\0');
        while (stream.read(&buf[0], read_size)) {
            out.append(buf, 0, stream.gcount());
        }
        out.append(buf, 0, stream.gcount());
        return out;
    }
}
