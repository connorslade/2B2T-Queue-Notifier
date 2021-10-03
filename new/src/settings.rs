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

    send_on_login(bool),
    send_on_logout(bool),
    send_on_position_change(bool),
}

impl Config {
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
            ConfigUpdate::send_on_login(send_on_login) => Config {
                toast_settings: ToastSettings {
                    send_on_login,
                    ..self.toast_settings
                },
                ..self.clone()
            },
            ConfigUpdate::send_on_logout(send_on_logout) => Config {
                toast_settings: ToastSettings {
                    send_on_logout,
                    ..self.toast_settings
                },
                ..self.clone()
            },
            ConfigUpdate::send_on_position_change(send_on_position_change) => Config {
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
        Config {
            timeout: 30,
            tick_delay: 10,
            log_file_path:
                r#"C:\Users\turtl\Software\MultiMC\instances\1.12.2\.minecraft\latest.log"#
                    .to_string(),
            chat_regex: "Position in queue:".to_string(),
            toast_settings: ToastSettings {
                send_on_login: true,
                send_on_logout: true,
                send_on_position_change: true,
            },
        }
    }
}
