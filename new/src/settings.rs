use std::fmt::Display;
use std::fs;
use std::fs::OpenOptions;
use std::io::Read;
use std::path::Path;
use std::path::PathBuf;

use directories::BaseDirs;
use regex::Regex;
use simple_config_parser as scp;

use super::style;
use super::style::Theme;
use super::VERSION;

#[derive(Debug, Clone)]
pub struct ToastSettings {
    pub send_on_login: bool,
    pub send_on_logout: bool,
    pub send_on_position_change: bool,
}

#[derive(Debug, Clone)]
pub struct Config {
    pub online_timeout: u64,
    pub timeout: u64,
    pub log_file_path: String,
    pub chat_regex: Regex,
    pub toast_settings: ToastSettings,
    pub theme: style::Theme,
}

#[derive(Debug, Clone)]
pub enum ConfigUpdate {
    OnlineTimeout(u64),
    Timeout(u64),
    LogFilePath(String),
    ChatRegex(Regex),

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

        let cfg = scp::Config::new().text(data).ok()?;

        Some(Config {
            online_timeout: cfg.get("online_timeout").ok()?,
            timeout: cfg.get("timeout").ok()?,
            log_file_path: cfg.get("log_file_path").ok()?,
            chat_regex: Regex::new(&cfg.get_str("chat_regex").ok()?).expect("Invalid chat regex"),

            toast_settings: ToastSettings {
                send_on_login: cfg.get("toast_send_on_login").ok()?,
                send_on_logout: cfg.get("toast_send_on_logout").ok()?,
                send_on_position_change: cfg.get("toast_send_on_position_change").ok()?,
            },
            theme: Theme::from_string(cfg.get("theme").ok()?)?,
        })
    }

    pub fn save(&self, path: PathBuf) {
        println!("[*] Saving Config");

        fs::create_dir_all(path.parent().unwrap()).unwrap();
        let configs: Vec<(&str, Box<dyn Display>)> = vec![
            ("theme", Box::new(self.theme)),
            ("online_timeout", Box::new(self.online_timeout)),
            ("timeout", Box::new(self.timeout)),
            ("log_file_path", Box::new(self.log_file_path.to_string())),
            ("chat_regex", Box::new(self.chat_regex.to_string())),
            (
                "toast_send_on_login",
                Box::new(self.toast_settings.send_on_login),
            ),
            (
                "toast_send_on_logout",
                Box::new(self.toast_settings.send_on_logout),
            ),
            (
                "toast_send_on_position_change",
                Box::new(self.toast_settings.send_on_position_change),
            ),
        ];
        let config = configs
            .iter()
            .map(|(cfg, val)| format!("{} = {}", cfg, val))
            .collect::<Vec<_>>()
            .join("\n");

        fs::write(
            path,
            format!("; 2B2T-Queue-Notifier V{} Config\n{}", VERSION, config),
        )
        .unwrap();
    }

    pub fn apply_update(&self, update: ConfigUpdate) -> Config {
        match update {
            ConfigUpdate::OnlineTimeout(online_timeout) => Config {
                online_timeout,
                ..self.clone()
            },
            ConfigUpdate::Timeout(timeout) => Config {
                timeout,
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
        // Get Default Minecraft Log File Path
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
            .join(Path::new(".minecraft/logs/latest.log"));

        Config {
            online_timeout: 30,
            timeout: 10,
            log_file_path: log_path.to_str().unwrap().to_string(),
            chat_regex: Regex::new("Position in queue: (\\d*)").unwrap(),
            toast_settings: ToastSettings {
                send_on_login: true,
                send_on_logout: true,
                send_on_position_change: true,
            },
            theme: style::Theme::Dark,
        }
    }
}
