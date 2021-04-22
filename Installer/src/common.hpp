#ifndef GREENDOTS_COMMON_HPP
#define GREENDOTS_COMMON_HPP

#include <vector>

namespace common {
    std::vector<std::string> tokenize(std::string const &str, const char delim);

    std::string cleanConfigString(const std::string &configString);

    bool updateFile(const std::string &fileName, const std::string &text);

    bool exists(const std::string &name);

    std::string getDateAsString();

    bool createFile(const std::string &path, const std::string &outData);

    void sleep(int ms);

    std::string getOsNullPipe();

    std::string cleanUserInput(const std::string &input);

    bool runSystemCommand(const std::string &command);

    auto read_file(const std::string& path) -> std::string;

    void waitForKeypress(const std::string& text, int colorCode);

    std::string stringToLower(std::string data);

    bool moveFile(const std::string& oldName, const std::string& newName);
}
#endif
