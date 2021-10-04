use std::fs::OpenOptions;
use std::io::Read;
use std::io::Write;
use std::path::Path;
use std::path::PathBuf;

use directories::BaseDirs;
use simple_config_parser::config;

use super::VERSION;

#[derive(Debug, Clone)]
pub struct ToastSettings {
    pub send_on_login: bool,
    pub send_on_logout: bool,
    pub send_on_position_change: bool,
}

#[derive(Debug, Clone)]
pub struct Config {
    pub timeout: u64,
    pub tick_delay: u64,
    pub log_file_path: String,
    pub chat_regex: String,
    pub toast_settings: ToastSettings,
}

#[derive(Debug, Clone)]
pub enum ConfigUpdate {
    Timeout(u64),
    TickDelay(u64),
    LogFilePath(String),
    ChatRegex(String),

    SendOnLogin(bool),
    SendOnLogout(bool),
    SendOnPositionChange(bool),
}

impl Config {
    pub fn load(path: PathBuf) -> Option<Config> {
        if !path.exists() {
            return None;
        }
        let mut file = match OpenOptions::new().read(true).open(path) {
            Ok(file) => file,
            Err(_) => return None,
        };
        let mut data = String::new();
        match file.read_to_string(&mut data) {
            Ok(_) => (),
            Err(_) => return None,
        };

        let mut cfg = config::Config::new(None);
        match cfg.parse(&data.replace('\r', "")) {
            Ok(_) => {}
            Err(_) => return None,
        };

        Some(Config {
            timeout: match cfg.get("timeout")?.parse() {
                Ok(timeout) => timeout,
                Err(_) => return None,
            },
            tick_delay: match cfg.get("tick_delay")?.parse() {
                Ok(tick_delay) => tick_delay,
                Err(_) => return None,
            },
            log_file_path: cfg.get("log_file_path")?,
            chat_regex: cfg.get("chat_regex")?,

            toast_settings: ToastSettings {
                send_on_login: cfg.get_bool("toast_send_on_login")?,
                send_on_logout: cfg.get_bool("toast_send_on_logout")?,
                send_on_position_change: cfg.get_bool("toast_send_on_position_change")?,
            },
        })
    }

    pub fn save(&self, path: PathBuf) {
        // Make folder
        std::fs::create_dir_all(path.parent().unwrap()).unwrap();

        // Open file
        let mut file = OpenOptions::new()
            .write(true)
            .create(true)
            .open(path)
            .unwrap();

        // write!(
        //     file,
        //     "; 2B2T-Queue-Notifier V{} Config\ntimeout = {}\tick_delay = {}",
        //     VERSION, self.timeout, self.tick_delay
        // )
        .unwrap();
    }

    pub fn apply_update(&self, update: ConfigUpdate) -> Config {
        match update {
            ConfigUpdate::Timeout(timeout) => Config {
                timeout,
                ..self.clone()
            },
            ConfigUpdate::TickDelay(tick_delay) => Config {
                tick_delay,
                ..self.clone()
            },
            ConfigUpdate::LogFilePath(log_file_path) => Config {
                log_file_path,
                ..self.clone()
            },
            ConfigUpdate::ChatRegex(chat_regex) => Config {
                chat_regex,
                ..self.clone()
            },
            ConfigUpdate::SendOnLogin(send_on_login) => Config {
                toast_settings: ToastSettings {
                    send_on_login,
                    ..self.toast_settings
                },
                ..self.clone()
            },
            ConfigUpdate::SendOnLogout(send_on_logout) => Config {
                toast_settings: ToastSettings {
                    send_on_logout,
                    ..self.toast_settings
                },
                ..self.clone()
            },
            ConfigUpdate::SendOnPositionChange(send_on_position_change) => Config {
                toast_settings: ToastSettings {
                    send_on_position_change,
                    ..self.toast_settings
                },
                ..self.clone()
            },
        }
    }
}

impl Default for Config {
    fn default() -> Self {
        // Get Minecraft Path
        #[cfg(target_os = "windows")]
        let log_path = BaseDirs::new()
            .unwrap()
            .data_dir()
            .join(Path::new(".minecraft\\logs\\latest.log"));

        #[cfg(target_os = "macos")]
        let log_path = BaseDirs::new()
            .unwrap()
            .data_dir()
            .join(Path::new("minecraft/logs/latest.log"));

        #[cfg(not(any(target_os = "windows", target_os = "macos")))]
        let log_path = BaseDirs::new()
            .unwrap()
            .data_dir()
            .join(Path::new("/.minecraft/logs/latest.log"));

        Config {
            timeout: 30,
            tick_delay: 10,
            log_file_path: log_path.to_str().unwrap().to_string(),
            chat_regex: "Position in queue:".to_string(),
            toast_settings: ToastSettings {
                send_on_login: true,
                send_on_logout: true,
                send_on_position_change: true,
            },
        }
    }
}
