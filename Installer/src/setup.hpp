#ifndef INC_2B2T_QUEUE_NOTIFIER_INSTALLER_SETUP_HPP
#define INC_2B2T_QUEUE_NOTIFIER_INSTALLER_SETUP_HPP
namespace setup {
    std::string getNewVersion(std::string baseUri, const std::string& name);

    std::string downloadUriFromVersionJSON(const std::string& versionJson);

    void downloadFileFromUri(const std::string& uri, const std::string& fileName);

    std::string getFileNameFromUri(const std::string& Uri);
}
#endif