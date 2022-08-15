use std::path::Path;

use home::home_dir;
use iced::Space;
use iced::{
    button, executor, scrollable, slider, text_input, time, Align, Application, Button, Checkbox,
    Clipboard, Color, Column, Command, Container, Element, Length, Radio, Row, Rule, Scrollable,
    Slider, Text, TextInput,
};

use regex::Regex;

use super::assets;
use super::common;
use super::queue;
use super::settings::Config;
use super::settings::ConfigUpdate;
use super::style::TextColor;
use super::style::Theme;
use super::CFG_PATH;
use super::VERSION;

#[derive(Default)]
pub struct Queue {
    position: queue::Queue,
    queue_color: Color,
    config: Config,
    view: View,

    // Ui elements
    settings_button: button::State,

    // Config Stuff
    old_config: Option<Config>,
    config_scroller: scrollable::State,
    save_button: button::State,
    exit_button: button::State,
    reset_button: button::State,

    timeout_slider: slider::State,
    online_timeout_slider: slider::State,
    log_file_path_input: text_input::State,
    chat_regex_input: text_input::State,
}

#[derive(Debug, Clone)]
pub enum Message {
    SettingsUpdate(ConfigUpdate),
    UpdateTheme(Theme),
    OpenSettings,
    ConfigReset,
    ConfigSave,
    ConfigExit,
    Tick,

    None,
}

enum View {
    Queue,
    Settings,
}

impl Application for Queue {
    type Executor = executor::Default;
    type Message = Message;
    type Flags = ();

    fn new(_flags: ()) -> (Queue, Command<Message>) {
        let config_path = home_dir().unwrap().join(Path::new(CFG_PATH));

        let config = match Config::load(config_path.clone()) {
            Some(config) => {
                println!("[*] Successfully Read Config");
                config
            }
            None => {
                match config_path.exists() {
                    false => println!("[*] Config File Not Found. Using Defaults"),
                    true => println!("[*] Error Parsing Config File. Using Defaults"),
                }
                Config::default()
            }
        };

        (
            Self {
                position: queue::Queue::Offline,
                queue_color: Color::from_rgb8(191, 97, 106),
                config,
                ..Default::default()
            },
            Command::none(),
        )
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {}", VERSION)
    }

    fn update(&mut self, message: Message, _clipboard: &mut Clipboard) -> Command<Message> {
        match message {
            Message::OpenSettings => {
                self.view = View::Settings;
                self.old_config = Some(self.config.clone());
            }

            Message::UpdateTheme(theme) => {
                self.config.theme = theme;

                // :P
                if theme == Theme::Light {
                    msgbox::create(
                        "you have a problem",
                        "light mode... really?",
                        msgbox::IconType::Info,
                    )
                    .unwrap();
                }
            }

            Message::SettingsUpdate(config_update) => {
                self.config = self.config.apply_update(config_update);
            }

            Message::ConfigSave => {
                self.config
                    .save(home_dir().unwrap().join(Path::new(CFG_PATH)));
                self.view = View::Queue;
            }

            Message::ConfigExit => {
                self.view = View::Queue;
                if let Some(i) = self.old_config.take() {
                    self.config = i;
                }
            }

            Message::ConfigReset => {
                self.config = Config::default();
            }

            Message::Tick => {
                let new_pos = queue::check(&self.config);

                if self.position == new_pos {
                    return Command::none();
                }

                self.queue_color = common::update_color(new_pos);
                self.position = new_pos;

                match new_pos {
                    queue::Queue::Offline => {
                        if self.config.toast_settings.send_on_logout {
                            common::send_basic_toast("❌ Disconnected\n＞︿＜")
                        }
                    }
                    queue::Queue::Online => {
                        if self.config.toast_settings.send_on_login {
                            common::send_basic_toast("✅ You have logged in!")
                        }
                    }
                    queue::Queue::Queue(i) => {
                        if self.config.toast_settings.send_on_position_change {
                            common::send_basic_toast(&format!("⏰ Queue: {}", i));
                        }
                    }
                }
            }

            Message::None => {}
        };
        Command::none()
    }

    fn subscription(&self) -> iced::Subscription<Self::Message> {
        time::every(std::time::Duration::from_millis(500)).map(|_| Message::Tick)
    }

