use std::env::consts;
use std::panic;
use std::path::Path;
use std::process;

use home::home_dir;
use iced::{
    button, slider, text_input, window, Align, Button, Checkbox, Color, Column, Container, Element,
    Length, Row, Sandbox, Settings, Slider, Text, TextInput,
};
use image;
use image::GenericImageView;
use msgbox;

#[macro_use]
mod common;
mod assets;
mod settings;
mod style;
use settings::Config;
use settings::ConfigUpdate;
use style::TextColor;

pub const VERSION: &str = "α3.0.0";

pub fn main() -> iced::Result {
    panic::set_hook(Box::new(|p| {
        eprintln!("{}", p);
        msgbox::create(
            "2B2T-Queue-Notifier Error",
            &format!(
                "{}\n{}\nCompile Time: {}\nPlatform: {} {}\nVersion: {}",
                p.to_string(),
                env!("GIT_INFO"),
                env!("COMPILE_TIME"),
                consts::OS,
                consts::ARCH,
                VERSION,
            ),
            msgbox::IconType::Error,
        )
        .unwrap_or_default();
        process::exit(-1);
    }));

    let icon = image::load_from_memory(assets::ICON).unwrap();

    Queue::run(Settings {
        window: window::Settings {
            size: (800, 400),
            min_size: Some((800, 400)),
            icon: Some(
                window::Icon::from_rgba(icon.to_rgba8().into_raw(), icon.width(), icon.height())
                    .unwrap(),
            ),
            ..Default::default()
        },
        default_font: Some(assets::MAIN_FONT_RAW),
        ..Settings::default()
    })
}

#[derive(Default)]
struct Queue {
    position: Option<u32>,
    theme: style::Theme,
    queue_color: Color,
    config: Config,
    view: View,

    settings_button: button::State,
    debug_button: button::State,

    // Config Stuff
    save_button: button::State,
    exit_button: button::State,
    reset_button: button::State,

    timeout_slider: slider::State,
    tick_delay_slider: slider::State,
    log_file_path_input: text_input::State,
    chat_regex_input: text_input::State,
}

#[derive(Debug, Clone)]
enum Message {
    SettingsUpdate(ConfigUpdate),
    SetPosition(u32),
    OpenSettings,
    ConfigSave,
    ConfigExit,
    ConfigReset,
}

enum View {
    Queue,
    Settings,
}

impl Sandbox for Queue {
    type Message = Message;

    fn new() -> Self {
        let config_path = home_dir()
            .unwrap()
            .join(Path::new(".2B2T-Queue-Notifier\\config.cfg"));

        let config = match Config::load(config_path) {
            Some(config) => {
                println!("[*] Successfully Read Config");
                config
            }
            None => {
                println!("[*] Config File Not Found. Using Defaults");
                Config::default()
            }
        };

        Self {
            position: None,
            queue_color: Color::from_rgb8(191, 97, 106),
            config,
            ..Default::default()
        }
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {}", VERSION)
    }

    fn update(&mut self, message: Message) {
        match message {
            Message::OpenSettings => {
                self.view = View::Settings;
            }

            Message::SetPosition(position) => {
                if self.position == Some(position) {
                    return;
                }

                self.position = Some(position);
                self.queue_color = update_color(position);
            }

            Message::SettingsUpdate(config_update) => {
                self.config = self.config.apply_update(config_update);
            }

            Message::ConfigExit => {
                self.view = View::Queue;
            }

            Message::ConfigReset => {
                self.config = Config::default();
            }

            _ => {
                panic!("Unhandled Event: {:?}", message);
            }
        }
    }

