use iced::{
    button, window, Align, Button, Color, Column, Container, Element, Length, Sandbox, Settings,
    Text,
};

use super::style;
use super::VERSION;

struct ToastSettings {
    send_on_login: bool,
    send_on_logout: bool,
    send_on_position_change: bool,
}

struct Config {
    timeout: u64,
    tick_delay: u64,
    log_file_path: String,
    chat_regex: String,
    toast_settings: ToastSettings,
}

#[derive(Default)]
pub struct SettingsWindow {
    settings: Config,
}

#[derive(Debug, Clone, Copy)]
pub enum Message {
    OpenSettings,
    SetPosition(u32),
}

impl Default for Config {
    fn default() -> Self {
        Config {
            timeout: 30,
            tick_delay: 10,
            log_file_path: r"#C:\Users\turtl\Software\MultiMC\instances\1.12.2#".to_string(),
            chat_regex: "Position in queue:".to_string(),
            toast_settings: ToastSettings {
                send_on_login: true,
                send_on_logout: true,
                send_on_position_change: true,
            },
        }
    }
}

impl Sandbox for SettingsWindow {
    type Message = Message;

    fn new() -> Self {
        SettingsWindow {
            settings: Config::default(),
        }
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {} - Settings", VERSION)
    }

    fn update(&mut self, message: Self::Message) {}

    fn view(&mut self) -> Element<Message> {
        let content = Column::new()
            .padding(20)
            .push(Text::new("Settings").size(40).color(Color::WHITE))
            .push(Text::new("Settings").size(20).color(Color::WHITE))
            .push(Text::new("Settings").size(20).color(Color::WHITE))
            .push(Text::new("Settings").size(20).color(Color::WHITE))
            .push(Text::new("Settings").size(20).color(Color::WHITE));

        Container::new(content)
            .width(Length::Fill)
            .height(Length::Fill)
            .center_x()
            .center_y()
            .style(style::Theme::Dark)
            .into()
    }
}