    fn view(&mut self) -> Element<Message> {
        match self.view {
            View::Queue => Container::new(
                Column::new()
                    .padding(20)
                    .align_items(Align::Center)
                    .push(
                        Text::new(match self.position {
                            queue::Queue::Offline => "…".to_owned(),
                            queue::Queue::Online => "Online".to_owned(),
                            queue::Queue::Queue(i) => i.to_string(),
                        })
                        .size(200)
                        .color(self.queue_color)
                        .font(assets::QUEUE_FONT),
                    )
                    .push(
                        Button::new(&mut self.settings_button, Text::new("Settings").size(25))
                            .style(self.config.theme)
                            .on_press(Message::OpenSettings),
                    ),
            )
            .width(Length::Fill)
            .height(Length::Fill)
            .center_x()
            .center_y(),
            View::Settings => Container::new(
                Column::new()
                    .padding(20)
                    .spacing(10)
                    .push(
                        Text::new("Settings")
                            .size(40)
                            .color(self.config.theme.text_color()),
                    )
                    .push(
                        Scrollable::new(&mut self.config_scroller)
                            .spacing(10)
                            .push(
                                Row::new()
                                    .spacing(20)
                                    .push(
                                        Text::new("Timeout")
                                            .size(25)
                                            .color(self.config.theme.text_color())
                                            .width(Length::FillPortion(1)),
                                    )
                                    .push(
                                        Slider::new(
                                            &mut self.timeout_slider,
                                            0.0..=100.0,
                                            self.config.timeout as f64,
                                            |x| {
                                                Message::SettingsUpdate(ConfigUpdate::Timeout(
                                                    x as u64,
                                                ))
                                            },
                                        )
                                        .width(Length::FillPortion(4))
                                        .style(self.config.theme),
                                    )
                                    .push(Text::new(format!("[ {:0>3} ]", self.config.timeout))),
                            )
                            .push(
                                Row::new()
                                    .spacing(20)
                                    .push(
                                        Text::new("Online Timeout")
                                            .size(25)
                                            .color(self.config.theme.text_color())
                                            .width(Length::FillPortion(1)),
                                    )
                                    .push(
                                        Slider::new(
                                            &mut self.online_timeout_slider,
                                            0.0..=100.0,
                                            self.config.online_timeout as f64,
                                            |x| {
                                                Message::SettingsUpdate(
                                                    ConfigUpdate::OnlineTimeout(x as u64),
                                                )
                                            },
                                        )
                                        .width(Length::FillPortion(4))
                                        .style(self.config.theme),
                                    )
                                    .push(Text::new(format!(
                                        "[ {:0>3} ]",
                                        self.config.online_timeout
                                    ))),
                            )
                            .push(
                                Row::new()
                                    .spacing(20)
                                    .push(
                                        Text::new("Log File")
                                            .size(25)
                                            .color(self.config.theme.text_color())
                                            .width(Length::FillPortion(1)),
                                    )
                                    .push(
                                        TextInput::new(
                                            &mut self.log_file_path_input,
                                            "",
                                            &self.config.log_file_path,
                                            |x| {
                                                Message::SettingsUpdate(ConfigUpdate::LogFilePath(
                                                    x,
                                                ))
                                            },
                                        )
                                        .width(Length::FillPortion(4))
                                        .style(self.config.theme),
                                    ),
                            )
                            .push(
                                Row::new()
                                    .spacing(20)
                                    .push(
                                        Text::new("Chat Regex")
                                            .size(25)
                                            .color(self.config.theme.text_color())
                                            .width(Length::FillPortion(1)),
                                    )
                                    .push(
                                        TextInput::new(
                                            &mut self.chat_regex_input,
                                            "",
                                            self.config.chat_regex.as_str(),
                                            |x| match Regex::new(&x) {
                                                Ok(i) => Message::SettingsUpdate(
                                                    ConfigUpdate::ChatRegex(i),
                                                ),
                                                Err(_) => {
                                                    msgbox::create(
                                                        "you have a problem",
                                                        "light mode... really?",
                                                        msgbox::IconType::Info,
                                                    )
                                                    .unwrap();
                                                    Message::None
                                                }
                                            },
                                        )
                                        .width(Length::FillPortion(4))
                                        .style(self.config.theme),
                                    ),
                            )
                            .push(Rule::horizontal(16).style(self.config.theme))
                            .push(
                                Row::new()
                                    .spacing(10)
                                    .push(
                                        Text::new("Toasts").size(25).width(Length::FillPortion(2)),
                                    )
                                    .push(
                                        Checkbox::new(
                                            self.config.toast_settings.send_on_login,
                                            "Login",
                                            |x| {
                                                Message::SettingsUpdate(ConfigUpdate::SendOnLogin(
                                                    x,
                                                ))
                                            },
                                        )
                                        .width(Length::FillPortion(1))
                                        .style(self.config.theme),
                                    )
                                    .push(
                                        Checkbox::new(
                                            self.config.toast_settings.send_on_logout,
                                            "Logout",
                                            |x| {
                                                Message::SettingsUpdate(ConfigUpdate::SendOnLogout(
                                                    x,
                                                ))
                                            },
                                        )
                                        .width(Length::FillPortion(1))
                                        .style(self.config.theme),
                                    )
                                    .push(
                                        Checkbox::new(
                                            self.config.toast_settings.send_on_position_change,
                                            "Position Change",
                                            |x| {
                                                Message::SettingsUpdate(
                                                    ConfigUpdate::SendOnPositionChange(x),
                                                )
                                            },
                                        )
                                        .width(Length::FillPortion(1))
                                        .style(self.config.theme),
                                    ),
                            )
                            .push(Rule::horizontal(16).style(self.config.theme))
                            .push(
                                Row::new()
                                    .spacing(20)
                                    .push(
                                        Text::new("Theme")
                                            .size(25)
                                            .color(self.config.theme.text_color())
                                            .width(Length::Fill),
                                    )
                                    .push(Radio::new(
                                        Theme::Dark,
                                        "Dark",
                                        Some(self.config.theme),
                                        Message::UpdateTheme,
                                    ))
                                    .push(Radio::new(
                                        Theme::Light,
                                        "Light",
                                        Some(self.config.theme),
                                        Message::UpdateTheme,
                                    )),
                            ),
                    )
                    .push(Space::new(Length::Fill, Length::Fill))
                    .push(
                        Row::new()
                            .spacing(10)
                            .push(
                                Button::new(&mut self.save_button, Text::new("Save").size(25))
                                    .on_press(Message::ConfigSave)
                                    .style(self.config.theme),
                            )
                            .push(
                                Button::new(&mut self.reset_button, Text::new("Reset").size(25))
                                    .on_press(Message::ConfigReset)
                                    .style(self.config.theme),
                            )
                            .push(
                                Button::new(&mut self.exit_button, Text::new("Cancel").size(25))
                                    .on_press(Message::ConfigExit)
                                    .style(self.config.theme),
                            ),
                    ),
            )
            .width(Length::Fill)
            .height(Length::Fill)
            .center_x(),
        }
        .style(self.config.theme)
        .into()
    }
}

impl Default for View {
    fn default() -> Self {
        View::Queue
    }
}