    fn view(&mut self) -> Element<Message> {
        let content = match self.view {
            View::Queue => Column::new()
                .padding(20)
                .align_items(Align::Center)
                .push(
                    Text::new(match self.position {
                        Some(position) => position.to_string(),
                        None => "…".to_string(),
                    })
                    .size(200)
                    .color(self.queue_color)
                    .font(assets::QUEUE_FONT),
                )
                .push(
                    Button::new(&mut self.settings_button, Text::new("Settings").size(25))
                        .style(self.theme)
                        .on_press(Message::OpenSettings),
                )
                .push(
                    Button::new(&mut self.debug_button, Text::new("Debug").size(25))
                        .style(self.theme)
                        .on_press(Message::SetPosition(self.position.unwrap_or(0) + 25)),
                ),

            View::Settings => Column::new()
                .padding(20)
                .spacing(20)
                .push(
                    Text::new("Settings")
                        .size(40)
                        .color(self.theme.text_color()),
                )
                .push(
                    Row::new()
                        .spacing(20)
                        .push(
                            Text::new("Timeout (SEC)")
                                .size(25)
                                .color(self.theme.text_color())
                                .width(Length::FillPortion(1)),
                        )
                        .push(
                            Slider::new(
                                &mut self.timeout_slider,
                                0.0..=100.0,
                                self.config.timeout as f64,
                                |x| Message::SettingsUpdate(ConfigUpdate::Timeout(x as u64)),
                            )
                            .width(Length::FillPortion(4))
                            .style(self.theme),
                        )
                        .push(Text::new(format!("[ {:0>3} ]", self.config.timeout))),
                )
                .push(
                    Row::new()
                        .spacing(20)
                        .push(
                            Text::new("Tick Delay (SEC)")
                                .size(25)
                                .color(self.theme.text_color())
                                .width(Length::FillPortion(1)),
                        )
                        .push(
                            Slider::new(
                                &mut self.tick_delay_slider,
                                0.0..=100.0,
                                self.config.tick_delay as f64,
                                |x| Message::SettingsUpdate(ConfigUpdate::TickDelay(x as u64)),
                            )
                            .width(Length::FillPortion(4))
                            .style(self.theme),
                        )
                        .push(Text::new(format!("[ {:0>3} ]", self.config.tick_delay))),
                )
                .push(
                    Row::new()
                        .spacing(20)
                        .push(
                            Text::new("Log File")
                                .size(25)
                                .color(self.theme.text_color())
                                .width(Length::FillPortion(1)),
                        )
                        .push(
                            TextInput::new(
                                &mut self.log_file_path_input,
                                "",
                                &self.config.log_file_path,
                                |x| {
                                    Message::SettingsUpdate(ConfigUpdate::LogFilePath(
                                        x.to_string(),
                                    ))
                                },
                            )
                            .width(Length::FillPortion(4))
                            .style(self.theme),
                        ),
                )
                .push(
                    Row::new()
                        .spacing(20)
                        .push(
                            Text::new("Chat Regex")
                                .size(25)
                                .color(self.theme.text_color())
                                .width(Length::FillPortion(1)),
                        )
                        .push(
                            TextInput::new(
                                &mut self.chat_regex_input,
                                "",
                                &self.config.chat_regex,
                                |x| Message::SettingsUpdate(ConfigUpdate::ChatRegex(x.to_string())),
                            )
                            .width(Length::FillPortion(4))
                            .style(self.theme),
                        ),
                )
                .push(
                    Row::new()
                        .spacing(10)
                        .push(Text::new("Toasts").size(25).width(Length::FillPortion(2)))
                        .push(
                            Checkbox::new(self.config.toast_settings.send_on_login, "Login", |x| {
                                Message::SettingsUpdate(ConfigUpdate::SendOnLogin(x))
                            })
                            .width(Length::FillPortion(1))
                            .style(self.theme),
                        )
                        .push(
                            Checkbox::new(
                                self.config.toast_settings.send_on_logout,
                                "Logout",
                                |x| Message::SettingsUpdate(ConfigUpdate::SendOnLogout(x)),
                            )
                            .width(Length::FillPortion(1))
                            .style(self.theme),
                        )
                        .push(
                            Checkbox::new(
                                self.config.toast_settings.send_on_position_change,
                                "Position Change",
                                |x| Message::SettingsUpdate(ConfigUpdate::SendOnPositionChange(x)),
                            )
                            .width(Length::FillPortion(1))
                            .style(self.theme),
                        ),
                )
                .push(
                    Row::new()
                        .spacing(10)
                        .push(
                            Button::new(&mut self.save_button, Text::new("Save").size(25))
                                .on_press(Message::ConfigSave)
                                .style(self.theme),
                        )
                        .push(
                            Button::new(&mut self.reset_button, Text::new("Reset").size(25))
                                .on_press(Message::ConfigReset)
                                .style(self.theme),
                        )
                        .push(
                            Button::new(&mut self.exit_button, Text::new("Cancel").size(25))
                                .on_press(Message::ConfigExit)
                                .style(self.theme),
                        ),
                ),
        };

        Container::new(content)
            .width(Length::Fill)
            .height(Length::Fill)
            .center_x()
            .center_y()
            .style(self.theme)
            .into()
    }
}

impl Default for View {
    fn default() -> Self {
        View::Queue
    }
}

fn update_color(pos: u32) -> Color {
    if pos >= 500 {
        return Color::from_rgb8(191, 97, 106);
    }

    if pos >= 250 {
        return Color::from_rgb8(235, 203, 139);
    }

    Color::from_rgb8(163, 190, 140)
}
